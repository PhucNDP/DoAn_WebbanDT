using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Project3.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;
        public HomeController(ApplicationDbContext _db, ILogger<HomeController>logger)
        
        {
            _logger = logger;
           db = _db;
        }
        public IActionResult Index(int? page)
        {
            //int pagesize = 9;
            //int pageIndex;
            //pageIndex = page == null ? 1 : (int)page;
            //var lstProduct = db.Products.ToList();
            //int pageCount = lstProduct.Count / pagesize + (lstProduct.Count % page > 0 ? 1 : 0);
            //// gui qua view
            //ViewBag.pagesum = pageCount;
            //ViewBag.pageIndex = pageIndex;
            //return View(lstProduct.Skip((pageIndex - 1) * pagesize).Take(pagesize).ToList());
            {
                int pageSize = 9;
                int pageIndex;
                pageIndex = page == null ? 1 : (int)page;

                var listProduct = db.Products.ToList();
                //int pageCount = (int)Math.Ceiling((double)listProduct.Count / pageSize);
                int pageCount = listProduct.Count / pageSize + (listProduct.Count % pageSize > 0 ? 1 : 0);
                ViewBag.PageSum = pageCount;
                ViewBag.PageIndex = pageIndex;
                return View(listProduct.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
            }
        }

        public IActionResult Detail(int id)
        {
            var objProduct = db.Products.Find(id);
            if (objProduct == null)
            {
                return NotFound();
            }
            ViewBag.LIST_PRODUCT = db.Products.Where(x => x.CategoryId == objProduct.CategoryId && x.Id != id).Take(5).ToList();
            return View(objProduct);
        }
        

    }
}
