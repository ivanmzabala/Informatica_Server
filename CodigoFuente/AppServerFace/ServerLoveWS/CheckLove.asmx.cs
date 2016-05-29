using Emgu.CV;
using Emgu.CV.Structure;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;

namespace ServerLoveWS
{
    /// <summary>
    /// Descripción breve de CheckLove
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class CheckLove : System.Web.Services.WebService
    {
        Respuesta Resultt = new Respuesta();
        [WebMethod]
        public Respuesta CheckLovePhoto(string img, string email, string idDevice)
        {

            Conection c = new Conection();
            MySqlCommand sqlCommand = new MySqlCommand();
            bool boolResult = true;
            int idUsuario = -1;
            string loveName = "";
            c.SQLConnection = new MySqlConnection();
            c.SQLConnection.ConnectionString = c.connectionString;

            string sql = "SELECT idUsuario, nombreAmor FROM usuario WHERE correo = '" + email + "'";
            sqlCommand.Connection = c.SQLConnection;
            sqlCommand.CommandText = sql;
            c.SQLConnection.Open();
            MySqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                idUsuario = reader.GetInt32(0);
                loveName = reader.GetString(1);
            }
            else
            {
                boolResult = false;
                Resultt.State = "Error";
                Resultt.Description = "El usuario no existe";
            }
            c.SQLConnection.Close();

            if (boolResult)
            {
                string name = "";
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(img);
                    List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
                    List<string> labels = new List<string>();
                    using (var ms = new MemoryStream(imageBytes, 0,
                                             imageBytes.Length))
                    {
                        // Convert byte[] to Image
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        Bitmap bitmap = new Bitmap(ms);

                        string path = System.Web.Hosting.HostingEnvironment.MapPath
                            ("~");
                        Image<Gray, byte> TrainedFace = null;
                        HaarCascade face;
                        face = new HaarCascade(Path.Combine(path, "haarcascade_frontalface_default.xml"));

                        Image<Bgr, Byte> imageOr = new Image<Bgr, byte>(bitmap);
                        Image<Gray, Byte> gray = imageOr.Convert<Gray, byte>();
                        Image<Gray, Byte> result = null;

                        /*Inicio*/
                        //eye = new HaarCascade("haarcascade_eye.xml");
                        //Load of previus trainned faces and labels for each image
                        /*string[] lines = File.ReadAllLines(Path.Combine(path, "TransientStorage", email, "TrainedLabels.txt"));
                        string Labelsinfo = lines[1];
                        string[] Labels = Labelsinfo.Split('%');
                        int NumLabels = Convert.ToInt16(lines[0]);
                        string LoadFaces;

                        for (int tf = 1; tf < NumLabels + 1; tf++)
                        {
                            LoadFaces = "face" + tf + ".bmp";
                            trainingImages.Add(new Image<Gray, byte>(Path.Combine(path, "TransientStorage", email, LoadFaces)));
                            labels.Add(Labels[tf]);
                        }*/

                        sql = "SELECT imagen FROM serverlove.imagenamor where idUsuario = " + idUsuario;
                        //string sql = "SELECT imagen, nombreAmor FROM serverlove.imagenamor JOIN serverlove.usuario ON serverlove.usuario.idUsuario = serverlove.imagenamor.idUsuario";
                        sqlCommand.Connection = c.SQLConnection;
                        sqlCommand.CommandText = sql;
                        c.SQLConnection.Open();
                        reader = sqlCommand.ExecuteReader();
                        int counter = 0;
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                counter++;
                                byte[] imageBytes2 = Convert.FromBase64String(reader.GetString(0));

                                using (var ms2 = new MemoryStream(imageBytes2, 0,
                                             imageBytes2.Length))
                                {
                                    labels.Add(loveName);

                                    // Convert byte[] to Image
                                    ms2.Write(imageBytes2, 0, imageBytes2.Length);
                                    Bitmap bitmap2 = new Bitmap(ms2);
                                    Image<Gray, byte> tmpImg = new Image<Gray, byte>(bitmap2);
                                    //tmpImg.Save(Path.Combine(path, "TransientStorage", "face" + counter + ".bmp"));
                                    trainingImages.Add(tmpImg);
                                }
                            }
                        }
                        c.SQLConnection.Close();
                        /*fin*/

                        if (trainingImages.Count > 0)
                        {

                            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                                  face,
                                  1.2,
                                  10,
                                  Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                  new Size(20, 20));


                            int t = 0;

                            //var i = 28;
                            //Action for each element detected

                            if (facesDetected[0].Length > 0)
                            {

                                foreach (MCvAvgComp f in facesDetected[0])
                                {
                                    t = t + 1;
                                    result = gray.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                                    //i++;
                                    //result.Save(Path.Combine(path, "TransientStorage", "face.bmp"));

                                    if (trainingImages.ToArray().Length != 0)
                                    {
                                        //TermCriteria for face recognition with numbers of trained images like maxIteration
                                        MCvTermCriteria termCrit = new MCvTermCriteria(labels.Count, 0.001);

                                        //Eigen face recognizer
                                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                                           trainingImages.ToArray(),
                                           labels.ToArray(),
                                           1500,
                                           ref termCrit);

                                        name = recognizer.Recognize(result);
                                        if (name.Length > 0)
                                        {
                                            string str_carSql;
                                            str_carSql = "UPDATE reconocimiento SET amorValido='1', idDevice='" + idDevice + "'Where idUsuario ='" + idUsuario + "'";
                                            sqlCommand.Connection = c.SQLConnection;
                                            sqlCommand.CommandText = str_carSql;
                                            c.SQLConnection.Open();

                                            sqlCommand.ExecuteNonQuery();

                                            c.SQLConnection.Close();
                                            string fun = POST(idUsuario);
                                            boolResult = true;


                                            Resultt.State = "Success" + fun;
                                            Resultt.Description = name;
                                        }
                                        else
                                        {
                                            string str_carSql;
                                            str_carSql = "UPDATE reconocimiento SET amorValido='0', idDevice='" + idDevice + "'Where idUsuario ='" + idUsuario + "'";
                                            sqlCommand.Connection = c.SQLConnection;
                                            sqlCommand.CommandText = str_carSql;
                                            c.SQLConnection.Open();

                                            sqlCommand.ExecuteNonQuery();

                                            c.SQLConnection.Close();

                                            boolResult = false;
                                            Resultt.State = "Error";
                                            Resultt.Description = "No es el amor ";

                                        }


                                    }

                                }
                                t = 0;
                            }
                            else
                            {
                                boolResult = false;
                                Resultt.State = "Error";
                                Resultt.Description = "No se ha detectado rostro en la imagen ";

                            }
                        }
                        else
                        {
                            boolResult = false;
                            Resultt.State = "Error";
                            Resultt.Description = "El usuario no ha registrado imagenes del amor";

                        }

                    }
                }
                catch (Exception e)
                {
                    boolResult = false;
                    Resultt.State = "Error";
                    Resultt.Description = "Ha ocurrido un problema por favor intente de nuevo" + e.ToString();

                }
            }


            return Resultt;
        }


        public string POST(int id)
        {


            string resultado = "";

            string url = "http://52.37.50.140:8080/hw.v1.entrega";

            string jsonContent = "{\"id\":" + id.ToString() + "}";


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                }
                resultado = " si envio";
            }
            catch (WebException ex)
            {
                resultado = "no envio"; // Log exception and throw as for GET example above
            }

            // return resultado;
            return resultado;

        }




    }
    public class Respuesta
    {
        public string State { get; set; }
        public string Description { get; set; }

    }
}
