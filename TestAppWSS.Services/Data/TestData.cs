using TestAppWSS.Domain.Entities;

namespace TestAppWSS.Services.Data
{
    internal class TestData
    {
        public static IEnumerable<Node> Departments { get; } = new[]
     {
             new Node
                {
                    Name = $"Компания",
                    Depth = 1,
                    DepthId=1,
                    ParentId = null
                },
               new Node
                {
                    Name = "Компания",
                    Depth = 1,
                    DepthId=2,
                    ParentId = null
                },

                 new Node
                {
                    Name = "Департамент",
                    Depth = 2,
                    DepthId=1,
                    ParentId = 1
                },

                 new Node
                {
                    Name = "Департамент",
                    Depth = 2,
                    DepthId=2,
                    ParentId = 1
                },

                   new Node
                {
                    Name = "Департамент",
                    Depth = 2,
                    DepthId=3,
                    ParentId = 1
                },

                new Node
                {
                    Name = "Департамент",
                    Depth = 2,
                    DepthId=1,
                    ParentId = 2
                },

                new Node
                {
                    Name = "Отдел",
                    Depth = 3,
                    DepthId=1,
                    ParentId = 4
                },

                new Node
                {
                    Name = "Отдел",
                    Depth = 3,
                    DepthId=2,
                    ParentId = 4
                },

               new Node
                {
                    Name = "Отдел",
                    Depth = 3,
                    DepthId=3,
                    ParentId = 4
                },

                  new Node
                {
                    Name = "Отдел",
                    Depth = 3,
                    DepthId=1,
                    ParentId = 5
                },

                new Node
                {
                    Name = "Отдел",
                    Depth = 3,
                    DepthId=2,
                    ParentId = 5
                },
        };
    }
}
