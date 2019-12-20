using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ator.Utility.Baidu
{
     
    public class IpService
    {
        static string appId = "15339038";
        static string appKey = "4GrDaBu84EQ5cjdANbub12KcIa5Fxagt";
        static string apiBaseUrl = "https://api.map.baidu.com/location/ip?ip={0}&ak={1}&coord=bd09ll";
        static HttpClient httpClient = new HttpClient();
        public static async  Task<string> GetIpCity(string ip)
        {
            string apiUrl = string.Format(apiBaseUrl, ip, appKey);
            string res = await httpClient.GetStringAsync(apiUrl);
            try
            {
                JObject jObject = JObject.Parse(res);
                string result = jObject["content"]["address_detail"]["province"].ToString() +" "+ jObject["content"]["address_detail"]["city"].ToString();
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
