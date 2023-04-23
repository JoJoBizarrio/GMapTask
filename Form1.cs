using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GMapTask
{
    public partial class Form1 : Form
    {
        private GMapMarker _currentMarker;
        private SqlConnection _mySqlConnection;
        private Dictionary<int, GMapMarker> _keyValuePairs = new Dictionary<int, GMapMarker>();

        public Form1()
        {
            InitializeComponent();
            GMaps.Instance.Mode = AccessMode.ServerAndCache; //выбор подгрузки карты – онлайн или из ресурсов
            MyGMapControl.MapProvider = GoogleMapProvider.Instance; //какой провайдер карт используется (в нашем случае гугл) 
            MyGMapControl.MinZoom = 2; //минимальный зум
            MyGMapControl.MaxZoom = 16; //максимальный зум
            MyGMapControl.Zoom = 4; // какой используется зум при открытии
            MyGMapControl.Position = new PointLatLng(66.4169575018027, 94.25025752215694);// точка в центре карты при открытии (центр России)
            MyGMapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter; // как приближает (просто в центр карты или по положению мыши)
            MyGMapControl.CanDragMap = true; // перетаскивание карты мышью
            MyGMapControl.DragButton = MouseButtons.Left; // какой кнопкой осуществляется перетаскивание
            MyGMapControl.ShowCenter = false; //показывать или скрывать красный крестик в центре
            MyGMapControl.ShowTileGridLines = true; //показывать или скрывать тайлы

            MyGMapControl.MouseMove += MyGMapControl_MouseMove;
            MyGMapControl.OnMarkerEnter += MyGMapControl_OnMarkerEnter;
            MyGMapControl.OnMarkerLeave += MyGMapControl_OnMarkerLeave;
            FormClosed += Form1_FormClosed;

            SetOverlayWithMarkersByTSQLAsync();

            //GMapOverlay gMapOverlayWithMarkersByTSQL = new GMapOverlay();

            //foreach (int id in _keyValuePairs.Keys)
            //{
            //    gMapOverlayWithMarkersByTSQL.Markers.Add(_keyValuePairs[id]);
            //}

            //MyGMapControl.Overlays.Add(gMapOverlayWithMarkersByTSQL);

            MyGMapControl.Update();
        }

        // События
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateMarkersPositionsInTSQL();
        }

        private void MyGMapControl_OnMarkerEnter(GMapMarker item)
        {
            if (_currentMarker == null)
            {
                _currentMarker = item;
            }
        }

        private void MyGMapControl_OnMarkerLeave(GMapMarker item)
        {
            if (_currentMarker == item)
            {
                _currentMarker = null;
            }
        }

        private void MyGMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _currentMarker != null)
            {
                PointLatLng point = MyGMapControl.FromLocalToLatLng(e.X, e.Y);
                _currentMarker.Position = point;
                // Вывод координат маркера в подсказке
                _currentMarker.ToolTipText = string.Format("{0},{1}", point.Lat, point.Lng);
            }
        }

        // кусок работает с TSQL
        //private void GetMarkersByTSQL()
        //{
        //    using (_mySqlConnection = new SqlConnection("Data Source=DESKTOP-61HUL4I;Initial Catalog=VehiclesPositions;Integrated Security=True"))
        //    {
        //        _mySqlConnection.Open();
        //        SqlCommand sqlCommand = new SqlCommand("SELECT * FROM VehiclesPositions", _mySqlConnection);
        //        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

        //        if (sqlDataReader.HasRows)
        //        {
        //            for (int i = 0; sqlDataReader.Read(); i++)
        //            {
        //                _keyValuePairs.Add
        //                    (Convert.ToInt32(sqlDataReader[0]),
        //                    new GMarkerGoogle(
        //                        new PointLatLng(Convert.ToDouble(sqlDataReader[1]), Convert.ToDouble(sqlDataReader[2])), GMarkerGoogleType.purple_dot));
        //            }
        //        }
        //    }
        //}

        //private void UpdateMarkersPositionsInTSQLAsync()
        //{
        //    using (_mySqlConnection = new SqlConnection("Data Source=DESKTOP-61HUL4I;Initial Catalog=VehiclesPositions;Integrated Security=True"))
        //    {
        //        _mySqlConnection.Open();

        //        foreach (int id in _keyValuePairs.Keys)
        //        {
        //            GMapMarker gMapMarker = _keyValuePairs[id];
        //            string latString = gMapMarker.Position.Lat.ToString().Replace(',', '.');
        //            string lngString = gMapMarker.Position.Lng.ToString().Replace(',', '.');
        //            SqlCommand sqlCommand = new SqlCommand($"UPDATE VehiclesPositions SET Latitude={latString}, Longitude={lngString} WHERE Id={id}", _mySqlConnection);
        //            sqlCommand.ExecuteNonQuery();
        //        }
        //    }
        //}

        // асинхронно
        async private void SetOverlayWithMarkersByTSQLAsync()
        {
            using (_mySqlConnection = new SqlConnection("Data Source=DESKTOP-61HUL4I;Initial Catalog=VehiclesPositions;Integrated Security=True"))
            {
                await _mySqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM VehiclesPositions", _mySqlConnection);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                if (sqlDataReader.HasRows)
                {
                    for (int i = 0; await sqlDataReader.ReadAsync(); i++)
                    {
                        _keyValuePairs.Add
                            (Convert.ToInt32(sqlDataReader[0]),
                            new GMarkerGoogle(
                                new PointLatLng(Convert.ToDouble(sqlDataReader[1]), Convert.ToDouble(sqlDataReader[2])), GMarkerGoogleType.purple_dot));
                    }
                }
            }

            GMapOverlay gMapOverlayWithMarkersByTSQL = new GMapOverlay();

            foreach (int id in _keyValuePairs.Keys)
            {
                gMapOverlayWithMarkersByTSQL.Markers.Add(_keyValuePairs[id]);
            }

            MyGMapControl.Overlays.Add(gMapOverlayWithMarkersByTSQL);
        }

        async private void UpdateMarkersPositionsInTSQL()
        {
            using (_mySqlConnection = new SqlConnection("Data Source=DESKTOP-61HUL4I;Initial Catalog=VehiclesPositions;Integrated Security=True"))
            {
                await _mySqlConnection.OpenAsync();

                foreach (int id in _keyValuePairs.Keys)
                {
                    GMapMarker gMapMarker = _keyValuePairs[id];
                    string latString = gMapMarker.Position.Lat.ToString().Replace(',', '.');
                    string lngString = gMapMarker.Position.Lng.ToString().Replace(',', '.');
                    SqlCommand sqlCommand = new SqlCommand($"UPDATE VehiclesPositions SET Latitude={latString}, Longitude={lngString} WHERE Id={id}", _mySqlConnection);
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
        }
    }
}