using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// <summary>
    ///  Breakthrough game implementation. 
    /// </summary>
    public sealed class BreakthroughGame : BaseGame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BreakthroughGame"/> class.
        /// </summary>
        public BreakthroughGame()
        {
            Turn = PlayerSide.WhiteSide;
          
            BoardItems = new ObservableCollection<BoardItem>();
        }


        /// <summary>
        /// Determines whether [is valid movement] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="point">The point.</param>
        /// <returns>bool value indicating is movement valid.</returns>
        public override bool IsValidMovement(BoardItem item, System.Windows.Point point)
        {
            int movementX = (int) Math.Round(point.X, 1);
            int movementY = (int) Math.Round(point.Y,1);

            System.Diagnostics.Debug.WriteLine("Point: {0} {1} - Item {2} {3}", movementX, movementY, item.Column, item.Row);

            var possibleMoves = GetPossibleMoves(item);
            if (possibleMoves != null)
            {
                foreach (var pos in possibleMoves)
                {
                    var boardItem = GetItem(pos, true);
                    boardItem.IsPossibleMove = true;
                }
            }

            int siirtoY = item.Side == PlayerSide.BlackSide ? movementY - item.Row : item.Row - movementY;
            int siirtoX = movementX - item.Column;

            // onko siirto yhden?
            if (siirtoY == 1)
            {
                System.Diagnostics.Debug.WriteLine("Item: {0} {1}", item.Column, item.Row);

                // onko siirto toisen pelaajan päälle
                var found = GetItem(new GamePosition {Column = movementX, Row = movementY});

                if (found != null)
                {
                    if (found.Side != item.Side && found.Column != item.Column)
                    {
                        return true;
                    }

                    return false;
                }

                if (siirtoX > 1 || siirtoX < -1)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Moves the specified board item.
        /// </summary>
        /// <param name="boardItem">The Boarditem.</param>
        /// <param name="toPosition">To position.</param>
        public override void Move(BoardItem boardItem, GamePosition toPosition)
        {
            HandleMove(boardItem, toPosition);

            boardItem.Row = toPosition.Row;
            boardItem.Column = toPosition.Column;

            if (boardItem.Side == PlayerSide.BlackSide && boardItem.Row == Size.Rows - 1)
            {
                Winner = PlayerBlack;
            }
            else if (boardItem.Row == 0)
            {
                Winner = PlayerWhite;
            }

            ClearPossibleMoveItems();

            Turn = Turn == PlayerSide.BlackSide ? PlayerSide.WhiteSide : PlayerSide.BlackSide;
        }

        public override List<GamePosition> GetPossibleMoves(BoardItem item)
        {
            var retVal = new List<GamePosition>();

            // hae kaikki jotka ovat yhden päässä Y suunnassa (3 kappaletta)
            int columnMin = item.Column == 0 ? item.Column : item.Column - 1;
            int columnMax = item.Column == Size.Columns - 1 ? item.Column : item.Column + 1;

            int row = item.Side == PlayerSide.BlackSide ? item.Row + 1 : item.Row - 1;

            for (int x = columnMin; x <= columnMax; x++)
            {
                var position = new GamePosition {Column = x, Row = row};
                var foundItem = GetItem(position, true);

                if (foundItem != null && (foundItem.Side == PlayerSide.None ||
                                          (foundItem.Side != PlayerSide.None && foundItem.Side != item.Side &&
                                           foundItem.Column != item.Column)))
                {
                    retVal.Add(position);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Creates the game.
        /// </summary>
        public override void CreateGame()
        {
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < Size.Columns; x++)
                {
                    BoardItems.Add(new BoardItem { Column = x, Row = y, FillBrush = new SolidColorBrush(PlayerBlack.SymbolColor), Side = PlayerSide.BlackSide, Symbol = PlayerBlack.MarkerSymbol});
                    BoardItems.Add(new BoardItem { Column = x, Row = Size.Rows - y - 1, FillBrush = new SolidColorBrush(PlayerWhite.SymbolColor), Side = PlayerSide.WhiteSide, Symbol = PlayerWhite.MarkerSymbol});
                }
            }

            CreateDummyItems();
        }
    }
}
