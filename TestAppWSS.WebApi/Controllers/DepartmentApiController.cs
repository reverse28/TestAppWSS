using Microsoft.AspNetCore.Mvc;
using TestAppWSS.Domain.Entities;
using TestAppWSS.Services.Interfaces;

namespace TestAppWSS.WebApi.Controllers
{
        [Route("api")]
        [ApiController]
        public class DepartmentApiController : ControllerBase
        {
            private readonly INodeData _NodeData;

            public DepartmentApiController(INodeData NodeData)
            {
                _NodeData = NodeData;
            }


            [HttpGet("departments")]
            public IActionResult GetDepartments()
            {
                var departments = _NodeData.GetNodesList();

                return Ok(departments);
            }


        [HttpGet("getbyid/{id?}")]
        public IActionResult GetById(int Id)
        {
            var departments = _NodeData.GetById(Id);

            return Ok(departments);
        }
    

        [HttpPost]
            [Route("add")]
            public IActionResult Add(Node node)
            {
                var department = _NodeData.AddNode(node);

                return CreatedAtAction(nameof(Add), new { department!.Id }, department);
            }


            [HttpGet("delete/{id?}")]
            public bool DeleteDepartment(int Id)
            {
                var result = _NodeData.Delete(Id);

                return result;
            }


            [HttpPost]
            [Route("edit")]
            public IActionResult Edit(Node node)
            {
                var result = _NodeData.Edit(node);

                return Ok(result);
            }

            [HttpPost]
            [Route("move/{id?}")]
            public IActionResult Move(Node node)
            {
                var result = _NodeData.Move(node);

                return Ok(result);
            }



            [HttpPost]
            [Route("path/{id?}")]
            public IActionResult GeneratePath(Node node)
            {
                var result = _NodeData.GeneratePath(node);

                return Ok(result);
            }
        }
    }

