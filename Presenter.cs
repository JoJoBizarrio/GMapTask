using GMap.NET;
using GMap.NET.WindowsForms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMapTask
{
    internal class Presenter
    {
        private readonly IGMapView _view;

        private readonly IMarkers _markers;

        public Presenter(IGMapView view, IMarkers markers)
        {
            _view = view;
            _markers = markers;
            //_ = _markers.LoadMarkers();
            _view.MarkerEnter += View_MarkerEnter;
            _view.MarkerLeave += View_MarkerLeave;
            _view.GMapControl_MouseMove += View_GMapControl_MouseMove;
            _view.GMapControl_Load += View_GMapControl_Load;
            _view.MainWindow_Closed += View_MainWindow_Closed;
        }

        async private void View_MainWindow_Closed(object sender, System.EventArgs e)
        {
            await _markers.SaveMarkers(); 
        }

        private void View_GMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _markers.CurrentMarker != null)
            {
                PointLatLng point = _view.FromLocalToLatLng(e.X, e.Y);
                _markers.CurrentMarker.Position = point;
                _markers.CurrentMarker.ToolTipText = string.Format("{0},{1}", point.Lat, point.Lng);
            }
        }

        async private void View_GMapControl_Load(object sender, System.EventArgs e)
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