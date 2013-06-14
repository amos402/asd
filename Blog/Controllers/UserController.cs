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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private BlogEntities db = new BlogEntities();

        //
        // GET: /User/

        public ActionResult Index()
        {
            var user = from c in db.UserProfiles
                       join b in db.UserMetas
                       on c.UserId equals b.UserId
                       into userInfo
                       from a in userInfo.DefaultIfEmpty()
                       select new UserModel()
                       {
                           UserId = c.UserId,
                           UserName = c.UserName,
                           NickName = a.NickName,
                           Description = a.Description,
                           Icon = a.Icon
                       };

            return View(user.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            UserMeta usermeta = db.UserMetas.Find(id);
            if (usermeta == null)
            {
                return HttpNotFound();
            }
            return View(usermeta);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserMeta usermeta)
        {
            if (ModelState.IsValid)
            {
                db.UserMetas.Add(usermeta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usermeta);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            UserMeta usermeta = db.UserMetas.Find(id);

            if (usermeta == null)
            {
                string name = ModelHelper.GetUserNameForId(id);
                if (WebSecurity.UserExists(name))
                {
                    usermeta = new UserMeta();
                    usermeta.UserId = id;
                    db.UserMetas.Add(usermeta);
                    db.SaveChanges();
                    
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return View(db.GetUserModel(id));
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel usermeta)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["image1"];
                if (file != null)
                {
                    int len = file.ContentLength;
                    byte[] buf = new byte[len];
                    file.InputStream.Read(buf, 0, len);
                    Bitmap image = new Bitmap(file.InputStream);
                    MemoryStream ms = new MemoryStream();

                    int height = image.Height / (image.Width / 50);
                    Image smallImage = image.GetThumbnailImage(50, 50, new Image.GetThumbnailImageAbort(() => false), IntPtr.Zero);
                    smallImage.Save(ms, ImageFormat.Png);
                    usermeta.Icon = ms.ToArray();
                }

                db.Entry(usermeta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usermeta);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserMeta usermeta = db.UserMetas.Find(id);
            if (usermeta != null)
            {
                db.UserMetas.Remove(usermeta);
                db.SaveChanges();
                //return HttpNotFound();
            }
            SimpleMembershipProvider asd = Membership.Provider as SimpleMembershipProvider;
            if (asd.DeleteUser(ModelHelper.GetUserNameForId(id), true))
            {
                return Content(string.Empty);
            }

            return View(usermeta);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserMeta usermeta = db.UserMetas.Find(id);
            db.UserMetas.Remove(usermeta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult GetIcon(int? id)
        {
            
            string contentType = "image/png";
            try
            {
                var user = db.UserMetas.Find(id);
                if (user != null)
                {
                    byte[] buf = user.Icon;

                    if (buf != null && buf.Length > 0)
                    {
                        return File(buf, contentType);
                    }
                }
            }
            catch (Exception)
            {
                
                return File("~/Images/DefaultIcon.png", contentType);
            }

            return File("~/Images/DefaultIcon.png", contentType);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}