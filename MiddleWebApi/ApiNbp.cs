using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text;

namespace MiddleWebApi
{
    class ApiNbp
    {

        string LINK_API = "http://api.nbp.pl/api/exchangerates/";

        public string createApiUrl(string control, string table = null, List<string> parameters = null) {
            string strParameters = (parameters != null) ? "/" + String.Join("/", parameters.ToArray()) : null;
            if(table == null)
                return LINK_API + $"{control}/{strParameters}";
            return LINK_API + $"{control}/{table}{strParameters}";
        }

        /// <summary>
        /// check the typology of api that we call and create the link to send to api
        /// </summary>
        /// <param name="control">typology of api</param>
        ///  /// <param name="parameters">parameter link</param>
        public List<RootObject> controlApiList(string control, string table, List<string> parameters = null)
        {

            string strParameters = (parameters != null) ? "/" + String.Join("/", parameters.ToArray()) : null;
            return this.returnApiListObj(LINK_API + $"{control}/{table}{strParameters}");
        }

        public RootObject controlApiObj(string control, string table, List<string> parameters = null)
        {
            //string strParameters = (parameters != null) ? "/" + String.Join("/", parameters.ToArray()) : null;
            string strParameters = (parameters != null) ? "/" + String.Join("/", parameters.ToArray()) : null;
            return this.returnApiObj(LINK_API + $"{control}/{table}{strParameters}");
        }

        private List<RootObject> returnApiListObj(string webLink)
        {

            string text = null;

            List<RootObject> rootObject = null;
            using (WebClient client = new WebClient())
            {
                try
                {
                    text = Encoding.UTF8.GetString(client.DownloadData(webLink));
                }
                catch (Exception ex)
                {
                    
                    //MessageBox.Show(ex.Message);
                    return rootObject;
                }

                return rootObject = RootObject.DeserializeArr_newton(text);
            }
        }

        private RootObject returnApiObj(string webLink)
        {

            string text = null;

            RootObject rootObject = null;
            using (WebClient client = new WebClient())
            {
                try
                {
                    text = Encoding.UTF8.GetString(client.DownloadData(webLink));
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    return rootObject;
                }
               
                return rootObject = RootObject.Deserialize_newton(text);
            }
        }
    }
}
