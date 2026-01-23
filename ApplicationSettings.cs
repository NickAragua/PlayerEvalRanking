using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYSAPlayerRanker
{
    public class ApplicationSettings
    {
        public double CurrentSeasonWeight { get; set; }
        public double PreviousSeasonWeight { get; set; }
        public double EvalWeight { get; set; }

        public List<double> DivisionWeights { get; set; } = new List<double>() { 1.0, 0.95, 0.9, 0.85, 0.8 };
        public bool AutoSave { get; set; } = true;
    }
}
