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
    internal class Markers : IMarkers
    {
        public Dictionary<int, GMapMarker> IdMarkerPairs { get; }

        public GMapMarker CurrentMarker { get; set; }

        public GMarkerGoogle AutoMarker { get; }

        public Markers()
        {
            IdMarkerPairs = new Dictionary<int, GMapMarker>();
            AutoMarker = new GMarkerGoogle(new PointLatLng(0, 0), GMarkerGoogleType.arrow);
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
                        GMapMarker gMapMarker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sqlDataReader[1]), Convert.ToDouble(sqlDataReader[2])), GMarkerGoogleType.purple_dot);
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

        async public Task GetPositionFromGPSAsync()
        {
            string lastLine;

            using (StreamReader streamReader = new StreamReader($"{Environment.CurrentDirectory}\\GPS\\output.nmea"))
            {
                string allLines = await streamReader.ReadToEndAsync();
                lastLine = allLines.Substring(allLines.LastIndexOf('\n') + 2);
            }

            AutoMarker.Position = GetPointLatLngFromMessageGPGGA(lastLine);
        }

        private PointLatLng GetPointLatLngFromMessageGPGGA(string messageGPGGA)
        {
            string temp = messageGPGGA;
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
    }
}