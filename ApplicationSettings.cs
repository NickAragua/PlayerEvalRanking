using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYSAPlayerRanker
{
    public class ApplicationSettings
    {
        public double CurrentSeasonWeight { get; set; } = .7;
        public double PreviousSeasonWeight { get; set; } = .0;
        public double AssessmentWeight { get; set; } = .3;

        public double NoAssessmentPenalty { get; set; } = .3;

        public List<double> DivisionWeights { get; set; } = new List<double>() { 1.0, 0.93, 0.86, 0.79, 0.72 };
        public bool AutoSave { get; set; } = true;
    }
}
