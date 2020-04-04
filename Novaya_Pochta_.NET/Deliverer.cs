using GMap.NET;
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
    [Serializable]
    public class LandPoint
    {
        public string place_id { get; set; }
        public int id { get; set; }
        public static int count { get; set; } = 0;
        public string adress { get; set; }
        public PointLatLng coordinates { get; set; }
        public LandPoint() { }
        public LandPoint(int i, string place_code, string adr, double la, double ln)
        {
            id = i;
            place_id = place_code;
            adress = adr;
            coordinates = new PointLatLng(la, ln);
        }
        public LandPoint(int i, string place_code, string adr, PointLatLng point)
        {
            id = i;
            place_id = place_code;
            adress = adr;
            coordinates = point;
        }
        public static void ReadLandPointFromFile(string path)
        {
            List<LandPoint> landPoints = new List<LandPoint>();
            StreamReader reader = new StreamReader(path);
            
            while (true)
            {
                string adress, coordinates;
                //id = reader.ReadLine();
                adress = reader.ReadLine();
                if (adress == null)
                {
                    reader.Close();
                    return;
                }
                
                coordinates = reader.ReadLine();
                string[] args = coordinates.Split(' ');
                LandPoint temp = new LandPoint(count, count.ToString(), adress, Convert.ToDouble(args[0]), Convert.ToDouble(args[1]));
                Deliverer.Adresses.Add(temp);
                count++;
            }
        }
        public static void ReadAndGeocode(string path)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            StreamReader reader = new StreamReader(path);
            while (true)
            {
                string adress;
                adress = reader.ReadLine();
                if (adress == null)
                {
                    reader.Close();
                    break;
                }
                string url = @"https://maps.googleapis.com/maps/api/geocode/xml?address=" + adress + "&key=AIzaSyCXpTullgkzPeHlXt3pye1M0NX749xW3Q0";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(dataStream);
                xmldoc.Save("GeocodeingResponse");
                XmlElement xRoot = xmldoc.DocumentElement;
                //XmlNodeList geometry = xmldoc.SelectNodes("geometry");
                if(xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lat") == null)
                {
                    continue;
                }
                string lat = xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lat").InnerText;
                string lng = xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lng").InnerText;
                string place_id = xmldoc.SelectSingleNode("GeocodeResponse/result/place_id").InnerText;
                
                double latitude = double.Parse(lat);
                double longitude = double.Parse(lng);
                PointLatLng point = new PointLatLng(latitude, longitude);
                LandPoint temp = new LandPoint(count, place_id, adress, point);
                Deliverer.Adresses.Add(temp);
                count++;
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
        }
        public static PointLatLng GetCoordinates(string adress)
        {
            foreach(LandPoint current in Deliverer.Adresses)
            {
                if(adress == current.adress)
                {
                    return current.coordinates;
                }
            }
            throw new Exception("Invalid adress: unable to get it coordinates");
        }
    }
    [Serializable]
    public class Deliverer
    {
        public double capacity { get; set; }
        public double salary { get; set; }
        public double speed { get; set; }
        public double volumecarrying { get; set; }
        public int allowedroadtype { get; set; }
        public List <Box> CarryingNow { get; set; }
        public static List<LandPoint> Adresses { get; set; } = new List<LandPoint>();
        public DateTime mytime { get; set; }
        public Deliverer() { }
        public Deliverer(double cap, double sal, double sp, double carry)
        {
            capacity = cap;
            salary = sal;
            speed = sp;
            volumecarrying = carry;
            mytime = new DateTime(2020, 2, 12, 9, 0, 0);
        }
        public void GroupMyBoxes()
        {
            for(int i = 0; i < CarryingNow.Capacity - 1; i++)
            {
                for(int j = 0; j < CarryingNow.Capacity - i - 1; j++)
                {
                    if(CarryingNow[j].adress > CarryingNow[j + 1].adress)
                    {
                        Box.SwapBoxes(CarryingNow[j], CarryingNow[j + 1]);
                    }
                }
            }

            for(int i = 0; i < CarryingNow.Capacity - 1; i++)
            {
                if((CarryingNow[i + 1].adress == CarryingNow[i].adress) && (CarryingNow[i].volume + CarryingNow[i + 1].volume <= 1000))
                {
                    CarryingNow[i] = CarryingNow[i] + CarryingNow[i + 1];
                    CarryingNow.Remove(CarryingNow[i + 1]);
                    i--;
                }
            }
        }
        public void AddBox(Box b)
        {
            CarryingNow.Add(b);
            volumecarrying += b.volume;

        }
        public Box TakeBox(int j)
        {
            Box took = CarryingNow[j];
            CarryingNow.Remove(took);
            volumecarrying -= took.volume;
            return took;
        }
        public void VolumeSort()
        {
            int num = CarryingNow.Capacity;
            for(int i = 0; i < num - 1; i++)
            {
                for(int j = 0; j < num - i - 1; j++)
                {
                    if(CarryingNow[j].volume > CarryingNow[j + 1].volume)
                    {
                        Box.SwapBoxes(CarryingNow[j], CarryingNow[j + 1]);
                    }
                }
            }
        }
        public void SchedulePrint(int [,] DistanceMatrix, List <int> way, string filename)
        {
            StreamWriter output = new StreamWriter(filename, false);
            List<Box> NewCarry = new List<Box>();
            for(int i = 0; i < CarryingNow.Capacity; i++)
            {
                int adr = way[i + 1];
                int index = 0;
                for(int j = 0; j < CarryingNow.Capacity; j++)
                {
                    if(CarryingNow[j].adress == adr)
                    {
                        index = j;
                        break;
                    }
                }
                NewCarry.Add(CarryingNow[index]);
            }
            for(int i = 0; i < way.Capacity - 3; i++)
            {
                mytime.AddMinutes(15 + Math.Round(DistanceMatrix[way[i], way[i + 1]] / speed));
                output.WriteLine("{0, 25} | {1, 25} | {2}:{3}", NewCarry[i].number, Adresses[NewCarry[i].adress].adress, mytime.Hour, mytime.Minute);
            }

            mytime.AddMinutes(15 + Math.Round(DistanceMatrix[way.Capacity - 3, way.Capacity - 2] / speed));
            output.WriteLine("{0, 50} | {1}:{2}", "Warehouse", mytime.Hour, mytime.Minute);
            output.Close();
        }
        public void FillBack(Deliverer donor)
        {
            for(int i = donor.CarryingNow.Capacity - 1; i >= 0; i--)
            {
                if(volumecarrying + donor.CarryingNow[i].volume <= capacity)
                {
                    AddBox(donor.TakeBox(i));
                }
            }
        }
        public void FillFront(Deliverer donor)
        {
            for(int i = 0; i < donor.CarryingNow.Capacity; i++)
            {
                if(volumecarrying + donor.CarryingNow[i].volume <= capacity)
                {
                    AddBox(donor.TakeBox(i));
                    i--;
                }
            }
        }
        public void FileFill(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            int count = Convert.ToInt32(reader.ReadLine());
            for(int i = 0; i < count; i++)
            {
                string vol;
                string adr;
                vol = reader.ReadLine();
                double volume = Convert.ToDouble(vol);
                adr = reader.ReadLine();
                int id = 0;
                for(int j = 0; j < Adresses.Capacity; j++)
                {
                    if(Adresses[j].adress == adr)
                    {
                        id = Adresses[j].id;
                        break;
                    }
                }
                AddBox(new Box(volume, Convert.ToString(i + 1), id));
            }
            reader.Close();
        }

    }
}
