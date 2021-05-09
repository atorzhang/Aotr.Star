using Ator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.IService
{
    public interface ISysUserService
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        string DoLogin(LoginViewModel loginViewModel);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        string DoRegiste(RegisteViewModel registeViewModel);

        string DoChangePwd(string UserName, string oldPwd, string  newPwd);

        List<XmSelectModel> GetRoleXmSelectList(string id);
    }
}
