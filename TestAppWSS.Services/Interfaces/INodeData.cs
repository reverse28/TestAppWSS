using TestAppWSS.Domain.Entities;

namespace TestAppWSS.Services.Interfaces
{
    public interface INodeData
    {
        public Node? AddNode(string name, int? pid);

        public bool Delete(int? id);

        public Node? Edit(int id, string name);

        public Node? Move(int id, int parentId);

        public string GeneratePath(Node node);

    }
}
