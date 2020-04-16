using GMap.NET;
using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Novaya_Pochta_.NET
{
    //[STAThread]
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            GoogleMapProvider.Instance.ApiKey = "AIzaSyCXpTullgkzPeHlXt3pye1M0NX749xW3Q0";
            //StreamWriter output = new StreamWriter("output");
            //output.WriteLine("Hello world");
            //output.Close();

            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Deliverer Warehouse = new Deliverer(100000, 0, 0, 0);
            //LandPoint.ReadAndGeocode("PossibleAdresses.txt");
            XmlSerializer formatter = new XmlSerializer(typeof(List<LandPoint>));
            using (FileStream fs = new FileStream("SomeAdresses.xml", FileMode.Open))
            {
                Deliverer.Adresses.Clear();
                Deliverer.Adresses = (List<LandPoint>)formatter.Deserialize(fs);
                //formatter.Serialize(fs, Deliverer.Adresses);

                //Console.WriteLine("Объект сериализован");
            }
            Application.SetCompatibleTextRenderingDefault(false);
            Interface myForm = new Interface();
            for (int k = 0; k < 5; k++)
            {
                List<LandPoint> request = new List<LandPoint>();
                for (int i = 0; i < 10; i++)
                {
                    int randnum = random.Next(Deliverer.Adresses.Count);
                    if (request.Contains(Deliverer.Adresses[randnum]))
                    {
                        i--;
                    }
                    else
                    {
                        request.Add(Deliverer.Adresses[randnum]);
                    }
                    
                }
                request.Add(request.First());
                request = Mathematics.GetRoute(request);
                myForm.Car.Add(request);
            }
            
            /*int[,] matrix = Mathematics.GetDistanceMatrix(request);
            StreamWriter output = new StreamWriter("output");
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    output.Write("{0, 5}  ", matrix[i, j]);
                }
                output.WriteLine();
            }
            output.Close();
            */


           
            
            Application.EnableVisualStyles();
            
            Application.Run(myForm);

        }
    }
}
