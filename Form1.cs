using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET;

using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace GMapTask
{
    public partial class Form1 : Form
    {
        private GMapMarker _currentMarker; //текущий перетаскиваемый маркер

        private Dictionary<int, GMapMarker> _idGMapMarkerPairs = new Dictionary<int, GMapMarker>(); // Коллекция сопоставляющая id-Marker

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

            MyGMapControl.MouseMove += MyGMapControl_MouseMove;
            MyGMapControl.OnMarkerEnter += MyGMapControl_OnMarkerEnter;
            MyGMapControl.OnMarkerLeave += MyGMapControl_OnMarkerLeave;
            FormClosed += Form1_FormClosed;

            _ = SetOverlayWithMarkersAsync();
        }

        // События
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ = DataShuttle.UpdateMarkersPositionsInServerAsync(_idGMapMarkerPairs);
        }

        private void MyGMapControl_OnMarkerEnter(GMapMarker item)
        {
            if (_currentMarker == null)
            {
                _currentMarker = item;
            }
        }

        private void MyGMapControl_OnMarkerLeave(GMapMarker item)
        {
            if (_currentMarker == item)
            {
                _currentMarker = null;
            }
        }

        private void MyGMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _currentMarker != null)
            {
                PointLatLng point = MyGMapControl.FromLocalToLatLng(e.X, e.Y);
                _currentMarker.Position = point;
                // Вывод координат маркера в подсказке
                _currentMarker.ToolTipText = string.Format("{0},{1}", point.Lat, point.Lng);
            }
        }

        // асинхронно
        async private Task SetOverlayWithMarkersAsync()
        {
            GMapOverlay gMapOverlayWithMarkersByTSQL = new GMapOverlay();
            _idGMapMarkerPairs = await DataShuttle.GetMarkersFromServerAsync();

            foreach (int id in _idGMapMarkerPairs.Keys)
            {
                gMapOverlayWithMarkersByTSQL.Markers.Add(_idGMapMarkerPairs[id]);
            }

            MyGMapControl.Overlays.Add(gMapOverlayWithMarkersByTSQL);
        }
    }
}