using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYSAPlayerRanker
{
    public class ApplicationSettings
    {
        public double CurrentSeasonWeight { get; set; } = .4;
        public double PreviousSeasonWeight { get; set; } = .3;
        public double EvalWeight { get; set; } = .3;

        public List<double> DivisionWeights { get; set; } = new List<double>() { 1.0, 0.94, 0.88, 0.82, 0.76 };
        public bool AutoSave { get; set; } = true;
    }
}
