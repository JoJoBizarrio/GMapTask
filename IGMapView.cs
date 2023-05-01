using GMap.NET;
using GMap.NET.WindowsForms;

using System;
using System.Windows.Forms;

namespace GMapTask
{
    public interface IGMapView
    {
        GMapControl GMapControl { get; }

        event EventHandler<EventArgs> GMapControl_Load;

        event EventHandler<EventArgs> MainWindow_Closed;

        event MarkerEnter MarkerEnter;

        event MarkerLeave MarkerLeave;

        event MouseEventHandler GMapControl_MouseMove;

        void SetOverlayWithMarkers(GMapOverlay gMapOverlay);
    }
}