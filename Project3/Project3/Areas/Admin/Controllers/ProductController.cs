using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Project3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Project3.Areas.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment host;
        public ProductController(ApplicationDbContext _db, IHostingEnvironment _host)
        {
            db = _db;
            host = _host;
        }
        public IActionResult Index(int ?page)
        {
            int pagesize = 5;
            int pageIndex;
            pageIndex = page == null ? 1 : (int)page;
            var lstProduct = db.Products.Include(x => x.Category).ToList();
            int pageCount = lstProduct.Count / pagesize + (lstProduct.Count % page > 0 ? 1 : 0);
            // gui qua view
            ViewBag.pagesum = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(lstProduct.Skip((pageIndex - 1) * pagesize).Take(pagesize).ToList());
        }
        //action xu ly them san pham
        public IActionResult Create()
        {
            ViewBag.LstCategory = db.Categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product objProduct)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(objProduct);
                db.SaveChanges();
                TempData["succses"] = "product inserted success";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int id)
        {
            var objJproduct = db.Products.Find(id);
            if (objJproduct == null)
                return NotFound();
            ViewBag.LstCategory = db.Categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View(objJproduct);
        }
        [HttpPost]
        public IActionResult Edit(Product obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string path = Path.Combine(host.WebRootPath, @"images/products");
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    if (!String.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldFilePath = Path.Combine(host.WebRootPath, obj.ImageUrl);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    obj.ImageUrl = @"images/products/" + filename;
                }
                db.Products.Update(obj);
                db.SaveChanges();
                TempData["success"] = "Product Updated success";
                return RedirectToAction("Index");
            }
            ViewBag.LstCategory = db.Categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View(obj);
        }
        public IActionResult Delete(int id)
        {
            var objproduct = db.Products.Find(id);
            if (objproduct == null)
                return NotFound();
            if (!String.IsNullOrEmpty(objproduct.ImageUrl))
            {
                var oldFilePath = Path.Combine(host.WebRootPath, objproduct.ImageUrl);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            db.Products.Remove(objproduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
