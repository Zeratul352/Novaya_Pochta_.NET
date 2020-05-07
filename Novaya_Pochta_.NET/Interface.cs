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
    public partial class Nova_pochta : Form//this form is user interface
    {       
        int stepcounter = 0;//all this 8 fields are required for proper demonstration of program results
        int routecounter = 0;
        int allroutescounter = 0;
        public List<List<LandPoint>> CurrentSteps { get; set; } = new List<List<LandPoint>>();
        public List<DelivererRoute> CurrentRoutes { get; set; } = new List<DelivererRoute>();
        public List<Tuple<List<DelivererRoute>, string>> AllRoutes { get; set; } = new List<Tuple<List<DelivererRoute>, string>>();
        GMapOverlay StepOverlay = new GMapOverlay("StepOverlay");
        GMapOverlay RouteOverlay = new GMapOverlay("RouteOverlay");
        public Nova_pochta()
        {
            InitializeComponent();
            
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
            // При загрузке 15-кратное увеличение
            gmap.Zoom = 15;
            gmap.ShowCenter = false;
            

            // Чья карта используется

            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            gmap.Position = new GMap.NET.PointLatLng(49.948022, 35.931451);
            gmap.SetPositionByKeywords("WWWJ+XC Люботин, Харьковская область");
            GMarkerGoogle gMarker = new GMarkerGoogle(gmap.Position, GMarkerGoogleType.arrow);
            StepOverlay.Markers.Add(gMarker);//shows you current start position

            
            GMapOverlay markersOverlay = new GMapOverlay("markers");
            gmap.Overlays.Add(StepOverlay);
            gmap.Overlays.Add(RouteOverlay);
            gmap.Overlays.Add(markersOverlay);
        }

        private void Next_step_click(object sender, EventArgs e)//maintains by - step route illustration
        {
            
            if(stepcounter == CurrentSteps.Count)
            {
                RouteOverlay.Clear();
                stepcounter = 0;
                return;
            }
            RouteOverlay.Clear();
            
            for (int i = 1; i < CurrentSteps[stepcounter].Count; i++)
            {
                
                var route = GoogleMapProvider.Instance.GetRoute(CurrentSteps[stepcounter][i - 1].coordinates, CurrentSteps[stepcounter][i].coordinates, false, true, 10);
                GMapRoute gMapRoute = new GMapRoute(route.Points, i.ToString())
                {
                    Stroke = new Pen(Color.Green, 3)
                };
                GMarkerGoogle temp = new GMarkerGoogle(CurrentSteps[stepcounter][i-1].coordinates, GMarkerGoogleType.green);
                temp.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(temp);
                
                GMarkerGoogle temp1 = new GMarkerGoogle(CurrentSteps[stepcounter][i].coordinates, GMarkerGoogleType.arrow);
                temp1.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(temp1);
                
                RouteOverlay.Routes.Add(gMapRoute);
                RouteOverlay.Markers.Add(temp);
                RouteOverlay.Markers.Add(temp1);
                
            }
            stepcounter++;
        }
        private void BuildRoute(GMapRoute route, GMarkerGoogle marker)//simply shows a route
        {
            
            StepOverlay.Routes.Add(route);
            StepOverlay.Markers.Add(marker);
                       
        }
        private void Next_route_click(object sender, EventArgs e)//maintain switching between routes
        {
            RouteOverlay.Clear();
            if (routecounter == CurrentRoutes.Count)
            {
                StepOverlay.Clear();
                CurrentSteps.Clear();
                stepcounter = 0;
                routecounter = 0;
                return;
            }
            StepOverlay.Clear();
            CurrentSteps.Clear();
            stepcounter = 0;
            for(int i = 0; i < CurrentRoutes[routecounter].route.Count - 1; i++)
            {
                List<LandPoint> temp = new List<LandPoint>();
                temp.Add(CurrentRoutes[routecounter].route[i]);
                temp.Add(CurrentRoutes[routecounter].route[i+1]);
                CurrentSteps.Add(temp);
            }
                       
            for (int i = 1; i < CurrentRoutes[routecounter].route.Count; i++)
            {
                var route = GoogleMapProvider.Instance.GetRoute(CurrentRoutes[routecounter].route[i - 1].coordinates, CurrentRoutes[routecounter].route[i].coordinates, false, true, 10);
                GMapRoute gMapRoute = new GMapRoute(route.Points, i.ToString())
                {
                    Stroke = new Pen(Color.Blue, 3)
                };
                GMarkerGoogle temp = new GMarkerGoogle(CurrentRoutes[routecounter].route[i - 1].coordinates, GMarkerGoogleType.blue);
                temp.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(temp);
                temp.ToolTipText = CurrentRoutes[routecounter].route[i-1].adress + CurrentRoutes[routecounter].Tooltips[i-1];
                BuildRoute(gMapRoute, temp);
                
                
            }
            routecounter++;
        }
        

        private void NextCurier_Click(object sender, EventArgs e)//maintain switching between routes
        {
            StepOverlay.Clear();
            RouteOverlay.Clear();
            CurrentRoutes.Clear();
            CurrentSteps.Clear();
            stepcounter = 0;
            routecounter = 0;
            if(allroutescounter == AllRoutes.Count)
            {
                allroutescounter = 0;
                curier_text.Text = "Nothing selected";
                return;
            }
            curier_text.Text = AllRoutes[allroutescounter].Item2;
            CurrentRoutes = new List<DelivererRoute>(AllRoutes[allroutescounter].Item1);
            allroutescounter++;
        }

        private void Select_source_Click(object sender, EventArgs e)//maintains initiating of math calculations
        {
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var filePath = openFileDialog1.FileName;
                    Process(filePath);
                }
                catch (System.Security.SecurityException ex)//if you can't open a file because of security
                {
                    StatusLabel.Text = $"Security error. \n\nError message: {ex.Message}\n\n" + $"Details:\n\n{ex.StackTrace}";
                    StatusLabel.ForeColor = Color.Red;
                }
                catch(Exception error)//if some other issues happen
                {
                    StatusLabel.Text = $"An error happened. Error message: {error.Message}";
                    StatusLabel.ForeColor = Color.Red;
                }
            }
        }
        public async void ProcessAsync(string filename)
        {
            await Task.Run(() => Process(filename));
        }
        public void Process(string filename)
        {
            StatusLabel.Text = "";

            progressStrip.ProgressBar.Value = 0;
            
            StatusLabel.ForeColor = Color.Black;
            Deliverer warehouse = new Deliverer(10000, 0, 0, 0);//adding and filling the warehouse
            //warehouse.RandomFill();
            warehouse.FileFill(filename);
            Deliverer bike_courier = new Deliverer(50, 100, 5, 0);//adding couriers
            Deliverer car_courier = new Deliverer(500, 500, 12, 0);
            Deliverer lorry_courier = new Deliverer(1500, 2000, 15, 0);
            bike_courier.TransferWithCap(warehouse, 30, 5);//transfering them all packages they can carry
            car_courier.TransferWithCap(warehouse, 250, 30);
            lorry_courier.TransferWithCap(warehouse, 1000, 300);
            StatusLabel.Text = "Data loaded succesfuly";//showing that initiation is done

            progressStrip.ProgressBar.Value = 10;

            bike_courier.GroupMyBoxes();
            bike_courier.BuildRoutesAsync("Bike");//building routes for bike

            StatusLabel.Text = "Calculations finished for bike curier";
            progressStrip.ProgressBar.Value = 40;

            car_courier.GroupMyBoxes();
            car_courier.BuildRoutesAsync("Car");//building routes for car
            StatusLabel.Text = "Calculations finished for car curier";
            progressStrip.ProgressBar.Value = 70;


            lorry_courier.GroupMyBoxes();
            lorry_courier.BuildRoutesAsync("Lorry");//building routes for lorry
            System.Threading.Thread.Sleep(4000);
            StatusLabel.Text = "Calculations finished for lorry curier";
            progressStrip.ProgressBar.Value = 100;

            AllRoutes.Add(new Tuple<List<DelivererRoute>, string>(bike_courier.Deliverer_routes, "Bike courier"));//showing them
            AllRoutes.Add(new Tuple<List<DelivererRoute>, string>(car_courier.Deliverer_routes, "Car courier"));
            AllRoutes.Add(new Tuple<List<DelivererRoute>, string>(lorry_courier.Deliverer_routes, "Lorry courier"));
            StatusLabel.Text = "All calculations finished successfully";
            warehouse.LoadOutToFile("Warehouse");//all rest in the warehouse is loaded out to file
        }

    }
}
