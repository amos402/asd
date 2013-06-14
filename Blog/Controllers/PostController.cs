using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using WebMatrix.WebData;
using System.Web.Security;
using Blog.Filters;
using System.Collections.Specialized;
using System.Configuration;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostController : Controller
    {
        private BlogEntities db = new BlogEntities();
        public static readonly List<SelectListItem> itemList = new List<SelectListItem>();

        public PostController()
        {
            if (itemList.Count == 0)
            {
                itemList.Add(new SelectListItem { Text = "公开", Value = "public" });
                itemList.Add(new SelectListItem { Text = "仅好友", Value = "protect" });
                itemList.Add(new SelectListItem { Text = "私有", Value = "private" });
            }
            ViewBag.Status = itemList;
            ViewBag.Categories = db.Categories.AsEnumerable();
        }

        //
        // GET: /Post/

        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        //
        // GET: /Post/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Post post = db.Posts.Find(id);
            ViewData["IndexModel"] = db.GetIndexModel();
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // GET: /Post/Create
        public ActionResult Create()
        {
           
            return View();
        }

        //
        // POST: /Post/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                if (Request["Category"] != null)
                {
                    int[] CategoryId = Request.Form.GetValues("Category").Select((T) => int.Parse(T)).ToArray();
                    foreach (var item in CategoryId)
                    {
                        post.Categories.Add(db.Categories.Find(item));
                    } 
                }
                post.UserId = WebSecurity.CurrentUserId;
                db.Posts.Add(post);
                db.SaveChanges();
                int asda = post.Id;
                return RedirectToAction("Details", new { id = post.Id });
            }

            return View(post);
        }

        //
        // GET: /Post/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Post/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                Post curPost = db.Posts.Find(post.Id);
                curPost.Categories.Clear();
                if (Request["Category"] != null)
                {
                    int[] CategoryId = Request.Form.GetValues("Category").Select((T) => int.Parse(T)).ToArray();
                   
                    foreach (var item in CategoryId)
                    {
                        curPost.Categories.Add(db.Categories.Find(item));
                    } 
                }
                curPost.Title = post.Title;
                curPost.Content = post.Content;
                curPost.Status = post.Status;
                //db.Entry(post).State = EntityState.Modified;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        //
        // GET: /Post/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return Content(string.Empty);
        }

        //
        // POST: /Post/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }




        public ActionResult View1()
        {
            return View();
        }
    }
}