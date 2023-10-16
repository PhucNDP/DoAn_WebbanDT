using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.Areas.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private ApplicationDbContext db;


        public CategoryController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var dstheloai_c1 = (from x in db.Categories select x).ToList();

            var dstheloai_c2 = db.Categories.ToList();

            return View(dstheloai_c2);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["success"] = "Category inserted success";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int id)
        {
            //truy van the loai theo id
            var objCategory = db.Categories.Find(id);
            if (id == null)
            {
                return NotFound();
            }//tra ve view edit
            return View(objCategory);
        }
        [HttpPost]
        public IActionResult Edit(Category objCategory)
        {
            if (ModelState.IsValid) //kiem tra hop le du lieu
            {
                //cập nhật vao category vao CSDL
                db.Categories.Update(objCategory);
                //hoặc lệnh db.Entry<Category>(objCategory).State = EntityState.Modified;
                db.SaveChanges();
                TempData["success"] = "Category inserted success";
                //chuyen huong den action Index
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            //truy van the loai theo id
            var objCategory = db.Categories.Find(id);
            if (objCategory == null)
            {
                return NotFound();
            }
            //tra ve view edit
            return View(objCategory);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var objCategory = db.Categories.Find(id);
            if (objCategory == null)
            {
                return NotFound();
            }
            //xoá
            db.Categories.Remove(objCategory);
            db.SaveChanges();
            TempData["success"] = "Category inserted success";
            return RedirectToAction("Index");
        }
    }
  }
