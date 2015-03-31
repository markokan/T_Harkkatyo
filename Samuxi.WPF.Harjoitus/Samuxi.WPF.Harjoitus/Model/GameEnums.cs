using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Samuxi.WPF.Harjoitus.Model
{
    public enum GameType
    {
        BreakThrough = 0,
        Tammi = 1
    }

    public struct GameSize
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
    }

    public struct GamePosition
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public enum PlayerSide
    {
        None = 0,
        WhiteSide = 1,
        BlackSide = 2
    }

    public enum MarkerSymbol
    {
        Ellipse = 0, // Default ellipse
        Triangle = 1,
        Cubic = 2,
        Winner = 3
    }
}
