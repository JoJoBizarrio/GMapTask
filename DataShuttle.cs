using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using GMap.NET;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.CodeDom.Compiler;
using Microsoft.SqlServer.Server;

namespace GMapTask
{
    internal static class DataShuttle
    {
        async static public Task<Dictionary<int, GMapMarker>> GetMarkersFromServerAsync()
        {
            Dictionary<int, GMapMarker> idGMapMarkerPairs = new Dictionary<int, GMapMarker>();

            using (SqlConnection MySqlConnection = new SqlConnection("Data Source=DESKTOP-61HUL4I;Initial Catalog=VehiclesPositions;Integrated Security=True"))
            {
                await MySqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM VehiclesPositions", MySqlConnection);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                if (sqlDataReader.HasRows)
                {
                    for (int i = 0; await sqlDataReader.ReadAsync(); i++)
                    {
                        idGMapMarkerPairs.Add(
                            Convert.ToInt32(sqlDataReader[0]), new GMarkerGoogle(
                                new PointLatLng(Convert.ToDouble(sqlDataReader[1]), Convert.ToDouble(sqlDataReader[2])), GMarkerGoogleType.purple_dot));
                    }
                }
            }

            return idGMapMarkerPairs;
        }

        async static public Task UpdateMarkersPositionsInServerAsync(Dictionary<int, GMapMarker> idGMApMarkerPairs)
        {
            using (SqlConnection MySqlConnection = new SqlConnection("Data Source=DESKTOP-61HUL4I;Initial Catalog=VehiclesPositions;Integrated Security=True"))
            {
                await MySqlConnection.OpenAsync();
                StringBuilder cmdTextStringBuilder = new StringBuilder();

                foreach (int id in idGMApMarkerPairs.Keys)
                {
                    GMapMarker gMapMarker = idGMApMarkerPairs[id];
                    string latString = gMapMarker.Position.Lat.ToString().Replace(',', '.');
                    string lngString = gMapMarker.Position.Lng.ToString().Replace(',', '.');

                    cmdTextStringBuilder.AppendLine($"UPDATE VehiclesPositions SET Latitude={latString}, Longitude={lngString} WHERE Id={id};");
                }

                SqlCommand sqlCommand = new SqlCommand(cmdTextStringBuilder.ToString(), MySqlConnection);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        async static public Task<List<PointLatLng>> GetPositionsFromGPS()
        {
            List<PointLatLng> pointLatLngsList = new List<PointLatLng>();

            using (StreamReader streamReader = new StreamReader($"{Environment.CurrentDirectory}\\GPS\\output.nmea"))
            {
                string currentLine;
                string temp;
                int index;
                string[] array = new string[6];

                while ((currentLine = await streamReader.ReadLineAsync()) != null)
                {
                    temp = currentLine;

                    for (int i = 0; i < array.Length; i++)
                    {
                        index = temp.IndexOf(',');
                        array[i] = temp.Remove(index);
                        temp = temp.Substring(index+2);
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
                    pointLatLngsList.Add(new PointLatLng(lat, lng));
                }
            }

            return pointLatLngsList;
        }
    }
}