using GMap.NET;
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
            Random random = new Random(0);
            //StreamWriter output = new StreamWriter("output");
            //output.WriteLine("Hello world");
            //output.Close();

            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Deliverer Warehouse = new Deliverer(100000, 0, 0, 0);
            //LandPoint.ReadAndGeocode("PossibleAdresses.txt");
            XmlSerializer formatter = new XmlSerializer(typeof(List<LandPoint>));
            using (FileStream fs = new FileStream("FullGeocode.xml", FileMode.OpenOrCreate))
            {
                Deliverer.Adresses = (List<LandPoint>)formatter.Deserialize(fs);
                //formatter.Serialize(fs, Deliverer.Adresses);

                //Console.WriteLine("Объект сериализован");
            }
            List<LandPoint> request = new List<LandPoint>();
            for (int i = 0; i < 10; i++)
            {               
                request.Add(Deliverer.Adresses[i * 3]);
            }
            int[,] matrix = Mathematics.GetDistanceMatrix(request);
            StreamWriter output = new StreamWriter("output");
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    output.Write("{0, 5}  ", matrix[i, j]);
                }
                output.WriteLine();
            }
            output.Close();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Interface());

        }
    }
}
