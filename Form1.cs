using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET;

using System;
using System.Windows.Forms;

namespace GMapTask
{
    public partial class Form1 : Form, IGMapView
    {
        public GMapControl GMapControl { get; private set; }

        public Form1()
        {
            InitializeComponent();

            GMapControl = MyGMapControl;
            GMaps.Instance.Mode = AccessMode.ServerAndCache; //выбор подгрузки карты – онлайн или из ресурсов
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