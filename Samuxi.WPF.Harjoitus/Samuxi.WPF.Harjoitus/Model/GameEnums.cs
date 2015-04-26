using System;
using System.Windows;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// @version 26.4.2015
    /// @author Marko Kangas
    /// 
    /// <summary>
    /// Gametype enum
    /// </summary>
    public enum GameType
    {
        [LocalizationAttribute("TextBreakthrough")]
        BreakThrough = 0,
        [LocalizationAttribute("TextChecker")]
        Checker = 1
    }

    /// <summary>
    /// Game size 8x8 or 16x16..
    /// </summary>
    public struct GameSize
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        public override string ToString()
        {
            return String.Format("{0} x {1}", Rows, Columns);
        }
    }

    /// <summary>
    /// Gameposition object.
    /// </summary>
    public class GamePosition
    {
        /// <summary>
        /// Gets or sets the row.
        /// </summary>
        /// <value>
        /// The row.
        /// </value>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>
        /// The column.
        /// </value>
        public int Column { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePosition"/> class.
        /// </summary>
        public GamePosition()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePosition"/> class.
        /// </summary>
        /// <param name="point">The point.</param>
        public GamePosition(Point point)
        {
            Row = (int) point.Y;
            Column = (int) point.X;
        }
    }

    /// <summary>
    /// Player side of game. White / Black.
    /// </summary>
    public enum PlayerSide
    {
        [LocalizationAttribute("TextNone")]
        None = 0,
        [LocalizationAttribute("TextWhiteSide")]
        WhiteSide = 1,
        [LocalizationAttribute("TextBlackSide")]
        BlackSide = 2
    }

    /// <summary>
    /// Different game symbols.
    /// </summary>
    public enum MarkerSymbol
    {
        Dummy = 0, // default dummy
        [LocalizationAttribute("Ellipse")]
        Ellipse = 1,
        [LocalizationAttribute("Triangle")]
        Triangle = 2,
        [LocalizationAttribute("Cubic")]
        Cubic = 3,
        Winner = 4
    }

    /// <summary>
    /// Defines is player human or computer.
    /// </summary>
    public enum PlayerType
    {
        [LocalizationAttribute("TextHuman")]
        Human = 0,
        [LocalizationAttribute("TextComputer")]
        Computer = 1
    }
}
