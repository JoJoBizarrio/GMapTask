using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMapTask
{
    public interface IMarkers
    {
        Dictionary<int, GMapMarker> IdMarkerPairs { get; }

        GMapMarker CurrentMarker { get; set; }

        Task LoadMarkers();
    }
}
