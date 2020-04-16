using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using System.Net;
using System.Xml;

namespace Novaya_Pochta_.NET
{
    public partial class Interface : Form
    {
        List<PointLatLng> RouteList = new List<PointLatLng>();
        int bikecounter = 0;
        int carcounter = 0;
        public List<List<LandPoint>> Car { get; set; } = new List<List<LandPoint>>();
        public List<List<LandPoint>> Bike { get; set; } = new List<List<LandPoint>>();
        //List<List<Tuple<PointLatLng, string, string>>> Bike = new List<List<Tuple<PointLatLng, string, string>>>();
        GMapOverlay CarOverlay = new GMapOverlay("CarOverlay");
        GMapOverlay BikeOverlay = new GMapOverlay("BikeOverlay");
        public Interface()
        {
            InitializeComponent();
            //LandPoint.ReadLandPointFromFile("C:/Users/Andrey/source/repos/Novaya_Pochta_.NET/Novaya_Pochta_.NET/DemoAdressList.txt");
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            
            // Настройки для компонента GMap
            gmap.Bearing = 0;
            // Перетаскивание правой кнопки мыши
            gmap.CanDragMap = true;
            // Перетаскивание карты левой кнопкой мыши
            gmap.DragButton = MouseButtons.Left;
            gmap.GrayScaleMode = true;
            // Все маркеры будут показаны
            gmap.MarkersEnabled = true;
            // Максимальное приближение
            gmap.MaxZoom = 18;
            // Минимальное приближение
            gmap.MinZoom = 2;
            // Курсор мыши в центр карты
            gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;

            // Отключение нигативного режима
            gmap.NegativeMode = false;
            // Разрешение полигонов
            gmap.PolygonsEnabled = true;
            // Разрешение маршрутов
            gmap.RoutesEnabled = true;
            // Скрытие внешней сетки карты
            gmap.ShowTileGridLines = false;
            // При загрузке 10-кратное увеличение
            gmap.Zoom = 15;
            gmap.ShowCenter = false;
            // Изменение размеров
            // gmap.Dock = DockStyle.Fill;

            // Чья карта используется

            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            gmap.Position = new GMap.NET.PointLatLng(49.948022, 35.931451);
            gmap.SetPositionByKeywords("WWWJ+XC Люботин, Харьковская область");
            GMarkerGoogle gMarker = new GMarkerGoogle(gmap.Position, GMarkerGoogleType.arrow);
            CarOverlay.Markers.Add(gMarker);

            /*StreamReader bikereader = new StreamReader("C:/Users/Andrey/source/repos/Novaya_Pochta_.NET/Novaya_Pochta_.NET/BikeDemo.txt");
            while(true)
            {
                List<Tuple<PointLatLng, string, string>> temp = new List<Tuple<PointLatLng, string, string>>();

                while(true)
                {
                    string number = bikereader.ReadLine();
                    string adress = bikereader.ReadLine();
                    string time = bikereader.ReadLine();
                    temp.Add(new Tuple<PointLatLng, string, string>(LandPoint.GetCoordinates(adress), number, time));
                    if(adress == "Warehouse")
                    {
                        break;
                    }
                }
                Bike.Add(temp);
                if (bikereader.EndOfStream)
                {
                    break;
                }
                
            }
            bikereader.Close();
            /*StreamReader carreader = new StreamReader("C:/Users/Andrey/source/repos/Novaya_Pochta_.NET/Novaya_Pochta_.NET/DemoCar.txt");
            while(true)
            {
                List<Tuple<PointLatLng, string, string>> temp = new List<Tuple<PointLatLng, string, string>>();

                while (true)
                {
                    string number = carreader.ReadLine();
                    string adress = carreader.ReadLine();
                    string time = carreader.ReadLine();
                    temp.Add(new Tuple<PointLatLng, string, string>(LandPoint.GetCoordinates(adress), number, time));
                    if (adress == "Warehouse")
                    {
                        break;
                    }
                }
                Car.Add(temp);
                if (carreader.EndOfStream)
                {
                    break;
                }
                
            }
            carreader.Close();
            
            List<PointLatLng> testroute = new List<PointLatLng>();
            for(int i = 0; i < Car[0].Count; i++)
            {
                testroute.Add(Car[0][i].Item1);
            }
            ///////////////////////////////////////////////////trying to make an url request
            string origin = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            foreach (PointLatLng point in testroute)
            {

                string lat = point.Lat.ToString();
                string lng = point.Lng.ToString();
                origin += lat + ',' + lng;
                if(point != testroute.Last())
                {
                    origin += '|';
                }
            }
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RUS");
            //Console.WriteLine(origin);
            origin = "Slobozhanska 35, Lyubotin, Ukraine+ON|Slobozhanska 51, Lyubotin, Ukraine+ON|Depovska 117, Lyubotin, Ukraine+ON|Likarnyana 21, Lyubotin, Ukraine+ON";
            string url = @"https://maps.googleapis.com/maps/api/distancematrix/xml?units=metric&origins=" + origin + " &destinations=" + origin + "&key=AIzaSyCXpTullgkzPeHlXt3pye1M0NX749xW3Q0";//нужно попробовать координаты точных адресов, а не взятых наобум
            //string url = @"https://maps.googleapis.com/maps/api/distancematrix/xml?units=metric&origins=40.6655101,-73.89188969999998&destinations=40.6905615%2C-73.9976592%7C40.6905615%2C-73.9976592%7C40.6905615%2C-73.9976592%7C40.6905615%2C-73.9976592%7C40.6905615%2C-73.9976592%7C40.6905615%2C-73.9976592%7C40.659569%2C-73.933783%7C40.729029%2C-73.851524%7C40.6860072%2C-73.6334271%7C40.598566%2C-73.7527626%7C40.659569%2C-73.933783%7C40.729029%2C-73.851524%7C40.6860072%2C-73.6334271%7C40.598566%2C-73.7527626&key=AIzaSyCXpTullgkzPeHlXt3pye1M0NX749xW3Q0";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            //StreamReader sreader = new StreamReader(dataStream);
            //string responsereader = sreader.ReadToEnd();
            //response.Close();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(dataStream);
            xmldoc.Save("TestResponce");
            ///////////////////////////////////////////////////
            /*var test = GoogleMapProvider.Instance.GetRoadsRoute(testroute, false);
            
            
            PointLatLng start = new PointLatLng(49.946328, 35.927963);
            PointLatLng end = new PointLatLng(49.958404, 35.910537);
            
            //routeOverlay.Routes.Add(r);
            foreach(PointLatLng dot in test.Points)
            {
                Console.WriteLine(dot);
            }
            GMapRoute Denis = new GMapRoute(test.Points, "first");*/

            //markersOverlay.Routes.Add(Denis);
            GMapOverlay markersOverlay = new GMapOverlay("markers");
            gmap.Overlays.Add(CarOverlay);
            gmap.Overlays.Add(BikeOverlay);
            gmap.Overlays.Add(markersOverlay);
        }

