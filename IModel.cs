using GMap.NET;
using GMap.NET.WindowsForms;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GMapTask
{
    public interface IModel
    {
        GMapOverlay MarkersOverlay { get; }
        GMapOverlay PolygonsOverlay { get; }
        GMapOverlay AutoMarkerOverlay { get; }
        GMapPolygon Polygon { get; }
        GMapMarker CurrentMarker { get; set; }

        void UpdateCurrentMarker(PointLatLng point);
        Task LoadMarkers();
        Task SaveMarkers();
        Task GetPositionFromGpsAsync();
        void ChangeMarkerColor();
        Task AddNewMarkerInsideViewArea(RectLatLng viewArea);
    }
}