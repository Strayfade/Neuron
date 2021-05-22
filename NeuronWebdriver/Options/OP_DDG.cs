using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuronWebdriver;

namespace NeuronWebdriver.Options
{
    public struct OP_DDG
    {
        public string OP_DDG_FontOptionToParameter(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    return "a";
                    break;
                case 1:
                    return "c";
                    break;
                case 2:
                    return "g";
                    break;
                case 3:
                    return "h";
                    break;
                case 4:
                    return "p";
                    break;
                case 5:
                    return "n";
                    break;
                case 6:
                    return "e";
                    break;
                case 7:
                    return "s";
                    break;
                case 8:
                    return "o";
                    break;
                case 9:
                    return "t";
                    break;
                case 10:
                    return "b";
                    break;
                case 11:
                    return "v";
                    break;
            }
            return "";
        }
        public string OP_DDG_ThemeOptionToParameter(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    return "-1";
                    break;
                case 1:
                    return "c";
                    break;
                case 2:
                    return "r";
                    break;
                case 3:
                    return "d";
                    break;
                case 4:
                    return "t";
                    break;
            }
            return "";
        }
    }
}
