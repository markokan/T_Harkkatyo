using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// @author Marko Kangas
    /// @version 26.4.2015
    /// 
    /// <summary>
    /// Checker game logic.
    /// </summary>
    public sealed class CheckerGame : BaseGame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckerGame"/> class.
        /// </summary>
        public CheckerGame()
        {
            Turn = PlayerSide.WhiteSide; //Default 
            BoardItems = new ObservableCollection<BoardItem>();
        }

        /// <summary>
        /// Creates the Checker game.
        /// </summary>
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

            CreateDummyItems(GameType.Checker);
        }


        /// <summary>
        /// Determines whether [is valid movement] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public override bool IsValidMovement(BoardItem item, System.Windows.Point point)
        {
            var possibleMoves = GetPossibleMoves(item);

            if (possibleMoves != null)
            {
                foreach (var ix in possibleMoves)
                {
                    var boardItem = GetItem(ix, true);

                    if (boardItem != null)
                    {
                        boardItem.IsPossibleMove = true;
                    }
                }

                var tryingMove = new GamePosition(point);
                var found = possibleMoves.FirstOrDefault(c => c.Column == tryingMove.Column && c.Row == tryingMove.Row);
                return found != null;
            }

            return false;
        }


        /// <summary>
        /// Determines whether [is valid movement] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public override bool IsValidMovement(BoardItem item)
        {
            ClearPossibleMoveItems();

            if (!item.IsSelected)
            {
                var possibleMoves = GetPossibleMoves(item);

                if (possibleMoves != null)
                {
                    foreach (var ix in possibleMoves)
                    {
                        var boardItem = GetItem(ix, true);
                        if (boardItem != null)
                        {
                            boardItem.IsPossibleMove = true;
                        }
                    }

                    return possibleMoves.Any();
                }
            }

            return false;
        }


        /// <summary>
        /// Gets all possible moves.
        /// </summary>
        /// <param name="sideOfPlayer">The side of player.</param>
        /// <returns></returns>
        public override List<Move> GetAllPossibleMoves(PlayerSide sideOfPlayer)
        {
            var allMoves = new List<Move>();

            foreach (var boardItem in BoardItems.Where(c => c.Side == sideOfPlayer))
            {
                bool forceMoves;
                var moves = GetPossibleMoves(boardItem, out forceMoves);
                if (moves != null)
                {
                    if (forceMoves)
                    {
                        return moves.Select(possibleMove => new Move { Id = boardItem.Id, Position = possibleMove }).ToList();
                    }

                    allMoves.AddRange(moves.Select(possibleMove => new Move {Id = boardItem.Id, Position = possibleMove}));
                }
            }

            return allMoves;
        }


        /// <summary>
        /// Gets the possible moves.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="forceMoves">if set to <c>true</c> [force moves].</param>
        /// <returns>list of gamepositions to move board item</returns>
        public List<GamePosition> GetPossibleMoves(BoardItem item, out bool forceMoves)
        {
            var retVal = new List<GamePosition>();
            forceMoves = false;

            int rowAdjust = item.Side == PlayerSide.BlackSide ? 1 : -1;
            PlayerSide targetSide = item.Side == PlayerSide.BlackSide ? PlayerSide.WhiteSide : PlayerSide.BlackSide;

            // Basic movements
            var basicMovements = BoardItems.Where(c => (c.Column == item.Column - 1 || c.Column == item.Column + 1)
                                      && c.Row == item.Row + rowAdjust && (c.Side == targetSide  || c.Side == PlayerSide.None)).ToList();

            if (!item.IsKing && !basicMovements.Any())
            {
                return null;
            }

            List<BoardItem> kingMovements = null;

            if (item.IsKing)
            {
                // King movements
                kingMovements = BoardItems.Where(c => (c.Column == item.Column - 1 || c.Column == item.Column + 1)
                                      && c.Row == item.Row + (rowAdjust*-1) && (c.Side == targetSide || c.Side == PlayerSide.None)).ToList();
            }

            // Something to eat?
            List<GamePosition> eatablePositions = GetEatable(item, basicMovements, targetSide);
            
            if (eatablePositions.Any())
            {
                retVal = eatablePositions;
                forceMoves = true;
            }

            if (item.IsKing)
            {
                // Feed king !
                List<GamePosition> kingEatablemoves = GetEatable(item, kingMovements, targetSide);
                if (kingEatablemoves.Any())
                {
                    retVal.AddRange(kingEatablemoves);
                    forceMoves = true;
                }
            }

            if (!forceMoves)
            {
                retVal.AddRange(basicMovements.Where(c => c.Side == PlayerSide.None).Select(c => c.GamePosition));

                if (item.IsKing && kingMovements != null)
                {
                    retVal.AddRange(kingMovements.Where(c => c.Side == PlayerSide.None).Select(c => c.GamePosition));
                }
            }

            return retVal.Any() ? retVal : null;
        }


        /// <summary>
        /// Gets the eatable directions.
        /// </summary>
        /// <param name="currentBoardItem">The current board item.</param>
        /// <param name="possibleEatBoardItems">The possible eat board items.</param>
        /// <param name="targetSide">The target side.</param>
        /// <returns>list of gamepositions</returns>
        private List<GamePosition> GetEatable(BoardItem currentBoardItem, List<BoardItem> possibleEatBoardItems, PlayerSide targetSide)
        {
            List<GamePosition> retVal = new List<GamePosition>();

            foreach (BoardItem boardItem in possibleEatBoardItems.Where(c => c.Side == targetSide))
            {
                int toColumn = currentBoardItem.Column < boardItem.Column ? 1 : -1;
                int rowAdjust = currentBoardItem.Row < boardItem.Row ? 1 : -1;

                var positionTocheck = new GamePosition
                {
                    Column = boardItem.Column + toColumn,
                    Row = boardItem.Row + rowAdjust
                };

                if (positionTocheck.Column >= 0 && positionTocheck.Column < Size.Columns
                    && positionTocheck.Row >= 0 && positionTocheck.Row < Size.Rows)
                {
                    var jumpItem = GetItem(positionTocheck);

                    if (jumpItem == null)
                    {
                        positionTocheck.IsEat = true;
                        retVal.Add(positionTocheck);
                    }
                }
            }

            return retVal;
        }


        /// <summary>
        /// Gets the possible moves.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>list of gamepositions</returns>
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


        /// <summary>
        /// Moves the specified board item.
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <param name="toPosition">To position.</param>
        public override void Move(BoardItem boardItem, GamePosition toPosition)
        {
            HandleMove(boardItem, toPosition);

            // Syötiinkö jotain?
            bool isEated = EatSomething(boardItem, toPosition);

            boardItem.Row = toPosition.Row;
            boardItem.Column = toPosition.Column;

            if (!boardItem.IsKing && (boardItem.Side == PlayerSide.BlackSide && boardItem.Row == Size.Rows - 1 ||
                (boardItem.Side == PlayerSide.WhiteSide && boardItem.Row == 0)))
            {
                // kuningas/tammi merkki .. muuntuu
                boardItem.IsKing = true;
                Turn = boardItem.Side;
            }
            else
            {
                bool forceMoves;
                var moves = GetPossibleMoves(boardItem, out forceMoves);

                // If something was eated check is there more possible "must" eates...
                if (forceMoves && isEated)
                {
                    var anythingToEat = moves.FirstOrDefault(c => c.IsEat);
                    if (anythingToEat != null)
                    {
                        Move(boardItem, anythingToEat);
                    }
                }
                else // change turn
                {
                    Turn = Turn == PlayerSide.BlackSide ? PlayerSide.WhiteSide : PlayerSide.BlackSide;
                    boardItem.IsSelected = false;
                }
            }

            ClearPossibleMoveItems();

            if (IsWinner(boardItem.Side))
            {
                Winner = CurrentPlayer;
            }
        }

        /// <summary>
        /// Determines whether the specified side is winner.
        /// </summary>
        /// <param name="side">The side.</param>
        /// <returns></returns>
        private bool IsWinner(PlayerSide side)
        {
            return BoardItems.FirstOrDefault(c => c.Side != PlayerSide.None && c.Side != side) == null;
        }


        /// <summary>
        /// Eats opponent boarditems.
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <param name="toPosition">To position.</param>
        private bool EatSomething(BoardItem boardItem, GamePosition toPosition)
        {
            bool retval = false;

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
                    retval = true;
                }
            }

            return retval;
        }

        /// <summary>
        /// Chooses the item and check possible moves.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void ChooseItemAndCheckPossibleMoves(BoardItem item)
        {
            if (IsValidMovement(item))
            {
                var choosedItem = BoardItems.FirstOrDefault(c => c.IsSelected);
                if (choosedItem != null)
                {
                    choosedItem.IsSelected = false;

                    if (choosedItem.Id != item.Id)
                    {
                        item.IsSelected = true;
                    }
                }
                else
                {
                    item.IsSelected = true;
                }
            }
        }
    }
}
