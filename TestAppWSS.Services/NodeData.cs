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

        public Node? AddNode(Node node)
        {
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


        public bool Delete(int id)
        {
            if (_dbContext.Departments.Find(id) is not null)
            {
                _dbContext.Departments.Load();
                var node = _dbContext.Departments.Find(id);
                // Удаляем узел вместе с детишками
                RemoveChildren(node!);
                _dbContext.SaveChangesAsync();
            }

            return true;
        }


        public Node? Edit(Node node)
        {
            _dbContext.Update(node);
            _dbContext.SaveChanges();
            return node;
        }

        public Node? Move(Node node)
        {
            // Проверяем, не совпадает ли родительский идентификатор и один из дочерних элементов текущего узла
            var childrenIds = GetChildrenIds(node.Id, new List<int>());

            foreach (int childId in childrenIds)
            {
                if (node.ParentId == childId)
                    return null;
            }

            _dbContext.Update(node);
            _dbContext.SaveChanges();

            _dbContext.Departments.Load();

            // Обновление себя и детишек
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


        // Получить весь список
        public IEnumerable<Node> GetNodesList()
        {
            List<Node> nodesList = new List<Node>();
            _dbContext.Departments.Load();
            var rootNodes = _dbContext.Departments.Where(n => n.Depth == 1).ToList();

            rootNodes = rootNodes.OrderBy(n => n.Name).ToList();
            foreach (var node in rootNodes)
            {
                // Добавляем узлы с дочерними элементами
                nodesList.Add(node);
                nodesList = ToNodesList(node, nodesList);
            }

            return nodesList;
        }

        // Получить весь список
        public Node? GetById(int? id)
        {
            _dbContext.Departments.Load();
            var node = _dbContext.Departments.FirstOrDefault(n => n.Id == id);

            return node;
        }



        // Получаем идентификаторы дочерних элементов
        public List<int> GetChildrenIds(int id, List<int> childrenIds)
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
        private List<Node> ToNodesList(Node node, List<Node> nodeList)
        {
            if (node.Children != null)
            {
                node.Children = node.Children.OrderBy(n => n.Name).ToList();
                foreach (var child in node.Children)
                {
                    nodeList.Add(child);
                    nodeList = ToNodesList(child, nodeList);
                }
                return nodeList;
            }
            else
            {
                return nodeList;
            }

        }


        // Создаем путь для более удобной навигации
        public string GeneratePath(Node node)
        {
            string path = node.Name + "/";
            var parent = GetById(node.ParentId);

            while (parent != null)
            {
                path = parent.Name + "/" + path;
                parent = parent.Parent;
            }
            return "/" + path;
        }
    }
}
