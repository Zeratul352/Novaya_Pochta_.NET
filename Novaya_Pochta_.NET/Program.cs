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
        public static Random random = new Random((int)DateTime.Now.Ticks);
        
        public static void LoadAdresses()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<LandPoint>));
            using (FileStream fs = new FileStream("Adresses_expanded.xml", FileMode.Open))
            {
                Deliverer.Adresses.Clear();
                Deliverer.Adresses = (List<LandPoint>)formatter.Deserialize(fs);
                //formatter.Serialize(fs, Deliverer.Adresses);

                //Console.WriteLine("Объект сериализован");
            }
            using (FileStream fs = new FileStream("SomeAdresses.xml", FileMode.Open))
            {
                List<LandPoint> old = (List<LandPoint>)formatter.Deserialize(fs);
                Deliverer.Adresses.AddRange(old);
            }
            using (FileStream fs = new FileStream("NovaPochtaAdress.xml", FileMode.Open))
            {
                List<LandPoint> old = (List<LandPoint>)formatter.Deserialize(fs);
                Deliverer.NovaPochta = old.First();
            }
        }
        public static void SetApiKey()
        {
            GoogleMapProvider.Instance.ApiKey = "AIzaSyCXpTullgkzPeHlXt3pye1M0NX749xW3Q0";
        }
        public static void MainOld()
        {
            SetApiKey();
            LoadAdresses();
            Deliverer Warehouse = new Deliverer(100000, 0, 0, 0);
            //LandPoint.ReadAndGeocode("PossibleAdresses.txt");
            
            Application.SetCompatibleTextRenderingDefault(false);
            Nova_pochta myForm = new Nova_pochta();
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
                //request = Mathematics.GetRoute(request).route;
                myForm.CurrentRoutes.Add(Mathematics.GetRoute(request));
                //myForm.Car.Add(Mathematics.GetGenetic(request));
            }

           

            Application.EnableVisualStyles();

            Application.Run(myForm);
        }
        [STAThread]
        static void Main(string[] args)
        {
            //MainOld();
            LoadAdresses();
            SetApiKey();
            Application.SetCompatibleTextRenderingDefault(false);
            Nova_pochta myForm = new Nova_pochta();
            Application.EnableVisualStyles();

            Application.Run(myForm);
            
        }
    }
}
