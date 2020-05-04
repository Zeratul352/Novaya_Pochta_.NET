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
        public static void MainOld()
        {
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
                for (int i = 0; i < 8; i++)
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
                //request = Mathematics.GetRoute(request);
                //myForm.Car.Add(Mathematics.GetRoute(request));
                //myForm.Car.Add(Mathematics.GetGenetic(request));
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

            //Application.Run(myForm);
        }
        static void Main(string[] args)
        {
            MainOld();
            Deliverer warehouse = new Deliverer(10000, 0, 0, 0);
            warehouse.RandomFill();
            Deliverer bike_courier = new Deliverer(100, 100, 300, 0);
            Deliverer car_courier = new Deliverer(1000, 500, 600, 0);
            Deliverer lorry_courier = new Deliverer(3000, 2000, 600, 0);
            bike_courier.TransferWithCap(warehouse, 100, 5);
            car_courier.TransferWithCap(warehouse, 300, 30);
            lorry_courier.TransferWithCap(warehouse, 1000, 100);

            bike_courier.GroupMyBoxes();
            car_courier.GroupMyBoxes();
            lorry_courier.GroupMyBoxes();
            Console.ReadKey();
        }
    }
}
