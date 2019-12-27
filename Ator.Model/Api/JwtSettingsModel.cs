using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model.Api
{
    public class JwtSettingsModel
    {
        public string Issuer { get; set; }//Token颁发者

        public string Audience { get; set; }//Token使用者

        public string SecretKey { get; set; }//Token密钥

        public int EffectiveTime { get; set; } = 60 * 24;//单位分钟：这里默认Token有效时间【一天】
    }
}
