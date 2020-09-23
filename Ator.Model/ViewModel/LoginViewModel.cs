﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model
{
    public class LoginViewModel
    {
        #region Attribute

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Display(Name = "验证码")]
        public string PIN { get; set; }

        /// <summary>
        /// 登录Ip
        /// </summary>
        [Display(Name = "登录Ip")]
        public string Ip { get; set; }

        /// <summary>
        /// 跳转页
        /// </summary>
        [Display(Name = "跳转页")]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 是否跳转
        /// </summary>
        [Display(Name = "是否跳转")]
        public string IsRedict { get; set; } = "0";

        /// <summary>
        /// 记住我按钮（点击记住我该值变为1，将保持一周的登陆时间）
        /// </summary>
        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
        
        #endregion
    }
}
