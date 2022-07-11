using TestAppWSS.Domain.Entities;

namespace TestAppWSS.Services.Interfaces
{
    public interface INodeData
    {
        public IEnumerable<Node> GetNodesList();

        public Node? GetById(int? id);

        public Node? AddNode(Node node);

        public bool Delete(int id);

        public Node? Edit(Node node);

        public Node? Move(Node node);

        public string GeneratePath(Node node);

        public byte[] ExportXml();

        public bool ImportXml(byte[] array);

    }
}
