using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GMapTask
{
    internal class Model : IModel
    {
        public GMapOverlay MarkersOverlay { get; }
        public GMapOverlay PolygonsOverlay { get; }
        public GMapOverlay AutoMarkerOverlay { get; }

        private Dictionary<int, GMapMarker> IdMarkerPairs { get; }
        private GMapMarker AutoMarker { get; }

        public GMapPolygon Polygon { get; }
        public GMapMarker CurrentMarker { get; set; }

        private static Random Random { get; set; }
        private readonly Array _gMarkerGoogleTypesArray;

        public Model()
        {
            IdMarkerPairs = new Dictionary<int, GMapMarker>();

            AutoMarker = new GMarkerGoogle(new PointLatLng(0, 0), GMarkerGoogleType.arrow);
            AutoMarkerOverlay = new GMapOverlay();
            AutoMarkerOverlay.Markers.Add(AutoMarker);

            List<PointLatLng> pointLatLngs = new List<PointLatLng>()
            {
                new PointLatLng(30, 30),
                new PointLatLng(30, 60),
                new PointLatLng(60, 60),
                new PointLatLng(60, 30),
            };

            PolygonsOverlay = new GMapOverlay();
            Polygon = new GMapPolygon(pointLatLngs, "Square");
            PolygonsOverlay.Polygons.Add(Polygon);

            MarkersOverlay = new GMapOverlay();
            
            Random = new Random();
            _gMarkerGoogleTypesArray = Enum.GetValues(typeof(GMarkerGoogleType));
        }

        async public Task LoadMarkers()
        {
            using (SqlConnection MySqlConnection = new SqlConnection("Data Source=DESKTOP-61HUL4I;Initial Catalog=VehiclesPositions;Integrated Security=True"))
            {
                await MySqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM VehiclesPositions", MySqlConnection);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                if (sqlDataReader.HasRows)
                {
                    for (int i = 0; await sqlDataReader.ReadAsync(); i++)
                    {
                        GMapMarker gMapMarker = new GMarkerGoogle(
                            new PointLatLng(Convert.ToDouble(sqlDataReader[1]), Convert.ToDouble(sqlDataReader[2])), GMarkerGoogleType.purple_dot);
                        MarkersOverlay.Markers.Add(gMapMarker);
                        IdMarkerPairs.Add(Convert.ToInt32(sqlDataReader[0]), gMapMarker);
                    }
                }
            }
        }

        async public Task SaveMarkers()
        {
            using (SqlConnection MySqlConnection = new SqlConnection("Data Source=DESKTOP-61HUL4I;Initial Catalog=VehiclesPositions;Integrated Security=True"))
            {
                await MySqlConnection.OpenAsync();
                StringBuilder cmdTextStringBuilder = new StringBuilder();

                foreach (int id in IdMarkerPairs.Keys)
                {
                    GMapMarker gMapMarker = IdMarkerPairs[id];
                    string latString = gMapMarker.Position.Lat.ToString().Replace(',', '.');
                    string lngString = gMapMarker.Position.Lng.ToString().Replace(',', '.');

                    cmdTextStringBuilder.AppendLine($"UPDATE VehiclesPositions SET Latitude={latString}, Longitude={lngString} WHERE Id={id};");
                }

                SqlCommand sqlCommand = new SqlCommand(cmdTextStringBuilder.ToString(), MySqlConnection);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        async public Task UpdateAutomarkerPositionFromGpsAsync()
        {
            string lastLine;

            using (StreamReader streamReader = new StreamReader($"{Environment.CurrentDirectory}\\GPS\\output.nmea"))
            {
                string allLines = await streamReader.ReadToEndAsync();
                lastLine = allLines.Substring(allLines.LastIndexOf('\n') + 2);
            }

            AutoMarker.Position = GetPointLatLngFromGpggaMessage(lastLine);
        }

        private PointLatLng GetPointLatLngFromGpggaMessage(string GpggaMessage)
        {
            string temp = GpggaMessage;
            string[] array = new string[6];
            int index;

            for (int i = 0; i < array.Length; i++)
            {
                index = temp.IndexOf(',');
                array[i] = temp.Remove(index);
                temp = temp.Substring(index + 2);
            }

            string latString = array[2];
            string lngString = array[4];
            int latSign = 1;
            int lngSign = 1;

            if (array[3] != "N")
            {
                latSign = -1;
            }

            if (array[5] != "E")
            {
                lngSign = -1;
            }

            double lat = latSign * (Convert.ToDouble(latString.Remove(2)) + Convert.ToDouble(latString.Substring(2, 2)) / 60);
            double lng = lngSign * (Convert.ToDouble(lngString.Remove(3)) + Convert.ToDouble(lngString.Substring(3, 2)) / 60);

            return new PointLatLng(lat, lng);
        }

        async public Task AddNewMarkerInsideViewArea(RectLatLng viewArea)
        {
            double lat = viewArea.Bottom + Random.NextDouble() * viewArea.HeightLat;
            double lng = viewArea.Left + Random.NextDouble() * viewArea.WidthLng;

            GMapMarker gMapMarker = new GMarkerGoogle(new PointLatLng(lat, lng), GMarkerGoogleType.purple);
            MarkersOverlay.Markers.Add(gMapMarker);

            using (SqlConnection MySqlConnection = new SqlConnection("Data Source=DESKTOP-61HUL4I;Initial Catalog=VehiclesPositions;Integrated Security=True"))
            {
                await MySqlConnection.OpenAsync();

                string latString = lat.ToString().Replace(',', '.');
                string lngString = lng.ToString().Replace(',', '.');
                string commandInsert = $"INSERT INTO VehiclesPositions (Latitude, Longitude) VALUES ({latString}, {lngString});";

                SqlCommand sqlCommand = new SqlCommand(commandInsert, MySqlConnection);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public void ChangeMarkerColor()
        {
            int typeIndex = Random.Next(1, _gMarkerGoogleTypesArray.Length);

            MarkersOverlay.Markers.Remove(CurrentMarker);

            CurrentMarker = new GMarkerGoogle(
                new PointLatLng(CurrentMarker.Position.Lat, CurrentMarker.Position.Lng), 
                (GMarkerGoogleType)_gMarkerGoogleTypesArray.GetValue(typeIndex));

            MarkersOverlay.Markers.Add(CurrentMarker);
        }

        public void UpdateCurrentMarker(PointLatLng point)
        {
            CurrentMarker.Position = point;
            CurrentMarker.ToolTipText = string.Format("{0},{1}", point.Lat, point.Lng);
        }
    }
}