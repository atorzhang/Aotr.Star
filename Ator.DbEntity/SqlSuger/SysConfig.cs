using Ator.Utility.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.DbEntity.SqlSuger
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SysConfig
    {
        /// <summary>
        /// 系统配置Json文件路径
        /// </summary>
        private static readonly string ConfigPath = FileHelper.GetAbsolutePath("Jsons/Configs.json");

        #region 数据库配置

        /// <summary>
        /// 属性集合 使用:Params.属性名称
        /// </summary> 
        public static dynamic Params { get; set; }
        #endregion 

        #region 初始化系统配置 
        /// <summary>
        /// 初始化配置
        /// </summary> 
        public static void InitConfig()
        {
            try
            {
                if (!System.IO.File.Exists(ConfigPath))
                {
                    return;
                }
                try
                {
                    using (System.IO.StreamReader file = System.IO.File.OpenText(ConfigPath))
                    {
                        using (JsonTextReader reader = new JsonTextReader(file))
                        {
                            Params = JToken.ReadFrom(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
        #endregion

    }
}
