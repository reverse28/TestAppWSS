using System.Net.Http.Json;
using TestAppWSS.Domain.Entities;
using TestAppWSS.Services.Interfaces;

namespace TestAppWSS.WebApi.Clients
{
    public class DepartmentsClient : BaseClient, INodeData
    {
        public DepartmentsClient(HttpClient Client) : base(Client, "api")
        {

        }


        public Node AddNode(string name, int? pid)
        {
            var response = Post($"{Adress}/add", new Node
            {
                Name = name,
                ParentId = pid,
            });

            var department = response?.Content.ReadFromJsonAsync<Node>().Result;

            return department!;
        }


        public bool Delete(int Id)
        {
            var response = Get<bool>($"{Adress}/delete/{Id}");

            return response!;
        }


        public Node Edit(int id, string name)
        {
            var response = Post($"{Adress}/edit", new Node
            {
                Name = name,
            });

            var department = response?.Content.ReadFromJsonAsync<Node>().Result;

            return department!;
        }

        public string GeneratePath(Node node)
        {
            var response = Post($"{Adress}/path", node);

            var department = response?.Content.ReadFromJsonAsync<string>().Result;

            return department!;
        }

        public List<Node> GetNodesList()
        {
            var departments = Get<List<Node>>($"{Adress}/departments");
            return departments!;
        }


        public Node? Move(int nodeId, int newParentId)
        {
            var response = Post($"{Adress}/move", new Node
            {
                Id = nodeId,
                ParentId = newParentId
            });

            var department = response?.Content.ReadFromJsonAsync<Node>().Result;

            return department!;
        }

    }
}
