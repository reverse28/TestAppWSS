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


        public Node AddNode(Node node)
        {
            var response = Post($"{Adress}/add",  node);

            var department = response?.Content.ReadFromJsonAsync<Node>().Result;

            return department!;
        }


        public bool Delete(int Id)
        {
            var response = Get<bool>($"{Adress}/delete/{Id}");

            return response!;
        }


        public Node Edit(Node node)
        {
            var response = Post($"{Adress}/edit", node);

            var department = response?.Content.ReadFromJsonAsync<Node>().Result;

            return department!;
        }

        public string GeneratePath(Node node)
        {
            var response = Post($"{Adress}/path", node);

            var department = response?.Content.ReadAsStringAsync().Result;

            return department!;
        }

        public IEnumerable<Node> GetNodesList()
        {
            var departments = Get<List<Node>>($"{Adress}/departments");
            return departments!;
        }

        public Node? GetById(int? id)
        {
            var response = Get<Node>($"{Adress}/getbyid/{id}");
            return response!;
        }

    public Node? Move(Node node)
        {
            var response = Post($"{Adress}/move", node);

            var department = response?.Content.ReadFromJsonAsync<Node>().Result;

            return department!;
        }

        public List<Node> RemoveChildrenFromList(List<Node> children, Node node)
        {

            var response = Post($"{Adress}/removechildren", children, node);

            var department = response?.Content.ReadFromJsonAsync<Node>().Result;

            return department!;
        }

        public List<int> GetChildrenIds(int id, List<int> childrenIds)
        {
            throw new NotImplementedException();
        }
    }
}
