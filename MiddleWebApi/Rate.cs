using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft;
using System.Text;
// using System.Web.Script.Serialization;

namespace MiddleWebApi
{
    public class Rate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public double mid { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public double bid { get; set; }
        public double ask { get; set; }


    }

    public class RootObject
    {

        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public List<Rate> rates { get; set; }

        public string currency { get; set; }
        public string code { get; set; }



        public static RootObject Deserialize_newton(string jSonString) => Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(jSonString);
        public static List<RootObject> DeserializeArr_newton(string jSonString) => Newtonsoft.Json.JsonConvert.DeserializeObject<List<RootObject>>(jSonString);

        public static RootObject Deserialize_Web(string jSonString)
        {
            List<RootObject> result = new List<RootObject>();
            // List<RootObject> result = new JavaScriptSerializer().Deserialize<List<RootObject>>(jSonString);
            return result[0];
        }

        public static List<RootObject> DeserializeArr_Web(string jSonString)
        {
            List<RootObject> result = new List<RootObject>();
            //List<RootObject> result = new JavaScriptSerializer().Deserialize<List<RootObject>>(jSonString);
            return result;
        }
    }
}
