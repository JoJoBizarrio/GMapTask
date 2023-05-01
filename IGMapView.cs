using GMap.NET.WindowsForms;
using System;

namespace GMapTask
{
    public interface IGMapView
    {
        event MarkerEnter MarkerEnter;

        event MarkerLeave MarkerLeave;

        void SetOverlayWithMarkers(GMapOverlay gMapOverlay);

        event EventHandler<EventArgs> LoadMarkers_Click;
    }
}