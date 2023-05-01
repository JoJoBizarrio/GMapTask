using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Windows.Forms;

namespace GMapTask
{
    public partial class Form1 : Form, IGMapView
    {
        public Form1()
        {
            InitializeComponent();

            GMaps.Instance.Mode = AccessMode.ServerAndCache; //выбор подгрузки карты – онлайн или из ресурсов
            MyGMapControl.MapProvider = GoogleMapProvider.Instance; //какой провайдер карт используется (в нашем случае гугл) 
            MyGMapControl.MinZoom = 2; //минимальный зум
            MyGMapControl.MaxZoom = 16; //максимальный зум
            MyGMapControl.Zoom = 2; // какой используется зум при открытии
            MyGMapControl.Position = new PointLatLng(66.4169575018027, 94.25025752215694);// точка в центре карты при открытии (центр России)
            MyGMapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter; // как приближает (просто в центр карты или по положению мыши)
            MyGMapControl.CanDragMap = true; // перетаскивание карты мышью
            MyGMapControl.DragButton = MouseButtons.Left; // какой кнопкой осуществляется перетаскивание
            MyGMapControl.ShowCenter = false; //показывать или скрывать красный крестик в центре
            MyGMapControl.ShowTileGridLines = true; //показывать или скрывать тайлы
        }

        // События
        public event MarkerEnter MarkerEnter;
        private void MyGMapControl_OnMarkerEnter(GMapMarker item)
        {
            if (MarkerEnter != null)
            {
                MarkerEnter(item);
            }
        }

        public event MarkerLeave MarkerLeave;
        private void MyGMapControl_OnMarkerLeave(GMapMarker item)
        {
            if (MarkerLeave != null)
            {
                MarkerLeave(item);
            }
        }

        public event MouseEventHandler GMapControl_MouseMove;
        private void MyGMapMarker_MouseMove(object sender, MouseEventArgs e)
        {
            if (GMapControl_MouseMove != null)
            {
                GMapControl_MouseMove(this, e);
            }
        }

        public event EventHandler<EventArgs> GMapControl_Load;
        private void MyGMapControl_Load(object sender, EventArgs e)
        {
            if (GMapControl_Load != null)
            {
                GMapControl_Load(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> MainWindow_Closed;
        private void Form1_Closed(object sender, FormClosedEventArgs e)
        {
            if (MainWindow_Closed != null)
            {
                MainWindow_Closed(this, EventArgs.Empty);
            }
        }

        // Методы
        public PointLatLng FromLocalToLatLng(int X, int Y)
        {
            return MyGMapControl.FromLocalToLatLng(X, Y);
        }

        public void SetOverlayWithMarkers(GMapOverlay gMapOverlay)
        {
            MyGMapControl.Overlays.Add(gMapOverlay);

            MyGMapControl.ReloadMap();
            MyGMapControl.Refresh();

            MyGMapControl.Zoom *= 2;
            MyGMapControl.Zoom /= 2;
        }
    }
}