using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    public sealed class CheckerGame : BaseGame
    {
        public CheckerGame()
        {
            Turn = PlayerSide.WhiteSide;
            BoardItems = new ObservableCollection<BoardItem>();
        }

        public override void CreateGame()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < Size.Columns; x++)
                {
                    bool isBlack = ((y % 2 == 0 && x % 2 == 1) || (y % 2 == 1 && x % 2 == 0));

                    if (isBlack)
                    {
                        BoardItems.Add(new BoardItem
                        {
                            Column = x,
                            Row = y,
                            FillBrush = new SolidColorBrush(PlayerBlack.SymbolColor),
                            Side = PlayerSide.BlackSide,
                            Symbol = PlayerBlack.MarkerSymbol
                        });
                    }
                    else 
                    {
                        BoardItems.Add(new BoardItem
                        {
                            Column = x,
                            Row = Size.Rows - y - 1,
                            FillBrush = new SolidColorBrush(PlayerWhite.SymbolColor),
                            Side = PlayerSide.WhiteSide,
                            Symbol = PlayerWhite.MarkerSymbol
                        });
                    }
                }
            }

            CreateDummyItems();
        }

    
        public override bool IsValidMovement(BoardItem item, System.Windows.Point point)
        {
            var possibleMoves = GetPossibleMoves(item);

            if (possibleMoves != null)
            {
                foreach (var ix in possibleMoves)
                {
                    var boardItem = GetItem(ix, true);
                    boardItem.IsPossibleMove = true;
                    System.Diagnostics.Debug.WriteLine("Possible pos: {0} {1}", ix.Row, ix.Column);
                }


                var tryingMove = new GamePosition(point);
                var found = possibleMoves.FirstOrDefault(c => c.Column == tryingMove.Column && c.Row == tryingMove.Row);
                return found != null;
            }

            return false;
        }

        public List<Move> GetAllPossibleMoves(PlayerSide sideOfPlayer)
        {
            var allMoves = new List<Move>();

            foreach (var boardItem in BoardItems.Where(c => c.Side == sideOfPlayer))
            {
                bool forceMoves;
                var moves = GetPossibleMoves(boardItem, out forceMoves);
                if (moves != null)
                {
                    allMoves.AddRange(moves.Select(possibleMove => new Move {Id = boardItem.Id, Position = possibleMove}));

                    if (forceMoves) break;
                }
            }

            return allMoves;
        }

        public List<GamePosition> GetPossibleMoves(BoardItem item, out bool forceMoves)
        {
            var retVal = new List<GamePosition>();

            int rowAdjust = item.Side == PlayerSide.BlackSide ? 1 : -1;

            var movementOne = BoardItems.FirstOrDefault(c => c.Column == item.Column - 1 && c.Row == item.Row + rowAdjust && c.Side != PlayerSide.None);
            var movementTwo = BoardItems.FirstOrDefault(c => c.Column == item.Column + 1 && c.Row == item.Row + rowAdjust && c.Side != PlayerSide.None);

            BoardItem movementThree = null;
            BoardItem movementFour = null;

            if (item.IsKing)
            {
                movementThree = BoardItems.FirstOrDefault(c => c.Column == item.Column - 1 && c.Row == item.Row - rowAdjust && c.Side != PlayerSide.None);
                movementFour = BoardItems.FirstOrDefault(c => c.Column == item.Column + 1 && c.Row == item.Row - rowAdjust && c.Side != PlayerSide.None);
            }

            forceMoves = false;

            if (movementOne != null || movementTwo != null)
            {
                if (movementTwo != null && movementTwo.Side != item.Side)
                {
                    if (movementTwo.Column + 1 <= Size.Columns - 1)
                    {
                        var positionTocheck = new GamePosition
                        {
                            Column = movementTwo.Column + 1,
                            Row = movementTwo.Row + rowAdjust
                        };

                        var jumpItem = GetItem(positionTocheck);

                        if (jumpItem == null)
                        {
                            retVal.Add(positionTocheck);
                            forceMoves = true;
                        }
                    }
                }

                if (movementOne != null && movementOne.Side != item.Side)
                {
                    if (movementOne.Column - 1 >= 0)
                    {
                        var positionTocheck = new GamePosition
                        {
                            Column = movementOne.Column - 1,
                            Row = movementOne.Row + rowAdjust
                        };

                        var jumpItem = GetItem(positionTocheck);

                        if (jumpItem == null)
                        {
                            retVal.Add(positionTocheck);
                            forceMoves = true;
                        }
                    }
                }

               
            }

            // No must moves..
            if (!forceMoves)
            {
                if (movementTwo == null && item.Column + 1 <= Size.Columns - 1)
                {
                    retVal.Add(new GamePosition { Column = item.Column + 1, Row = item.Row + rowAdjust });
                }

                if (movementOne == null && item.Column - 1 >= 0)
                {
                    retVal.Add(new GamePosition { Column = item.Column - 1, Row = item.Row + rowAdjust });
                }

                if (item.IsKing)
                {
                    if (movementThree == null && item.Column - 1 >= 0 && item.Row - 1 > 0)
                    {
                        retVal.Add(new GamePosition {Column = item.Column - 1, Row = item.Row - rowAdjust});
                    }

                    if (movementFour == null && item.Column + 1 <= Size.Columns - 1 && item.Row - 1 > 0)
                    {
                        retVal.Add(new GamePosition {Column = item.Column + 1, Row = item.Row - rowAdjust});
                    }
                }
            }

            return retVal.Any() ? retVal : null;
        }

        public override List<GamePosition> GetPossibleMoves(BoardItem item)
        {
            var moves = GetAllPossibleMoves(item.Side);

            if (moves != null && moves.Any())
            {
                var items = moves.Where(c => c.Id == item.Id).ToList();
                return items.Select(c => c.Position).ToList();
            }

            return null;
        }

        public override void Move(BoardItem boardItem, GamePosition toPosition)
        {
            HandleMove(boardItem, toPosition);

            // Syötiinkö jotain?
            EatSomething(boardItem, toPosition);

            boardItem.Row = toPosition.Row;
            boardItem.Column = toPosition.Column;

            if (!boardItem.IsKing && (boardItem.Side == PlayerSide.BlackSide && boardItem.Row == Size.Rows - 1 ||
                (boardItem.Side == PlayerSide.WhiteSide && boardItem.Row == 0)))
            {
                // kuningas/tammi merkki .. muuntuu
                boardItem.IsKing = true;
            }
            else
            {
                Turn = Turn == PlayerSide.BlackSide ? PlayerSide.WhiteSide : PlayerSide.BlackSide;
            }

            ClearPossibleMoveItems();
        }


        /// <summary>
        /// Eats opponent boarditems.
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <param name="toPosition">To position.</param>
        private void EatSomething(BoardItem boardItem, GamePosition toPosition)
        {
            if (Math.Abs(toPosition.Column - boardItem.Column) == 2)
            {
                GamePosition position = new GamePosition();

                if (toPosition.Column > boardItem.Column)
                {
                    position.Column = toPosition.Column - 1;
                }
                else
                {
                    position.Column = boardItem.Column - 1;
                }

                if (toPosition.Row > boardItem.Row)
                {
                    position.Row = toPosition.Row - 1;
                }
                else
                {
                    position.Row = boardItem.Row - 1;
                }
               

                var removeOppositeItem = GetItem(position);

                if (removeOppositeItem != null)
                {
                    var dummyItem = CreateDummyBoardItem(position);
                    BoardItems.Remove(removeOppositeItem);
                    BoardItems.Add(dummyItem);
                }
            }
        }
    }
}
