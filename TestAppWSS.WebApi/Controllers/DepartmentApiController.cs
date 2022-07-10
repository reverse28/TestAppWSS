using Microsoft.AspNetCore.Mvc;
using TestAppWSS.Services.Interfaces;

namespace TestAppWSS.WebApi.Controllers
{
    public class DepartmentApiController
    {
        [Route("api")]
        [ApiController]
        public class DepartmentsApiController : ControllerBase
        {
            private readonly INodeData _NodeData;

            public DepartmentsApiController(INodeData NodeData)
            {
                _NodeData = NodeData;
            }


            [HttpGet("departments")]
            public IActionResult GetDepartments()
            {
                var departments = _NodeData.GetNodesList();

                return Ok(departments);
            }


            [HttpPost]
            [Route("add/{pid?}")]
            public IActionResult Add(string name, int Id)
            {
                var department = _NodeData.AddNode(name, Id);

                return CreatedAtAction(nameof(Add), new { department!.Id }, department);
            }


            [Route("delete/{id?}")]
            public IActionResult DeleteDepartment(int Id)
            {
                var result = _NodeData.Delete(Id);

                return Ok(result);
            }



            [Route("edit/{id?}")]
            public IActionResult Edit(int id, string name)
            {
                var result = _NodeData.Edit(id, name);

                return Ok(result);
            }


            [Route("move/{id?}")]
            public IActionResult Move(int id, int parentId)
            {
                var result = _NodeData.Move(id, parentId);

                return Ok(result);
            }

        }
    }
}
