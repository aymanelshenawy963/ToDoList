using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TodoItemsController : Controller
    {
        ApplicationDbContext dbcontext = new();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(User user)
        {
            dbcontext.Users.Add(user);
            dbcontext.SaveChanges();
            var lastItem = dbcontext.Users.OrderByDescending(x => x.Id).FirstOrDefault();
            TempData["Name"] = lastItem.Name;
            return RedirectToAction(nameof(Items));
        }

        public IActionResult CreateNew()
        {          
            return View();
        }
        [HttpPost]
        public IActionResult CreateNew(UserData userdata , IFormFile Pdf)
        {
            if (Pdf.Length > 0)//99656
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Pdf.FileName);// 1.png
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Pdf", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    Pdf.CopyTo(stream);
                }
                userdata.Pdf = fileName;
            }
            dbcontext.UserDatas.Add(userdata);
            dbcontext.SaveChanges();
            TempData["Action"] = $"Created {userdata.Title} Successfully";
            return RedirectToAction(nameof(Items));
        }
        public IActionResult Items()
        {
            var userdata = dbcontext.UserDatas.ToList();
            //ViewBag.userdata = userdata;
            

            return View(model: userdata);
        }

        public IActionResult Edit(int UserDataId)
        {
            var user = dbcontext.UserDatas.Find(UserDataId);

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(UserData userData, IFormFile Pdf)
        {
            var oldProduct = dbcontext.UserDatas.AsNoTracking().FirstOrDefault(e => e.Id == userData.Id);

            if (Pdf != null && Pdf.Length > 0)//99656
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Pdf.FileName);// 1.png
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Pdf", fileName);
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Pdf", oldProduct.Pdf);

                using (var stream = System.IO.File.Create(filePath))
                {
                    Pdf.CopyTo(stream);
                }
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
                userData.Pdf = fileName;
            }
            else
            {
                userData.Pdf = oldProduct.Pdf;
            }
            dbcontext.UserDatas.Update(userData);
            dbcontext.SaveChanges();
            TempData["Action"] = $"Update {userData.Title} Successfully";
            return RedirectToAction(nameof(Items));
        }
        public IActionResult Delete(int UserDataId)
        {
            var userData = dbcontext.UserDatas.Find(UserDataId);
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Pdf", userData.Pdf);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            dbcontext.UserDatas.Remove(userData);
            dbcontext.SaveChanges();
            TempData["Action"] = $"Delete {userData.Title} Successfully";

            return RedirectToAction(nameof(Items));

        }

    }
}
