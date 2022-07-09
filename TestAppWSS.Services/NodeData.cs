using Microsoft.EntityFrameworkCore;
using TestAppWSS.DAL;
using TestAppWSS.Domain.Entities;
using TestAppWSS.Services.Interfaces;

namespace TestAppWSS.Services
{
    public class NodeData : INodeData
    {
        private readonly Database _dbContext;
        public NodeData(Database dbContext)
        {
            _dbContext = dbContext;
        }

        public Node? AddNode(string name, int? pid)
        {
            var node = new Node()
            {
                Name = name,
                ParentId = pid
            };

            // Устанавливаем глубину для узла
            if (node.ParentId == null)
                node.Depth = 1;
            else
            {
                var parentDepth = _dbContext.Departments.Find(node.ParentId);
                if (parentDepth == null)
                    return null;
                node.Depth = parentDepth.Depth + 1;
            }
            _dbContext.Add(node);
            _dbContext.SaveChanges();

            return node;
        }


        public bool Delete(int? id)
        {
            if (_dbContext.Departments.Find(id) is not null)
            {
                var node = _dbContext.Departments.Find(id);
                // Удаляем узел вместе с детишками
                RemoveChildren(node!);
                _dbContext.SaveChangesAsync();
            }

            return true;
        }


        public Node? Edit(int id, string name)
        {
            if (_dbContext.Departments.Find(id) is not null)
            {
                var node = _dbContext.Departments.Find(id);

                node!.Name = name;
                _dbContext.Update(node);
                _dbContext.SaveChanges();
                return node;
            }
            return null;
        }

        public Node? Move(int id, int parentId)
        {
            if (_dbContext.Departments.Find(parentId) is null || _dbContext.Departments.Find(id) is null)
                return null;

            // Проверяем, не совпадает ли родительский идентификатор или один из дочерних элементов текущего узла
            var childrenIds = GetChildrenIds(id, new List<int>());

            foreach (int childId in childrenIds)
            {
                if (parentId == childId)
                    return null;
            }

            var node = _dbContext.Departments.Find(id);

            var parentNode = _dbContext.Departments.Find(parentId);


            // Перемещаем узел
            if (parentId == 0)
            {
                node!.ParentId = null;
                node.Parent = null;
                node.Depth = 1;
            }
            else
                node!.ParentId = parentId;

            _dbContext.Update(node);
            _dbContext.SaveChanges();

            _dbContext.Departments.Load();

            // Обновление себя и детей
            UpdateChildrenDepth(node);

            _dbContext.SaveChanges();

            return node;
        }




        // Обновляем глубину для узла и всех его дочерних элементов
        private void UpdateChildrenDepth(Node node)
        {
            // обновляем значение глубины
            if (node.Parent != null)
                node.Depth = node.Parent.Depth + 1;
            else
                node.Depth = 1;

            _dbContext.Update(node);

            // повторяем для всех детишек
            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    UpdateChildrenDepth(child);
                }
            }
        }

        // Рекурсивное удаление дочерних элементов
        private void RemoveChildren(Node node)
        {
            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    RemoveChildren(child);
                }
            }

            _dbContext.Remove(node);
        }

        // Удаление дочерних элементов из списка
        private List<Node> RemoveChildrenFromList(List<Node> children, Node node)
        {
            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    children = RemoveChildrenFromList(children, child);
                    children.Remove(child);
                }
            }
            else
                children.Remove(node);

            return children;
        }

        // Получить весь список
        private async Task<List<Node>> GetNodesList()
        {
            List<Node> nodesList = new List<Node>();
            await _dbContext.Departments.LoadAsync();
            var rootNodes = await _dbContext.Departments.Where(n => n.Depth == 1).ToListAsync();

            rootNodes = rootNodes.OrderByDescending(n => n.Name).ToList();
            foreach (var node in rootNodes)
            {
                // Add all roots with children
                nodesList.Add(node);
                ToNodesList(nodesList);
            }

            return nodesList;
        }

        // Получить идентификатор родителя и ребенка
        private List<int> GetChildrenIds(int id, List<int> childrenIds)
        {
            var node = _dbContext.Departments.AsNoTracking().Where(n => n.Id == id).Include(n => n.Children).FirstOrDefault();
            if (node?.Children?.Count != 0)
            {
                foreach (var child in node?.Children!)
                {
                    childrenIds = GetChildrenIds(child.Id, childrenIds);
                }
                childrenIds.Add(id);
            }
            else
                childrenIds.Add(id);

            return childrenIds;
        }




        // Рекурсивно получаем всех детей для списка узлов
        private List<Node> ToNodesList(List<Node> nodeList)
        {
            foreach (var node in nodeList)
            {
                if (node.Children != null)
                {
                    node.Children = node.Children.OrderBy(n => n.Name).ToList();
                    foreach (var child in node.Children)
                    {
                        nodeList.Add(child);
                        nodeList = ToNodesList(nodeList);
                    }
                    return nodeList;
                }
                else
                {
                    return nodeList;
                }
            }

            return null!;
        }


        // Создаем путь для более удобной навигации
        public string GeneratePath(Node node)
        {
            string path = node.Name + "/";
            while (node.Parent != null)
            {
                path = node.Parent.Name + "/" + path;
                node.Parent = node.Parent.Parent;
            }
            return "/" + path;
        }
    }
}
