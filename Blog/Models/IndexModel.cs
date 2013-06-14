using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Blog.Models
{
    public class IndexModel
    {
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }

        public static IndexModel GetPostModel(string keyword)
        {
            BlogEntities db = new BlogEntities();
            IQueryable<Post> post;
            IQueryable<Comment> comment;
            IndexModel model = new IndexModel();
            if (Roles.IsUserInRole("Admin"))
            {
                post = from c in db.Posts
                       orderby c.Date descending
                       where c.Title.Contains(keyword)
                       select c;

                comment = (from c in db.Comments
                           orderby c.Date descending
                           select c).Take(5);
            }
            else if (Roles.IsUserInRole("User"))
            {
                post = from c in db.Posts
                       orderby c.Date descending
                       where c.Title.Contains(keyword) && c.Status != "private"
                       select c;


                comment = (from c in db.Comments
                           where c.Post.Status != "private"
                           orderby c.Date descending
                           select c).Take(5);
            }
            else
            {
                post = from c in db.Posts
                       orderby c.Date descending
                       where c.Title.Contains(keyword) && c.Status == "public"
                       select c;

                comment = (from c in db.Comments
                           where c.Post.Status == "public"
                           orderby c.Date descending
                           select c).Take(5);
            }

            var asd = (from c in post
                       select c.Comments).ToList();


            model.Posts = post.ToList();
            model.Comments = comment.ToList();
            return model;
        }
    }
}