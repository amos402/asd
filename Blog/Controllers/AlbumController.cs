using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using WebMatrix.WebData;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Blog.Controllers
{
    public class AlbumController : Controller
    {
        private BlogEntities db = new BlogEntities();

        //
        // GET: /Album/

        public ActionResult Index()
        {
            return View(db.Albums.ToList());
        }

        //
        // GET: /Album/Details/5

        public ActionResult Details(int id = 0)
        {
            string type = "image/jpeg";
            Album image = db.Albums.Find(id);

            return File(image.Image, image.Type);
        }

        //
        // GET: /Album/Create

        public ActionResult Create()
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            itemList.Add(new SelectListItem { Text = "公开", Value = "public" });
            itemList.Add(new SelectListItem { Text = "仅好友", Value = "protect" });
            itemList.Add(new SelectListItem { Text = "私有", Value = "private" });
            ViewBag.Status = itemList;
            return View();
        }

        //
        // POST: /Album/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                album.UserId = WebSecurity.CurrentUserId;

                HttpPostedFileBase file = Request.Files["image1"];
                if (file != null)
                {
                    int len = file.ContentLength;
                    byte[] buf = new byte[len];
                    file.InputStream.Read(buf, 0, len);
                    album.Image = buf;
                    album.Type = file.ContentType;
                }

                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(album);
        }

        //
        // GET: /Album/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        //
        // POST: /Album/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(album);
        }

        //
        // GET: /Album/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        //
        // POST: /Album/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public ActionResult GetImage(int? id)
        {
            string type = "image/jpeg";
            byte[] buf = db.Albums.Find(id).Image;
            Image image = Image.FromStream(new MemoryStream(buf));
            int height = image.Height / (image.Width / 150);
            Image smallImage = image.GetThumbnailImage(150, height, new Image.GetThumbnailImageAbort(()=>false), IntPtr.Zero);
            MemoryStream ms = new MemoryStream();
            smallImage.Save(ms, ImageFormat.Jpeg);
            
            return File(ms.ToArray(), type);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        public static Bitmap Zoom(Bitmap B, int size, int quality)           //不进行裁剪直接进行缩放
        {
            Bitmap transfer;

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            Image initImage = B;

            //原始图片的宽、高
            int initWidth = initImage.Width;
            int initHeight = initImage.Height;

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= size && initImage.Height <= size)
            {
                return B;
            }
            else
            {
                //缩略图对象
                System.Drawing.Image resultImage = new Bitmap(size, size);
                System.Drawing.Graphics resultG = Graphics.FromImage(resultImage);
                //设置质量
                resultG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                resultG.SmoothingMode = SmoothingMode.HighQuality;
                //用指定背景色清空画布
                resultG.Clear(Color.White);
                //绘制缩略图
                resultG.DrawImage(initImage, new Rectangle(0, 0, size, size), new Rectangle(0, 0, initWidth, initHeight), GraphicsUnit.Pixel);

                //关键质量控制
                //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo i in icis)
                {
                    if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                    {
                        ici = i;
                    }
                }
                EncoderParameters ep = new EncoderParameters(1);
                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                transfer = new Bitmap((Bitmap)resultImage);

                //释放关键质量控制所用资源
                ep.Dispose();

                //释放缩略图资源
                resultG.Dispose();
                resultImage.Dispose();

                //释放原始图片资源
                initImage.Dispose();
                return transfer;
            }
        }

        //public static Image ZoomImage(Image image, int width, int height)
        //{
            
        //}
    }
}