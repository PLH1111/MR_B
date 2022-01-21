using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Monitor.Common
{
    public class SerialHelper
    {
        private const string Key = "12345678";

        public static T ConvertToObject<T>(string path)
        {
            var str = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<T>(EncodeHelper.DESDecrypt(str, Key));

            //return JsonConvert.DeserializeObject<T>(str);//旧代码

        }

        public static string ConvertToStr(object obj)
        {
            var str = JsonConvert.SerializeObject(obj);

            return EncodeHelper.DESEncrypt(str, Key);

            //return str;//旧代码

        }

    }
}
