using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using GMap.NET;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

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
    }
}
