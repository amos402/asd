using Blog.Filters;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Blog.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        private BlogEntities db = new BlogEntities();
        public static int PageSize { get; set; }

        public HomeController()
        {
            PageSize = 5;
        }

        public ActionResult Index(int? p, int? cat, string keyword = "")
        {
            IndexModel model = db.GetIndexModel();
            if (cat.HasValue)
            {
                model.Posts = (from c in db.Categories.Find(cat).Posts
                               join b in model.Posts
                               on c.Id equals b.Id
                               select b
                              ).ToList();
            }

            ViewBag.PageCount = model.Posts.Count % PageSize == 0 ?
                model.Posts.Count / PageSize : model.Posts.Count / PageSize + 1;

            if (p.HasValue)
            {
                ViewBag.CurrentPage = p.Value;
                model.Posts = model.Posts.Skip((p.Value - 1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                ViewBag.CurrentPage = 1;
                model.Posts = model.Posts.Take(PageSize).ToList();
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "关于程序.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }
    }
}
