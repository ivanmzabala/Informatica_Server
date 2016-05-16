using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.ComponentModel;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[ScriptService]
public class WebService : System.Web.Services.WebService
{

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string confirmaEntregaDulce(string a)
    {
        //RegisterId you got from Android Developer.
        string deviceId = "APA91bFXgT6uHojJxxJ6JFlHwDATp7bcMjafO2E3qHt2oOn-TIiSQoG7OxPCRziW3E2nCMyI5hHO8ePVJDgJa8Y0rAp0lFRVdafTI_w_dstPUGBCGJayVkrRVOeCkayQrJ_UbdL7r9SZiJxJpAPkkFOlWsuQz6ZlQg";

        string message = "Demo Notification";
        string tickerText = "Patient Registration";
        string contentTitle = "Mensaje";
        string postData = "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
          "\"data\": {\"tickerText\":\"" + tickerText + "\", " +
                     "\"contentTitle\":\"" + contentTitle + "\", " +
                     "\"message\": \"" + message + "\"}}";

        //string response = SendGCMNotification("BrowserAPIKey", postData);


        string apiKey = "AIzaSyBTw6dsE3YkhvERyMULDr5W-ohQe-4sBkA"; 

        //string postDataContentType = "application/json; charset=utf-8"; 

        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

        //  
        //  MESSAGE CONTENT  
        byte[] byteArray = Encoding.UTF8.GetBytes(postData);

        //  
        //  CREATE REQUEST  
        HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
        Request.Method = "POST";
    //  Request.KeepAlive = false;  
    
        Request.ContentType = "application/json; charset=utf-8";
        Request.Headers.Add(string.Format("Authorization: key={0}", apiKey));
        Request.ContentLength = byteArray.Length;

        Stream dataStream = Request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        //  
        //  SEND MESSAGE  
        try
        {
            WebResponse Response = Request.GetResponse();

            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
            {
                return "Unauthorized - need new token";
            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                return "Response from web service isn't OK";
            }

            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string responseLine = Reader.ReadToEnd();
            Reader.Close();

            return responseLine;
        }
        catch (Exception e)
        {
        }
        return "error";
    }


    public static bool ValidateServerCertificate(
                                                 object sender,
                                                 X509Certificate certificate,
                                                 X509Chain chain,
                                                 SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }



}
