using CsvHelper;
using GMap.NET;
using GMap.NET.MapProviders;
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
                if (adress == null || adress == "")
                {
                    reader.Close();
                    break;
                }
                string url = @"https://maps.googleapis.com/maps/api/geocode/xml?address=" + adress + "&key=" + GoogleMapProvider.Instance.ApiKey;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(dataStream);
                xmldoc.Save("GeocodeingResponse");
                XmlElement xRoot = xmldoc.DocumentElement;

                if (xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lat") == null)
                {
                    continue;
                }
                string lat = xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lat").InnerText;
                string lng = xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lng").InnerText;
                string place_id = xmldoc.SelectSingleNode("GeocodeResponse/result/place_id").InnerText;
                adress = xmldoc.SelectSingleNode("GeocodeResponse/result/formatted_address").InnerText;
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
            foreach (LandPoint current in Deliverer.Adresses)
            {
                if (adress == current.adress)
                {
                    return current.coordinates;
                }
            }
            throw new Exception("Invalid adress: unable to get it coordinates");
        }

        public static LandPoint SingleGeocode(string adress)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");



            if (adress == null || adress == "")
            {

                throw new Exception("Invalid geocode request");
            }
            string url = @"https://maps.googleapis.com/maps/api/geocode/xml?address=" + adress + "&key=" + GoogleMapProvider.Instance.ApiKey;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(dataStream);
            xmldoc.Save("GeocodeingResponse");
            XmlElement xRoot = xmldoc.DocumentElement;

            if (xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lat") == null)
            {
                throw new Exception("No geocoding responce");
            }
            string lat = xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lat").InnerText;
            string lng = xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lng").InnerText;
            string place_id = xmldoc.SelectSingleNode("GeocodeResponse/result/place_id").InnerText;
            adress = xmldoc.SelectSingleNode("GeocodeResponse/result/formatted_address").InnerText;
            double latitude = double.Parse(lat);
            double longitude = double.Parse(lng);
            PointLatLng point = new PointLatLng(latitude, longitude);
            LandPoint temp = new LandPoint(0, place_id, adress, point);
            Deliverer.Adresses.Add(temp);


            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            return temp;
        }
    }
    public class DelivererRoute// simple class to bond LandPoint and distances
    {
        public List<LandPoint> route { get; set; }
        public List<int> distanses { get; set; }
        public List<string> Tooltips { get; set; }

        public DelivererRoute(List<LandPoint> way, List<int> dist)
        {
            route = way;
            distanses = dist;
            Tooltips = new List<string>();
        }
    }

    [Serializable]
    public class Deliverer
    {
        public double capacity { get; set; }// how many can take in backpack; littres
        public double salary { get; set; }// how many costs one hour of work; grivnas
        public int speed { get; set; }// speed; meters per seconds
        public double volumecarrying { get; set; }// how many carries now
        public int allowedroadtype { get; set; }//currently is not used because api with road type isn't free for use
        public static LandPoint NovaPochta { get; set; }//location of warehouse
        public List <Box> CarryingNow { get; set; }//all packages in the backpack
        public static List<LandPoint> Adresses { get; set; } = new List<LandPoint>();//already known adresses; can be serialised
        public List<DelivererRoute> Deliverer_routes { get; set; }// routes that deliverer will follow
        public DateTime mytime { get; set; }// deliverer time
        public Deliverer() { }
        public Deliverer(double cap, double sal, int sp, double carry)
        {
            capacity = cap;
            salary = sal;
            speed = sp;
            volumecarrying = carry;
            CarryingNow = new List<Box>();
            mytime = new DateTime(2020, 2, 12, 9, 0, 0);
            Deliverer_routes = new List<DelivererRoute>();
        }
        public void GroupMyBoxes()
        {
            

            for(int i = 0; i < CarryingNow.Count; i++)
            {
                for (int j = i + 1; j < CarryingNow.Count ; j++)
                {
                    if ((CarryingNow[i].adress == CarryingNow[j].adress) && (CarryingNow[j].volume + CarryingNow[i].volume <= 1000))
                    {
                        CarryingNow[i] = CarryingNow[j] + CarryingNow[i];
                        CarryingNow.Remove(CarryingNow[j]);
                        
                        j--;
                    }
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
        /*(public void SchedulePrint(int [,] DistanceMatrix, List <int> way, string filename)
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
        }*/
        private List<Box> FillBack()
        {
            double volume = 0;
            List<Box> package = new List<Box>();
            for(int i = CarryingNow.Count - 1; i >= 0; i--)
            {
                if(volume + CarryingNow[i].volume <= capacity && package.Count <= 18)
                {
                    volume += CarryingNow[i].volume;
                    package.Add(TakeBox(i));
                }
            }
            return package;
        }
        private List<Box> FillFront()
        {
            double volume = 0;
            List<Box> package = new List<Box>();
            for(int i = 0; i < CarryingNow.Count; i++)
            {
                if(volume + CarryingNow[i].volume <= capacity && package.Count <= 18)
                {
                    volume += CarryingNow[i].volume;
                    package.Add(TakeBox(i));
                    
                    i--;
                }
            }
            return package;
        }
        public void RandomFill()
        {
            if(Adresses.Count == 0)
            {
                throw new Exception("No source adresses");
            }
            for(int i = 0; i < 50; i++)
            {
                LandPoint adress = Adresses[Program.random.Next(Adresses.Count)];
                double volume = Math.Round(Program.random.NextDouble() * 50);
                double mass = Math.Round(Program.random.NextDouble() * 50);
                Box newBox = new Box(volume, mass, i.ToString(), adress);
                AddBox(newBox);
            }
           

        }
        public void FileFill(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            var args1 = reader.ReadLine().Split(';');
            if((args1[0] != "Volume") || (args1[1] != "Weight") || (args1[2] != "Adress"))
            {
                reader.Close();
                throw new Exception("Invalid source file");
            }
            int counter = 0;
            while (true)
            {
                string Line = reader.ReadLine();
                if(Line == null)
                {
                    break;
                }
                string[] args = Line.Split(';');
                
                double volume = System.Convert.ToDouble(args[0]);
                double weight = System.Convert.ToDouble(args[1]);
                LandPoint location = new LandPoint();
                bool flag = false;
                foreach( LandPoint point in Adresses)
                {//trying to find adress in already loaded
                    if(point.adress == args[2])
                    {
                        flag = true;
                        location = point;
                        break;
                    }

                }
                if (!flag)
                {
                    location = LandPoint.SingleGeocode(args[2]);
                }
                Box temp = new Box(volume, weight, counter.ToString(), location);
                AddBox(temp);
                counter++;
            }
            reader.Close();
        }
        public void TransferWithCap(Deliverer source, double volume_cap, double mass_cap)
        {
            for(int i = 0; i < source.CarryingNow.Count; i++)
            {
                if(source.CarryingNow[i].mass <= mass_cap && source.CarryingNow[i].volume < volume_cap)
                {
                    AddBox(source.TakeBox(i));
                    i--;
                }
            }
        }
        public async void BuildRoutesAsync(string filename)
        {
            await Task.Run(() => BuildRoutes(filename));
        }
        public void BuildRoutes(string logfile_name)//build routes for all packages in backpack
        {
            List<FileLine> output = new List<FileLine>();
            
            while(CarryingNow.Count != 0)
            {
                //FileLine fileLine = new FileLine();
                List<Box> load = FillFront();
                if(load.Count == 0)
                {
                    throw new Exception("Logical error in the algorythm happened: there are boxes that courier can't handle");
                }
                List<LandPoint> request = new List<LandPoint>();
                request.Add(NovaPochta);
                foreach(Box box in load)
                {
                    request.Add(box.adress);
                }
                request.Add(NovaPochta);
                output.Add(new FileLine
                {
                    Packages = "",
                    Destination = "Warehouse",
                    Approach = mytime.ToLongTimeString()
                });
                var result = Mathematics.GetRoute(request);
                result.Tooltips.Add("\n" + mytime.ToLongTimeString());
                for (int i = 0; i < result.distanses.Count; i++){
                    int time = result.distanses[i] / speed;
                    mytime = mytime.AddSeconds(time);
                    if(i < load.Count)
                    {
                        mytime = mytime.AddMinutes(load[i].count * (Program.random.Next(10) + 5));//some time to give out boxes
                    }
                    else
                    {
                        mytime = mytime.AddMinutes(15);//rest in warehouse
                    }
                    string tooltip = "\n";
                    tooltip += mytime.ToLongTimeString() + "\n";
                    if(i < load.Count)
                    {
                        output.Add(new FileLine
                        {
                            Packages = load[i].number,
                            Destination = load[i].adress.adress,
                            Approach = mytime.ToLongTimeString()
                        });
                        tooltip += load[i].number;//boxes that were delivered
                        result.Tooltips.Add(tooltip);
                    }
                    else
                    {
                        output.Add(new FileLine
                        {
                            Packages = "",
                            Destination = "Warehouse",
                            Approach = mytime.ToLongTimeString()
                        });
                        tooltip += "Warehouse";
                        result.Tooltips.Add(tooltip);
                        result.Tooltips[0] += '\n' + mytime.ToLongTimeString();
                    }
                    

                }
                Deliverer_routes.Add(result);
                using (StreamWriter writer = new StreamWriter(logfile_name + ".csv"))
                {
                    using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        csvWriter.Configuration.Delimiter = ";";
                        csvWriter.WriteRecords(output);
                    }
                }
            }
        }
        public void LoadOutToFile(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {
                    List<FileLine> output = new List<FileLine>();
                    foreach(Box box in CarryingNow)
                    {
                        string adr = box.adress.adress;
                        FileLine line = new FileLine();
                        line.Destination = adr;
                        line.Packages = box.number;
                        line.Approach = "Warehouse";
                        output.Add(line);
                    }
                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(output);
                }
            }
            CarryingNow.Clear();
            volumecarrying = 0;
        }
    }

    class FileLine
    {
        public string Packages { get; set; }
        public string Destination { get; set; }
        public string Approach { get; set; }
    }
}
