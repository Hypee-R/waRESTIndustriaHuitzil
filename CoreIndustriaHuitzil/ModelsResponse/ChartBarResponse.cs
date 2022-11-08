using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsResponse
{
    public class ChartBarResponse
    {
        public string title { get; set; } = String.Empty;
        public List<Series> series { get; set; }
    }

    public class Series
    {
        public string name { get; set; } = String.Empty;
        public List<int> data { get; set; }
    }
}
