using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// @version 26.4.2015
    /// @author Marko Kangas
    /// 
    /// <summary>
    ///  Breakthrough game implementation. 
    /// </summary>
    public sealed class BreakthroughGame : BaseGame
    {
        private const int MOVE_X = 1;
        private const int MOVE_Y = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakthroughGame"/> class.
        /// </summary>
        public BreakthroughGame()
        {
            Turn = PlayerSide.WhiteSide; //Default
        }

        /// <summary>
        /// Determines whether [is valid movement] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public override bool IsValidMovement(BoardItem item)
        {
            return false;
        }

        /// <summary>
        /// Determines whether [is valid movement]
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="point">The point.</param>
        /// <returns>bool value indicating is movement valid.</returns>
        public override bool IsValidMovement(BoardItem item, System.Windows.Point point)
        {
            int movementX = (int)Math.Round(point.X, MOVE_X);
            int movementY = (int)Math.Round(point.Y, MOVE_Y);

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
            if (siirtoY == MOVE_Y)
            {
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

                if (siirtoX > MOVE_X || siirtoX < MOVE_X * -1)
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

        /// <summary>
        /// Gets the possible moves.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public override List<GamePosition> GetPossibleMoves(BoardItem item)
        {
            var retVal = new List<GamePosition>();

            // hae kaikki jotka ovat yhden päässä Y suunnassa (3 kappaletta max)
            int columnMin = item.Column == 0 ? item.Column : item.Column - MOVE_Y;
            int columnMax = item.Column == Size.Columns - 1 ? item.Column : item.Column + MOVE_Y;

            int row = item.Side == PlayerSide.BlackSide ? item.Row + MOVE_X : item.Row - MOVE_X;

            for (int x = columnMin; x <= columnMax; x++)
            {
                var position = new GamePosition {Column = x, Row = row};
                var foundItem = GetItem(position, true);

                if (foundItem != null && foundItem.Side == PlayerSide.None)
                {
                    retVal.Add(position);
                }
            }

            return retVal;
        }


        /// <summary>
        /// Gets all possible moves.
        /// </summary>
        /// <param name="side">The side.</param>
        /// <returns></returns>
        public override List<Move> GetAllPossibleMoves(PlayerSide side)
        {
            var moves = new List<Move>();

            foreach (var item in BoardItems.Where(c => c.Side == side))
            {
                var items = GetPossibleMoves(item);

                foreach (var move in items)
                {
                    moves.Add(new Move { Id = item.Id, Position = new GamePosition {Column = move.Column, Row = move.Row }});
                }
            }

            return moves;
        }

        /// <summary>
        /// Creates the game.
        /// </summary>
        public override void CreateGame()
        {
            BoardItems = new ObservableCollection<BoardItem>();
            PlayedMoves = new ObservableCollection<Move>();
            Winner = null;

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

        /// <summary>
        /// Replay this BreakthroughGame.
        /// </summary>
        public override void Replay()
        {
            // Take playedmoves to memory to replay these moves.
            var playedMoves = PlayedMoves;
            // Create game newly and make moves again..
            CreateGame();

            var firstMover = playedMoves.FirstOrDefault();

            if (firstMover != null)
            {
                Turn = firstMover.Mover.Side;

                Task tempTask = new Task(() =>
                {
                    IsReplayRunning = true;

                    for (int i = 0; i < playedMoves.Count(); i++)
                    {
                        var boardItem = BoardItems.FirstOrDefault(c => c.Id == playedMoves[i].Id);

                        if (boardItem != null)
                        {
                            var position = playedMoves[i].Position;
                            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => Move(boardItem, position)));
                            Thread.Sleep(1000);
                        }
                    }
                    
                    playedMoves = null;
                    IsReplayRunning = false;

                }, TaskCreationOptions.None);

                tempTask.Start();
            }
            
        }
    }
}
