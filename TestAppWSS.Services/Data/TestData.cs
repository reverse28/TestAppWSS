using TestAppWSS.Domain.Entities;

namespace TestAppWSS.Services.Data
{
    internal class TestData
    {
        public static IEnumerable<Node> Departments { get; } = new[]
     {
             new Node
                {
                    Name = "Компания 1",
                    Depth = 1,
                    ParentId = null
                },
               new Node
                {
                    Name = "Компания 2",
                    Depth = 1,
                    ParentId = null
                },

                 new Node
                {
                    Name = "Департамент 1",
                    Depth = 2,
                    ParentId = 1
                },

                 new Node
                {
                    Name = "Департамент 2",
                    Depth = 2,
                    ParentId = 1
                },

                new Node
                {
                    Name = "Департамент 1",
                    Depth = 2,
                    ParentId = 2
                },

                new Node
                {
                    Name = "Отдел 1",
                    Depth = 3,
                    ParentId = 4
                },

                new Node
                {
                    Name = "Отдел 2",
                    Depth = 3,
                    ParentId = 4
                },

               new Node
                {
                    Name = "Отдел 3",
                    Depth = 3,
                    ParentId = 4
                },
        };
    }
}
