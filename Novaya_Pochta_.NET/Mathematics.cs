using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Novaya_Pochta_.NET
{
    class Mathematics
    {
        public static int[,] GetDistanceMatrix(List<LandPoint> points)
        {
            int[,] matrix = new int[points.Count, points.Count];
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string origin = "Slobozhanska St, 35, Lyubotin, Ukraine+ON|Slobozhanska St, 51, Lyubotin, Ukraine+ON|Шевченка вул. 15, Lyubotyn, Харківська, 62433+ON|Provulok Sanitarnyy, 6, Liubotyn, Kharkivs'ka oblast, 62434+ON|Klavdii Shulzhenko St, 45, Liubotyn, Kharkivs'ka oblast, 62435+ON|Fisaka St, 4, Lybotin, Ukraine+ON|Academician Amosov St, 45, Lyubotin, Ukraine+ON|Gaidar St, 47, Lyubotin, Ukraine+ON|Chernyshevsky St, 13, Lyubotin, Ukraine+ON|Guards St, 24, Lyubotin, Ukraine+ON";
            /*foreach(LandPoint point in points)
            {
                string lat = point.coordinates.Lat.ToString();
                string lng = point.coordinates.Lng.ToString();
                origin += lat + ',' + lng;
                if (point != points.Last())
                {
                    origin += '|';
                }
            }*/
            string url = @"https://maps.googleapis.com/maps/api/distancematrix/xml?units=metric&origins=" + origin + " &destinations=" + origin + "&key=AIzaSyCXpTullgkzPeHlXt3pye1M0NX749xW3Q0";//нужно попробовать координаты точных адресов, а не взятых наобум
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();            
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(dataStream);
            xmldoc.Save("MatrixResponse");
            XmlElement xRoot = xmldoc.DocumentElement;
            int i = 0;
            int j = 0;
            foreach (XmlNode xNode in xRoot)
            {
                if (xNode.Name == "row")
                {
                    foreach (XmlNode childnode in xNode.ChildNodes)
                    {
                        foreach (XmlNode node in childnode.ChildNodes)
                        {
                            if (node.Name == "distance")
                            {
                                matrix[i, j] = System.Convert.ToInt32(node.FirstChild.InnerText);
                                j++;
                                //Console.Write(node.FirstChild.InnerText + "   ");
                            }
                        }
                    }
                    i++;
                    j = 0;
                    //Console.WriteLine();
                }
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            return matrix;
        }
        public static List<LandPoint> GetRoute(List<LandPoint> points)
        {
            return points;
        }
    }
}
