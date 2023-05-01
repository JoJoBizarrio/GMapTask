using GMap.NET.WindowsForms;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            _view.LoadMarkers_Click += View_LoadMarkers_Click;
        }

        async private void View_LoadMarkers_Click(object sender, System.EventArgs e)
        {
            await _markers.LoadMarkers();
            SetOverlayWithMarkers(_markers.IdMarkerPairs);
        }

        //async private void View_GMapLoad(object sender, System.EventArgs e)
        //{
        //    await _markers.LoadMarkers();
        //    SetOverlayWithMarkers(_markers.IdMarkerPairs);
        //}

        private void SetOverlayWithMarkers(Dictionary<int, GMapMarker> idMarkerPairs)
        {
            GMapOverlay gMapOverlay = new GMapOverlay();

            foreach (int id in idMarkerPairs.Keys)
            {
                gMapOverlay.Markers.Add(idMarkerPairs[id]);
            }

            _view.SetOverlayWithMarkers(gMapOverlay);
        }

        async private Task LoadMarkers()
        {
            //Task markers = _markers.LoadMarkers();
            //_view.
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