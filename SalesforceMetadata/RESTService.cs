using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


/*
 *    Code and concepts copied from: https://blog.mkorman.uk/integrating-net-and-salesforce-part-1-rest-api/
 *    
 *    NOT COMPLETE
 *    
 * 
 */

namespace SalesforceMetadata
{
    public partial class RESTService : System.Windows.Forms.Form
    {
        private const string LOGIN_ENDPOINT = "https://test.salesforce.com/services/oauth2/token";
        private const string API_ENDPOINT = "/services/data/v48.0/";

        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthToken { get; set; }
        public string InstanceUrl { get; set; }

        public RESTService()
        {
            InitializeComponent();
        }

        private void btnSendRequest_Click(object sender, EventArgs e)
        {
            if (this.cmbVerb.Text == "GET")
            {
                getRecordsAsync("GET");
            }
            else if (this.cmbVerb.Text == "POST")
            {
                getRecordsAsync("POST");
            }
        }

        public void salesforceLogin(String jsonResponseFile)
        {
            // client_id is the Consumer Key from the HHAPIServiceCloudApp Managed Connected App in TM4
            // client_secret is the Consumer Secret which you will need to click to reveal in the HHAPIServiceCloudApp Managed Connected App in TM4

            String jsonResponse;
            using (HttpClient client = new HttpClient())
            {
                FormUrlEncodedContent request = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"client_id", ""},
                    {"client_secret", ""},
                    {"username", this.tbUsername.Text},
                    {"password", this.tbPassword.Text + this.tbSecurityToken.Text}
                });

                request.Headers.Add("X-PrettyPrint", "1");
                HttpResponseMessage response = client.PostAsync(LOGIN_ENDPOINT, request).Result;
                jsonResponse = response.Content.ReadAsStringAsync().Result;
            }


            StreamWriter sw = new StreamWriter(jsonResponseFile);
            sw.Write(jsonResponse);
            sw.Close();
        }



        private async void getRecordsAsync(String verb)
        {
            String jsonResponseFile = "";

            Dictionary<String, String> jsonParsedResponse = new Dictionary<String, String>();
            salesforceLogin(jsonResponseFile);

            //{
            //    "access_token" : "00Dc0000003wY5Z!ARMAQG7KlkmfGdIJX0JD86ckA2wrPrXTLJh1gLa45XlRVEIoQGF06wnVdPL.U7LB5w3aMd_o9tK8GDTjaQCACz3GkImLSWqL",
            //    "instance_url" : "https://horizonhobby--tm4.my.salesforce.com",
            //    "id" : "https://test.salesforce.com/id/00Dc0000003wY5ZEAU/0056g000005AxqlAAC",
            //    "token_type" : "Bearer",
            //    "issued_at" : "1590335598763",
            //    "signature" : "c5KGRBdYvM29yhA//Pu0eQLeRH8t7prxp4roSBP7kk0="
            //}

            // Get the Access Token and the Instance URL from the JSON response using a parser (or write your own) to use in your query requests as the BEARER_TOKEN
            jsonParsedResponse = parseJSONResponse(jsonResponseFile);

            if (verb == "GET")
            {
                //String uri = jsonParsedResponse["instance_url"] + this.tbURI.Text;
                //String requestUri = jsonParsedResponse["instance_url"] + API_ENDPOINT + "sobjects";

                String requestUri = jsonParsedResponse["instance_url"] + API_ENDPOINT + "sobjects";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("Authorization", "Bearer " + jsonParsedResponse["access_token"]);   // AuthToken is the access_token returned from the login respnose
                //request.Headers.Add("X-PrettyPrint", "1");

                HttpResponseMessage response = await client.SendAsync(request);

                String asyncResponse = await response.Content.ReadAsStringAsync();

            }

        }

        //private void tbFileSaveLocation_DoubleClick(object sender, EventArgs e)
        //{
        //    // Select the Directory where the JSON results will be stored
        //    //FolderBrowserDialog fbd = new FolderBrowserDialog();
        //    //fbd.ShowDialog();

        //    //this.tbFileSaveLocation.Text = fbd.SelectedPath + "\\";

        //    this.tbFileSaveLocation.Text = UtilityClass.folderBrowserSelectPath("Select the Directory where the JSON results will be stored", true);
        //}


        private Dictionary<String, String> parseJSONResponse(String jsonResponseFile)
        {
            char ch;
            StreamReader reader = new StreamReader(jsonResponseFile);
            //StreamWriter sw = new StreamWriter(this.tbFileSaveLocation.Text + objectName + "_Page" + pageNumber.ToString() + ".csv");

            Dictionary<String, String> jsonParsedResponse = new Dictionary<String, String>();

            Boolean isKey = true;
            Boolean insideQuote = false;
            //Boolean newRecord = false;
            //Boolean endOfRecord = false;

            String strValue1 = "";
            String strValue2 = "";

            do
            {
                ch = (char)reader.Read();

                if (ch == '{')
                {
                    //endOfRecord = false;
                    strValue1 = "";
                    strValue2 = "";
                }
                else if (ch == '}')
                {
                    if (strValue1.Length > 0)
                    {
                        jsonParsedResponse.Add(strValue1, strValue2);
                    }

                    strValue1 = "";
                    strValue2 = "";

                    insideQuote = false;
                    isKey = false;
                    //endOfRecord = true;
                }
                else if (ch == ':')
                {
                    isKey = false;
                }
                else if (ch == ',')
                {
                    jsonParsedResponse.Add(strValue1, strValue2);
                    strValue1 = "";
                    strValue2 = "";
                    isKey = true;
                }
                else if (Convert.ToInt32(ch) == 34 && insideQuote == false)
                {
                    insideQuote = true;
                }
                else if (Convert.ToInt32(ch) == 34 && insideQuote == true)
                {
                    insideQuote = false;
                }
                else if (insideQuote == true && isKey == true)
                {
                    strValue1 += ch.ToString();
                }
                else if (insideQuote == true && isKey == false)
                {
                    strValue2 += ch.ToString();
                }

                //else if (ch == '[')
                //{
                //    sw.Write(Environment.NewLine);
                //}
                //else if (ch == ']')
                //{
                //    sw.Write(Environment.NewLine);
                //}
                //else if (ch == ':')
                //{
                //    isKey = false;
                //}
                //else if (insideQuote == false && endOfRecord == true)
                //{
                //    isKey = true;
                //    strValue1 = "";
                //    strValue2 = "";
                //}
                //else if (insideQuote == false)
                //{
                //    isKey = true;

                //    strValue1 = "";
                //    strValue2 = "";
                //}
            }
            while (!reader.EndOfStream);

            reader.Close();

            return jsonParsedResponse;

            //sw.Close();
            //sw.Dispose();


        }

    }
}
