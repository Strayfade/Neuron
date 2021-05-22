using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronWebdriver
{
    public struct LoadtimeController
    {
        public DateTime StartLoadTime;
        public DateTime EndLoadTime;
        public DateTime Diff()
        {
            TimeSpan T = EndLoadTime.Subtract(StartLoadTime);
            DateTime R = new DateTime(1, 1, 1, T.Hours, T.Minutes, T.Seconds, T.Milliseconds);
            return R;
        }
    }
}
