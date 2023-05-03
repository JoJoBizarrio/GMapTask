using GMap.NET.WindowsForms;

using System;
using System.Threading;
using System.Windows.Forms;

namespace GMapTask
{
    internal class Presenter
    {
        private readonly IGMapView _view;

        private readonly IModel _model;

        private static System.Threading.Timer _timer { get; set; }
        private bool _executeScriptAction { get; set; }

        public Presenter(IGMapView view, IModel model)
        {
            _view = view;
            _model = model;

            _view.MarkerEnter += View_MarkerEnter;
            _view.MarkerLeave += View_MarkerLeave;
            _view.GMapControl_MouseMove += View_GMapControl_MouseMove;
            _view.GMapControl_Load += View_GMapControl_Load;
            _view.MainWindow_Closed += View_MainWindow_Closed;
            _view.ScriptActions_RadioButtonCheckedChanged += View_ScriptActions_RadioButtonCheckedChanged;
            _view.SetInitialParameters();

            _timer = new System.Threading.Timer(new TimerCallback(UpdateAutoMarker), null, 2000, 3000);

            _executeScriptAction = true;
        }

        private void View_ScriptActions_RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            _executeScriptAction = true;
        }

        async private void UpdateAutoMarker(object obj)
        {
            await _model.UpdateAutomarkerPositionFromGpsAsync();
        }

        async private void View_GMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _model.CurrentMarker != null)
            {
                _model.UpdateCurrentMarker(_view.GetFromLocalToLatLng(e.X, e.Y));

                if (_model.Polygon.IsInside(_model.CurrentMarker.Position) && _executeScriptAction)
                {
                    if (_view.IsDialogBox)
                    {
                        _view.ShowDialogBox();
                    }
                    else if (_view.IsMarkerColor)
                    {
                        _model.ChangeMarkerColor();

                    }
                    else if (_view.IsNewMarker)
                    {
                        await _model.AddNewMarkerInsideViewArea(_view.GetViewArea());
                    }

                    _executeScriptAction = false;
                }
            }
        }

        async private void View_GMapControl_Load(object sender, EventArgs e)
        {
            await _model.LoadMarkers();

            _view.AddOverlay(_model.MarkersOverlay);
            _view.AddOverlay(_model.AutoMarkerOverlay);
            _view.AddOverlay(_model.PolygonsOverlay);
        }
        async private void View_MainWindow_Closed(object sender, EventArgs e)
        {
            await _model.SaveMarkers();
        }

        private void View_MarkerEnter(GMapMarker item)
        {
            if (_model.CurrentMarker == null)
            {
                _model.CurrentMarker = item;
            }
        }

        private void View_MarkerLeave(GMapMarker item)
        {
            if (_model.CurrentMarker == item)
            {
                _model.CurrentMarker = null;
            }
        }
    }
}