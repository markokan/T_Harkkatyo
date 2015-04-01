using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private bool IsBlack(GamePosition position)
        {
            return (position.Row%2 == 1 && position.Column%2 == 0) || (position.Row%2 == 0 && position.Column%2 == 1);
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

        public override List<GamePosition> GetPossibleMoves(BoardItem item)
        {
            var retVal = new List<GamePosition>();

            if (item.Symbol != MarkerSymbol.King)
            {
                int rowAdjust = item.Side == PlayerSide.BlackSide ? 1 : -1;
                
                var movementOne = BoardItems.FirstOrDefault(c => c.Column == item.Column - 1 && c.Row == item.Row + rowAdjust && c.Side != PlayerSide.None);
                var movementTwo = BoardItems.FirstOrDefault(c => c.Column == item.Column + 1 && c.Row == item.Row + rowAdjust && c.Side != PlayerSide.None);

                if (movementTwo == null && item.Column + 1 < Size.Columns - 1)
                {
                    retVal.Add(new GamePosition { Column = item.Column + 1, Row = item.Row + rowAdjust });
                }

                if (movementOne == null && item.Column - 1 >= 0)
                {
                    retVal.Add(new GamePosition { Column = item.Column - 1, Row = item.Row + rowAdjust });
                }

                if (movementOne != null || movementTwo != null)
                {
                    if (movementTwo != null && movementTwo.Side != item.Side)
                    {
                        if (movementTwo.Column + 1 < Size.Columns - 1)
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
                            }
                        }
                    }
                }
            }

            return retVal.Any() ? retVal : null;
        }

        public override void Move(BoardItem boardItem, GamePosition toPosition)
        {
            if (boardItem.Symbol != MarkerSymbol.King)
            {
                
            }

            HandleMove(boardItem, toPosition);

            boardItem.Row = toPosition.Row;
            boardItem.Column = toPosition.Column;

            if (boardItem.Side == PlayerSide.BlackSide && boardItem.Row == Size.Rows - 1)
            {
                // kuningas..
            }
            else if (boardItem.Row == 0)
            {
                // kuningas..
            }

            Turn = Turn == PlayerSide.BlackSide ? PlayerSide.WhiteSide : PlayerSide.BlackSide;

            ClearPossibleMoveItems();
        }
    }
}
