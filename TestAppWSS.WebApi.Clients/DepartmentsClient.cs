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
            var response = Post($"{Adress}/add", node);

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


        public byte[] ExportXml()
        {
            var response = Get<byte[]>($"{Adress}/exportxml");

            return response!;
        }

        public bool ImportXml(byte[] arr)
        {
            var response = Post($"{Adress}/importxml", arr);

            var result = response!.Content.ReadFromJsonAsync<bool>().Result;

            return result;
        }

    }
}