        private void NextRoute_Click(object sender, EventArgs e)
        {
            
            if(bikecounter == Bike.Count)
            {
                BikeOverlay.Clear();
                bikecounter = 0;
                return;
            }
            BikeOverlay.Clear();
            /*var temproute = GoogleMapProvider.Instance.GetRoute(LandPoint.GetCoordinates("Warehouse"), Bike[bikecounter][0].Item1, false, true, 10);
            GMapRoute mapRoute = new GMapRoute(temproute.Points, bikecounter.ToString())
            {
                Stroke = new Pen(Color.Red, 3)
            };
            routeOverlay.Routes.Add(mapRoute);
            GMarkerGoogle tempmarker = new GMarkerGoogle(LandPoint.GetCoordinates("Warehouse"), GMarkerGoogleType.red);
            routeOverlay.Markers.Add(tempmarker);
            tempmarker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(tempmarker);
            tempmarker.ToolTipText = Bike[bikecounter][Bike[bikecounter].Count - 1].Item3;
            //System.Threading.Thread.Sleep(100);
            */
            for (int i = 1; i < Bike[bikecounter].Count; i++)
            {
                
                var route = GoogleMapProvider.Instance.GetRoute(Bike[bikecounter][i - 1].coordinates, Bike[bikecounter][i].coordinates, false, true, 10);
                GMapRoute gMapRoute = new GMapRoute(route.Points, i.ToString())
                {
                    Stroke = new Pen(Color.Red, 3)
                };
                GMarkerGoogle temp = new GMarkerGoogle(Bike[bikecounter][i-1].coordinates, GMarkerGoogleType.red);
                temp.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(temp);
                temp.ToolTipText = Bike[bikecounter][i-1].adress;
                GMarkerGoogle temp1 = new GMarkerGoogle(Bike[bikecounter][i].coordinates, GMarkerGoogleType.green);
                temp1.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(temp1);
                temp1.ToolTipText = Bike[bikecounter][i].adress;
                BikeOverlay.Routes.Add(gMapRoute);
                BikeOverlay.Markers.Add(temp);
                BikeOverlay.Markers.Add(temp1);
                //System.Threading.Thread.Sleep(100);
            }
            bikecounter++;
        }
        private void BuildRoute(GMapRoute route, GMarkerGoogle marker)
        {
            //GMapOverlay templay = new GMapOverlay();
            CarOverlay.Routes.Add(route);
            CarOverlay.Markers.Add(marker);
            
            //gmap.Overlays.Add(templay);
            //System.Threading.Thread.Sleep(500);
        }
        private void NextCarRoute_Click(object sender, EventArgs e)
        {
            if (carcounter == Car.Count)
            {
                CarOverlay.Clear();
                carcounter = 0;
                return;
            }
            CarOverlay.Clear();
            Bike.Clear();
            for(int i = 0; i < Car[carcounter].Count - 1; i++)
            {
                List<LandPoint> temp = new List<LandPoint>();
                temp.Add(Car[carcounter][i]);
                temp.Add(Car[carcounter][i+1]);
                Bike.Add(temp);
            }
            /*var temproute = GoogleMapProvider.Instance.GetRoute(LandPoint.GetCoordinates("Warehouse"), Car[carcounter][0].coordinates, false, false, 10);
            GMapRoute mapRoute = new GMapRoute(temproute.Points, carcounter.ToString())
            {
                Stroke = new Pen(Color.Blue, 3)
            };
            routeOverlay.Routes.Add(mapRoute);
            GMarkerGoogle tempmarker = new GMarkerGoogle(LandPoint.GetCoordinates("Warehouse"), GMarkerGoogleType.blue);
            tempmarker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(tempmarker);
            tempmarker.ToolTipText = Car[carcounter][Car[carcounter].Count-1].adress;
            //routeOverlay.Markers.Add(tempmarker);
            BuildRoute(mapRoute, tempmarker);*/
           
            for (int i = 1; i < Car[carcounter].Count; i++)
            {
                var route = GoogleMapProvider.Instance.GetRoute(Car[carcounter][i - 1].coordinates, Car[carcounter][i].coordinates, false, true, 10);
                GMapRoute gMapRoute = new GMapRoute(route.Points, i.ToString())
                {
                    Stroke = new Pen(Color.Blue, 3)
                };
                GMarkerGoogle temp = new GMarkerGoogle(Car[carcounter][i - 1].coordinates, GMarkerGoogleType.blue);
                temp.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(temp);
                temp.ToolTipText = Car[carcounter][i-1].adress;
                BuildRoute(gMapRoute, temp);
                
                
            }
            carcounter++;
        }

        private void NextCarRoute_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
