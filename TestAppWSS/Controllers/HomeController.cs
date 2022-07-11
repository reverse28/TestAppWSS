using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TestAppWSS.Domain.Entities;
using TestAppWSS.Models;
using TestAppWSS.Services.Interfaces;

namespace TestAppWSS.Controllers
{
    public class HomeController : Controller
    {
        INodeData _NodeData;

        public HomeController([FromServices] INodeData NodeData)
        {
            _NodeData = NodeData;
        }

        public IActionResult Index()
        {
            var deprtments = _NodeData.GetNodesList();

            return View(deprtments);
        }



        [HttpGet("Home/Add/{pid?}")]
        public IActionResult Add(int? pid)
        {
            if (pid == null)
                return NotFound();

            if (pid != 0)
            {
                var node = _NodeData.GetById(pid);

                if (node == null)
                    return NotFound();

                ViewData["Pid"] = node.Id;
                ViewData["Path"] = _NodeData.GeneratePath(node);
            }
            else
            {
                ViewData["Pid"] = null;
                ViewData["Path"] = "/";
            }

            return View();
        }

        [HttpPost("Home/Add/{pid?}")]
        public IActionResult Add([Bind("Name", "ParentId")] Node node)
        {
            if (ModelState.IsValid)
            {
                // Установить глубину для элемента
                if (node.ParentId == null)
                    node.Depth = 1;
                else
                {
                    var parentDepth = _NodeData.GetById(node.ParentId);
                    if (parentDepth == null)
                        return NotFound();
                    node.Depth = parentDepth.Depth + 1;
                }
                _NodeData.AddNode(node);
                return RedirectToAction(nameof(Index));
            }

            if (node.ParentId != 0)
            {
                node = _NodeData.GetById(node.ParentId);

                if (node == null)
                    return NotFound();

                ViewData["Pid"] = node.Id;
                ViewData["Path"] = _NodeData.GeneratePath(node);
            }
            else
            {
                ViewData["Pid"] = null;
                ViewData["Path"] = "/";
            }

            return View(node);
        }



        [Route("Home/Delete/{id?}")]
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var node = _NodeData.GetById(id);

            return View(node);
        }

        [HttpPost, ActionName("Delete")]
        [Route("Home/Delete/{id?}")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id == 0)
                return NotFound();

            _NodeData.Delete(id);

            return RedirectToAction(nameof(Index));
        }


        [Route("Home/Edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            var node = _NodeData.GetById(id);
            return View(node);
        }

        [HttpPost]
        [Route("Home/Edit/{id?}")]
        public IActionResult Edit(int id, string name)
        {
            var node = _NodeData.GetById(id);

            if (ModelState.IsValid)
            {
                ViewData["Error"] = "";
                var nodes = _NodeData.GetNodesList();
                if (nodes.Any(n => n.Name == name))
                {
                    ViewData["Error"] = "Компонент с таким именем уже существует в списке";
                    return View(node);
                }

                node!.Name = name;
                _NodeData.Edit(node);
                return RedirectToAction(nameof(Index));
            }

            return View(node);
        }


        [Route("Home/Move/{id?}")]
        public IActionResult Move(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var node = _NodeData.GetById(id);

            // Создаем список для выбора новго родителя, так как в категориях могут быть одинаковые имена, показываем полный путь
            var selectList = _NodeData.GetNodesList().Where(n => n.Id != id).Select(s => new Node()
            {
                Name = _NodeData.GeneratePath(s),
                Id = s.Id,
                ParentId = s.ParentId,
            }).ToList();

            selectList.Insert(0, new Node
            {
                Id = 0,
                Name = "/.",
                Depth = 0
            });

            ViewData["Parents"] = new SelectList(selectList, "Id", "Name");

            return View(node);
        }

        [HttpPost]
        [Route("Home/Move/{id?}")]
        public IActionResult Move(int id, int parentId)
        {
            var node = _NodeData.GetById(id);

            if (parentId != 0)
            {
                var parentNode = _NodeData.GetById(parentId);
            }

            if (ModelState.IsValid)
            {
                ViewData["Error"] = "";
                var nodes = _NodeData.GetNodesList().Where(n => n.ParentId == parentId);
                if (nodes.Any(n => n.Name == node!.Name))
                {
                    ViewData["Error"] = "Компонент с таким именем уже существует в списке, выберите другой путь или измените имя";
                    Move(parentId);
                    return View(node);
                }

                //Меняем родителя у элемента
                if (parentId == 0)
                {
                    node!.ParentId = null;
                    node.Parent = null;
                    node.Depth = 1;
                }
                else
                    node!.ParentId = parentId;


                _NodeData.Move(node);
                return RedirectToAction(nameof(Index));
            }

            return View(node);
        }



        [HttpPost, ActionName("ExportXml")]
        public IActionResult ExportXml()
        {
            var bytes = _NodeData.ExportXml();

            string file_type = "application/xml";
            string file_name = "nodes.xml";

            return File(bytes, file_type, file_name);
        }


        [HttpPost]
        public IActionResult ImportXml(IFormFile uploadedFile)
        {
            ViewData["Error"] = "";
            if (uploadedFile != null)
            {
                var fileStream = new MemoryStream();
                uploadedFile.CopyTo(fileStream);

                var result = _NodeData.ImportXml(fileStream.ToArray());

                if (!result)
                    ViewData["Error"] = "Во время загрузки произошла ошибка";
            }
            return RedirectToAction("Index");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}