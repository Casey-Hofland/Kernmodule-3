using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder
{
    public static class RobotExtensions
    {
        public static string RobotName(this string name) => name.Remove(0, name.LastIndexOf('.') + 1);


    }
}
