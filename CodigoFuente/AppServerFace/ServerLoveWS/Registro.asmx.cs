using Emgu.CV;
using Emgu.CV.Structure;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ServerLoveWS
{
    /// <summary>
    /// Descripción breve de Registro
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Registro : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] addImage(string img, string email)
        {
            Object[] resultObj = SaveImage(img, email);
            String[] result = new String[2];
            bool success = Convert.ToBoolean(resultObj[0]);
            if (success)
            {
                result[0] = "Success";
            }
            else
            {
                result[0] = "Error";
            }

            result[1] = resultObj[1].ToString();
            return result;
        }


        private Object[] SaveImage(string base64String, string email)
        {
            Object[] Result = new Object[2];
            Boolean boolResult = true;
            Conection c = new Conection();
            MySqlCommand sqlCommand = new MySqlCommand();
            String Description = "";
            int idUsuario = -1;
            try
            {
                c.SQLConnection = new MySqlConnection();
                c.SQLConnection.ConnectionString = c.connectionString;

                string sql = "SELECT idUsuario FROM usuario WHERE correo = '" + email + "'";
                sqlCommand.Connection = c.SQLConnection;
                sqlCommand.CommandText = sql;
                c.SQLConnection.Open();
                idUsuario = (Int32)sqlCommand.ExecuteScalar();
                c.SQLConnection.Close();

            }
            catch (Exception)
            {
                Description = "Usuario no existe";
                boolResult = false;
            }
            if (boolResult)
            {

                try
                {
                    // Convert Base64 String to byte[]
                    byte[] imageBytes = Convert.FromBase64String(base64String);
                    using (var ms = new MemoryStream(imageBytes, 0,
                                             imageBytes.Length))
                    {
                        // Convert byte[] to Image
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        Bitmap bitmap = new Bitmap(ms);
                        /*string path = System.Web.Hosting.HostingEnvironment.MapPath
                            ("~/TransientStorage");
                        if (File.Exists(Path.Combine(path, filename)))
                        {
                            string extension = Path.GetExtension(filename);
                            string name = Path.GetFileNameWithoutExtension(filename);
                            int fileMatchCount = 1;
                            while (File.Exists(Path.Combine(path, name + "(" + fileMatchCount + ")" + extension)))
                                fileMatchCount++;
                            filename = name + "(" + fileMatchCount + ")" + extension;
                        }*/
                        // write the memory stream containing the original 
                        // file as a byte array to the filestream 
                        //bitmap.Save(Path.Combine(path, filename));
                        string path = System.Web.Hosting.HostingEnvironment.MapPath
                            ("~");
                        Image<Gray, byte> TrainedFace = null;
                        HaarCascade face;
                        face = new HaarCascade(Path.Combine(path, "haarcascade_frontalface_default.xml"));

                        Image<Bgr, Byte> imageOr = new Image<Bgr, byte>(bitmap);
                        Image<Gray, Byte> gray = imageOr.Convert<Gray, byte>();

                        //Face Detector
                        MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                        face,
                        1.2,
                        10,
                        Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                        new Size(20, 20));

                        //Action for each element detected
                        foreach (MCvAvgComp f in facesDetected[0])
                        {
                            TrainedFace = gray.Copy(f.rect).Convert<Gray, byte>();
                            break;
                        }
                        if (TrainedFace == null)
                        {
                            Description = "No se ha detectado rostro en la imagen";
                            boolResult = false;
                        }
                        else
                        {
                            //resize face detected image for force to compare the same size with the 
                            //test image with cubic interpolation type method
                            TrainedFace = TrainedFace.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                            using (MemoryStream ms2 = new MemoryStream())
                            {
                                // Convert Image to byte[]
                                Bitmap bitmapTrainedFace = TrainedFace.Bitmap;
                                bitmapTrainedFace.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imageBytes = ms2.ToArray();

                                // Convert byte[] to Base64 String
                                base64String = Convert.ToBase64String(imageBytes);

                                try
                                {
                                    string str_carSql;
                                    str_carSql = "INSERT INTO imagenamor (idUsuario,imagen) VALUES (" + idUsuario + ", '" + base64String + "')";

                                    sqlCommand.Connection = c.SQLConnection;
                                    sqlCommand.CommandText = str_carSql;
                                    c.SQLConnection.Open();

                                    sqlCommand.ExecuteNonQuery();

                                    c.SQLConnection.Close();
                                }
                                catch (Exception)
                                {
                                    boolResult = false;
                                    Description = "ocurrio un problema al guardar la imagen";

                                }

                            }

                            /*string rutaArchivo = Path.Combine(path, "TransientStorage", email, "TrainedLabels.txt");
                            if (!Directory.Exists(Path.GetDirectoryName(rutaArchivo)))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivo));
                                File.Create(rutaArchivo).Dispose();
                            }
                            string[] lines = File.ReadAllLines(rutaArchivo);
                            string labels = "%";
                            int faces = 0;
                            if (lines.GetLength(0) > 1)
                            {
                                faces = Int32.Parse(lines.ElementAt(0));
                                labels = lines.ElementAt(1);
                            }
                            else
                            {
                                lines = new string[2];
                            }
                            labels = labels + loveName + "%";
                            faces++;

                            lines[0] = faces.ToString();
                            lines[1] = labels;
                            File.WriteAllLines(rutaArchivo, lines);

                            TrainedFace.Save(Path.Combine(Path.GetDirectoryName(rutaArchivo), "face" + faces + ".bmp"));*/
                            boolResult = true;
                            Description = "Imagen guardada";
                        }
                    }
                }
                catch (Exception e)
                {
                    boolResult = false;
                    Description = "Ha ocurrido un problema por favor intente de nuevo";

                }
            }
            
            Result[0] = boolResult;
            Result[1] = Description;
            return Result;
        }
    }
}
