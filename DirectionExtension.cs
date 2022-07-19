using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobot
{
    public static class DirectionExtension
    {
        public static string ToName(this Directions direction)
        {
            return Enum.GetName(direction)?.ToUpper();
        }

    }
}
