using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsResponse
{
    public class RankingResponse
    {
        public string title { get; set; } = String.Empty;
        public List<DataRanking> ranking { get; set; } = new List<DataRanking>();
    }

    public class DataRanking
    {
        public string descripcion { get; set; } = String.Empty;
        public string noVentas { get; set; } = String.Empty;
    }
}
