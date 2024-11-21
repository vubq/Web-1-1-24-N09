using BT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace BT.Controllers
{
    [Route("Admin/Student")]
    public class StudentController : Controller
    {
        private List<Student> studentList = new List<Student>();

        public StudentController()
        {
            studentList = new List<Student>()
            {
                new Student()
                {
                    Id = 101, Name = "Hải Nam", Branch = Branch.IT, Gender = Gender.Male, IsRegular = true, Address = "A1-2018", Email = "nam@g.com"
                },
                new Student()
                {
                    Id = 102, Name = "Minh Tú", Branch = Branch.BE, Gender = Gender.Female, IsRegular = true, Address = "A1-2019", Email = "tu@g.com"
                },
                new Student()
                {
                    Id = 103, Name = "Hoàng Phong", Branch = Branch.CE, Gender = Gender.Male, IsRegular = false, Address = "A1-2020", Email = "phong@g.com"
                },
                new Student()
                {
                    Id = 104, Name = "Xuân Mai", Branch = Branch.EE, Gender = Gender.Female, IsRegular = false, Address = "A1-2021", Email = "mai@g.com"
                }
            };
        }

        [Route("List")]
        public IActionResult Index()
        {
            return View(studentList);
        }

        [HttpGet]
        [Route("Add")]
        public IActionResult Create()
        {
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "1" },
                new SelectListItem { Text = "BE", Value = "2" },
                new SelectListItem { Text = "CE", Value = "3" },
                new SelectListItem { Text = "EE", Value = "4" }
            };
            return View();
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Create(Student s, IFormFile Avatar)
        {
            {
                if (Avatar != null && Avatar.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Avatar.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Avatar.CopyTo(fileStream);
                    }
                    s.Avatar = "/uploads/" + uniqueFileName;
                }

                if (ModelState.IsValid)
                {
                    s.Id = studentList.Last<Student>().Id + 1;
                    studentList.Add(s);
                    return View("Index", studentList);

                }
                ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();

                ViewBag.AllBranches = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "IT", Value = "1" },
                    new SelectListItem { Text = "BE", Value = "2" },
                    new SelectListItem { Text = "CE", Value = "3" },
                    new SelectListItem { Text = "EE", Value = "4" }
                };
                return View();
            }
        }
    }
}
