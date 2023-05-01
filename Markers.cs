using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GMapTask
{
    internal class Markers : IMarkers
    {
        public Dictionary<int, GMapMarker> IdMarkerPairs {  get; set; }

        public GMapMarker CurrentMarker { get; set; }

        public Markers() 
        {
            IdMarkerPairs = new Dictionary<int, GMapMarker>();
            _ = LoadMarkers();
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
    }
}
