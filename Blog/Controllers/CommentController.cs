using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using WebMatrix.WebData;

namespace Blog.Controllers
{
    public class CommentController : Controller
    {
        private BlogEntities db = new BlogEntities();

        //
        // GET: /Comment/

        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Post);
            return View(comments.ToList());
        }

        //
        // GET: /Comment/Details/5

        public ActionResult Details(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // GET: /Comment/Create

        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Status");
            return View();
        }

        //
        // POST: /Comment/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.UserId = WebSecurity.CurrentUserId;
                db.Comments.Add(comment);
                db.SaveChanges();

                return Redirect(Request.Url.ToString());
            }

            ViewBag.PostId = new SelectList(db.Posts, "Id", "Status", comment.PostId);
            return View(comment);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReply(Reply reply)
        {
            if (ModelState.IsValid)
            {
                reply.UserId = WebSecurity.CurrentUserId;
                db.Replies.Add(reply);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Posts, "Id", "Status", reply.CommentId);
            return View(reply);
        }

        //
        // GET: /Comment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Status", comment.PostId);
            return View(comment);
        }

        //
        // POST: /Comment/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return Content(comment.Content);
                //return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Status", comment.PostId);
            return View(comment);
        }

        //
        // GET: /Comment/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // POST: /Comment/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}