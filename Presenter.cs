using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

using System;
using System.Threading;
using System.Windows.Forms;

namespace GMapTask
{
    internal class Presenter
    {
        private readonly IGMapView _view;

        private readonly IMarkers _markers;

        private static System.Threading.Timer _timer { get; set; }
        private static Random _random { get; set; }
        private readonly Array _gMarkerGoogleTypesArray;
        private bool _executeScriptAction { get; set; }

        public Presenter(IGMapView view, IMarkers markers)
        {
            _view = view;
            _markers = markers;

            _view.MarkerEnter += View_MarkerEnter;
            _view.MarkerLeave += View_MarkerLeave;
            _view.GMapControl_MouseMove += View_GMapControl_MouseMove;
            _view.GMapControl_Load += View_GMapControl_Load;
            _view.MainWindow_Closed += View_MainWindow_Closed;
            _view.ScriptActions_RadioButtonCheckedChanged += View_ScriptActions_RadioButtonCheckedChanged;
            _view.SetInitialParameters();

            _timer = new System.Threading.Timer(new TimerCallback(UpdateAutoMarker), null, 2000, 3000);
            _random = new Random();
            _gMarkerGoogleTypesArray = Enum.GetValues(typeof(GMarkerGoogleType));

            _executeScriptAction = true;
        }

        private void View_ScriptActions_RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            _executeScriptAction = true;
        }

        async private void UpdateAutoMarker(object obj)
        {
            await _markers.GetPositionFromGpsAsync();
        }

        private void View_GMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _markers.CurrentMarker != null)
            {
                PointLatLng point = _view.GetFromLocalToLatLng(e.X, e.Y);
                _markers.CurrentMarker.Position = point;
                _markers.CurrentMarker.ToolTipText = string.Format("{0},{1}", point.Lat, point.Lng);

                if (_markers.Polygon.IsInside(_markers.CurrentMarker.Position) && _executeScriptAction)
                {
                    if (_view.IsDialogBox)
                    {
                        _view.ShowDialogBox();
                    }
                    else if (_view.IsMarkerColor)
                    {
                        int typeIndex = _random.Next(1, _gMarkerGoogleTypesArray.Length);

                        _view.MarkersOverlay.Markers.Remove(_markers.CurrentMarker);
                        _markers.CurrentMarker = new GMarkerGoogle(
                            new PointLatLng(_markers.CurrentMarker.Position.Lat, _markers.CurrentMarker.Position.Lng), (GMarkerGoogleType)_gMarkerGoogleTypesArray.GetValue(typeIndex));
                        _view.MarkersOverlay.Markers.Add(_markers.CurrentMarker);
                    }
                    else if (_view.IsNewMarker)
                    {
                        RectLatLng rectLatLng = _view.GetViewArea();
                        double lat = rectLatLng.Bottom + _random.NextDouble() * rectLatLng.HeightLat;
                        double lng = rectLatLng.Left + _random.NextDouble() * rectLatLng.WidthLng;

                        GMapMarker gMapMarker = new GMarkerGoogle(new PointLatLng(lat, lng), GMarkerGoogleType.purple);
                        _markers.AddNewMarker(gMapMarker);
                        _view.MarkersOverlay.Markers.Add(gMapMarker);
                    }

                    _executeScriptAction = false;
                }
            }
        }

        async private void View_GMapControl_Load(object sender, EventArgs e)
        {
            await _markers.LoadMarkers();

            _view.MarkersOverlay = new GMapOverlay();
            foreach (int id in _markers.IdMarkerPairs.Keys)
            {
                _view.MarkersOverlay.Markers.Add(_markers.IdMarkerPairs[id]);
            }

            GMapOverlay overlayWithAutoMarker = new GMapOverlay();
            overlayWithAutoMarker.Markers.Add(_markers.AutoMarker);

            GMapOverlay overlayWithPolygon = new GMapOverlay();
            overlayWithPolygon.Polygons.Add(_markers.Polygon);

            _view.AddOverlay(_view.MarkersOverlay);
            _view.AddOverlay(overlayWithAutoMarker);
            _view.AddOverlay(overlayWithPolygon);
        }
        async private void View_MainWindow_Closed(object sender, EventArgs e)
        {
            await _markers.SaveMarkers();
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