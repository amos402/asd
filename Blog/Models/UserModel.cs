using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "昵称")]
        public string NickName { get; set; }

        [Display(Name = "角色")]
        public string[] Roles
        {
            get
            {
                return ModelHelper.GetRolesForUser(UserId);
            }
        }

        [Display(Name = "个人说明")]
        public string Description { get; set; }

        [Display(Name = "头像")]
        public byte[] Icon { get; set; }

        [Display(Name = "密码")]
        public string Password { get; set; }


    }
}