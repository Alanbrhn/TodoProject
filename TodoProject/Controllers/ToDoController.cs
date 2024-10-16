using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoProject.Models;
using TodoProject.Services;

namespace TodoProject.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        public async Task<IActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            if (userIdString == null || userName == null)
            {
                return RedirectToAction("Login", "User"); 
            }

            int userId = int.Parse(userIdString);

            var toDos = await _toDoService.GetToDos(userId);

            var viewModel = new ToDoViewModel
            {
                UserId = userId,
                UserName = userName,
                ToDos = toDos
            };

            return View(viewModel);
        }



        [HttpGet]
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "User");
            }

            ViewBag.UserName = userName;
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(ToDo toDo)
        {
            
            var userIdString = HttpContext.Session.GetString("UserId");
            if (userIdString == null)
            {
                return RedirectToAction("Login", "User"); 
            }

            if (int.TryParse(userIdString, out int userId))
            {
                toDo.UserId = userId;

                if (!string.IsNullOrWhiteSpace(toDo.Subject) && !string.IsNullOrWhiteSpace(toDo.Description))
                {
                   
                    if (string.IsNullOrWhiteSpace(toDo.ActivityNo))
                    {
                        toDo.ActivityNo = toDo.GenerateActivityNo(); 
                    }

                  
                    if (string.IsNullOrWhiteSpace(toDo.Status))
                    {
                        toDo.Status = "Unmarked"; 
                    }

                    await _toDoService.CreateToDo(toDo); 
                    return RedirectToAction("Index"); 
                }
                else
                {
                    ViewBag.ErrorMessage = "Subject and Description are required fields.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid User ID."; // Handle invalid UserId scenario
            }

            return View(toDo); // Return to the Create view with an error message
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var toDo = await _toDoService.GetToDoById(id);
            if (toDo == null || toDo.Status != "Unmarked")
            {
                return NotFound();
            }
            return View(toDo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ToDo toDo)
        {
            // Check if the ToDo object has required fields populated
            if (!string.IsNullOrWhiteSpace(toDo.Subject) && !string.IsNullOrWhiteSpace(toDo.Description))
            {
                // Proceed with the update
                await _toDoService.UpdateToDo(toDo);
                return RedirectToAction("Index"); // Redirect to Index after updating
            }

            // Handle the case where required fields are missing
            ViewBag.ErrorMessage = "Subject and Description are required fields.";

            return View(toDo); // Return to the Edit view with an error message
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _toDoService.DeleteToDo(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Mark(int id, string status)
        {
            await _toDoService.MarkToDoStatus(id, status);
            return RedirectToAction("Index");
        }
    }


}
