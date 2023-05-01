using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace GMapTask
{
    internal class Presenter
    {
        private readonly IGMapView _view;

        private readonly IMarkers _markers;

        private System.Threading.Timer _timer { get; set; }

        public Presenter(IGMapView view, IMarkers markers)
        {
            _view = view;
            _markers = markers;

            SetInitialParameters();
            _view.MarkerEnter += View_MarkerEnter;
            _view.MarkerLeave += View_MarkerLeave;
            _view.GMapControl_MouseMove += View_GMapControl_MouseMove;
            _view.GMapControl_Load += View_GMapControl_Load;
            _view.MainWindow_Closed += View_MainWindow_Closed;

            GMapOverlay overlayWithAutoMarker = new GMapOverlay();
            overlayWithAutoMarker.Markers.Add(_markers.AutoMarker);
            _view.GMapControl.Overlays.Add(overlayWithAutoMarker);

            _timer = new System.Threading.Timer(new TimerCallback(UpdateAutoMarker), null, 2000, 3000);
        }

        private void SetInitialParameters()
        {
            _view.GMapControl.MapProvider = GoogleMapProvider.Instance; //какой провайдер карт используется (в нашем случае гугл) 
            _view.GMapControl.MinZoom = 2; //минимальный зум
            _view.GMapControl.MaxZoom = 16; //максимальный зум
            _view.GMapControl.Zoom = 2; // какой используется зум при открытии
            _view.GMapControl.Position = new PointLatLng(66.4169575018027, 94.25025752215694);// точка в центре карты при открытии (центр России)
            _view.GMapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter; // как приближает (просто в центр карты или по положению мыши)
            _view.GMapControl.CanDragMap = true; // перетаскивание карты мышью
            _view.GMapControl.DragButton = MouseButtons.Left; // какой кнопкой осуществляется перетаскивание
            _view.GMapControl.ShowCenter = false; //показывать или скрывать красный крестик в центре
            _view.GMapControl.ShowTileGridLines = true; //показывать или скрывать тайлы
        }

        async private void UpdateAutoMarker(object obj)
        {
            await _markers.GetPositionFromGPSAsync();
        }

        async private void View_MainWindow_Closed(object sender, EventArgs e)
        {
            await _markers.SaveMarkers();
        }

        private void View_GMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _markers.CurrentMarker != null)
            {
                PointLatLng point = _view.GMapControl.FromLocalToLatLng(e.X, e.Y);
                _markers.CurrentMarker.Position = point;
                _markers.CurrentMarker.ToolTipText = string.Format("{0},{1}", point.Lat, point.Lng);
            }
        }

        async private void View_GMapControl_Load(object sender, EventArgs e)
        {
            await _markers.LoadMarkers();
            SetOverlayWithMarkers(_markers.IdMarkerPairs);
        }

        private void SetOverlayWithMarkers(Dictionary<int, GMapMarker> idMarkerPairs)
        {
            GMapOverlay gMapOverlay = new GMapOverlay();

            foreach (int id in idMarkerPairs.Keys)
            {
                gMapOverlay.Markers.Add(idMarkerPairs[id]);
            }

            _view.SetOverlayWithMarkers(gMapOverlay);
        }

        private void View_MarkerEnter(GMapMarker item)
        {
            if (_markers.CurrentMarker == null)
            {
                _markers.CurrentMarker = item;
            }
        }

        private void View_MarkerLeave(GMapMarker item)
        {
            if (_markers.CurrentMarker == item)
            {
                _markers.CurrentMarker = null;
            }
        }
    }
}