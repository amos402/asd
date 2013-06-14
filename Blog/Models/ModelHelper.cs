using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;
namespace Blog.Models
{
    public static class ModelHelper
    {
        public static IndexModel GetIndexModel(this BlogEntities db, string keyword = "")
        {
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

        public static UserModel GetUserModel(this BlogEntities db, int id)
        {
            return (from c in db.UserProfiles
                    join b in db.UserMetas
                    on c.UserId equals b.UserId
                    into userInfo
                    from a in userInfo.DefaultIfEmpty()
                    where a.UserId == id
                    select new UserModel()
                    {
                        UserId = c.UserId,
                        UserName = c.UserName,
                        NickName = a.NickName,
                        Description = a.Description,
                        Icon = a.Icon
                    }).Single();
        }

        public static string GetUserNameForId(int id)
        {
            var ship = Membership.Provider as SimpleMembershipProvider;

            return ship.GetUserNameFromId(id);
        }

        public static string GetNickNameForId(int id)
        {
            string name;
            using (BlogEntities db = new BlogEntities())
            {
                var user = db.UserProfiles.Find(id);
                if (user.UserMeta != null && user.UserMeta.NickName != null)
                {
                    name = user.UserMeta.NickName;
                }
                else
                {
                    name = user.UserName;
                }

            }
            return name;
        }

        public static string[] GetRolesForUser(int id)
        {
            var role = Roles.Provider as SimpleRoleProvider;
            return role.GetRolesForUser(GetUserNameForId(id));
        }

        public static string[] GetAllUsers()
        {
            var ship = Membership.Provider as SimpleMembershipProvider;
            int count;

            var users = ship.GetAllUsers(0, 0, out count);

            var userList = from c in users.Cast<MembershipUser>()
                           select c.UserName;
            return userList.ToArray();
        }

        public static bool HasNext<T>(this IEnumerable<T> enumer)
        {

            return true;
        }
    }
}