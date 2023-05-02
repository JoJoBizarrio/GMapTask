using GMap.NET;
using GMap.NET.WindowsForms;

using System;
using System.Windows.Forms;

namespace GMapTask
{
    public interface IGMapView
    {
        event EventHandler<EventArgs> GMapControl_Load;

        event EventHandler<EventArgs> MainWindow_Closed;

        event MarkerEnter MarkerEnter;

        event MarkerLeave MarkerLeave;

        event EventHandler<EventArgs> ScriptActions_RadioButtonCheckedChanged;

        event MouseEventHandler GMapControl_MouseMove;

        GMapOverlay MarkersOverlay { get; set; }
        bool IsDialogBox { get; }
        bool IsMarkerColor { get; }
        bool IsNewMarker { get; }

        void SetInitialParameters();
        void AddOverlay(GMapOverlay gMapOverlay);
        PointLatLng GetFromLocalToLatLng(int X, int Y);
        void ShowDialogBox();
        RectLatLng GetViewArea();
    }
}