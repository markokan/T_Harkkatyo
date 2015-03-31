using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    public sealed class BreakThroungGame : BaseGame
    {
        public override bool IsValidMovement(BoardItem item, System.Windows.Point point)
        {
            int movementX = (int) Math.Round(point.X, 1);
            int movementY = (int) Math.Round(point.Y,1);

            System.Diagnostics.Debug.WriteLine("Point: {0} {1}", movementX, movementY);

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

        public override void Move(BoardItem item, GamePosition toPosition)
        {
            var oppositeItem = GetItem(toPosition);
            if (oppositeItem != null)
            {
                BoardItems.Remove(oppositeItem);
            }
            
            item.Row = toPosition.Row;
            item.Column = toPosition.Column;

            if (item.Side == PlayerSide.BlackSide && item.Row == Size.Rows - 1)
            {
                Winner = PlayerBlack;
            }
            else if (item.Row == 0)
            {
                Winner = PlayerWhite;
            }

            Turn = Turn == PlayerSide.BlackSide ? PlayerSide.WhiteSide : PlayerSide.BlackSide;
        }

        public BreakThroungGame()
        {
            Size = new GameSize{ Columns = 8, Rows = 8};
            Turn = PlayerSide.WhiteSide;

            PlayerWhite = new Player {Name = "Matti", SymbolColor = Colors.Brown, Side = PlayerSide.WhiteSide, MarkerSymbol = MarkerSymbol.Ellipse};
            PlayerBlack = new Player {Name = "Pekka", SymbolColor = Colors.BurlyWood, Side = PlayerSide.BlackSide, MarkerSymbol = MarkerSymbol.Triangle};

            BoardItems = new ObservableCollection<BoardItem>();
            CreateGame();
        }

        public override void CreateGame()
        {
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < Size.Columns; x++)
                {
                    BoardItems.Add(new BoardItem {Column = x, Row = y, FillBrush = new SolidColorBrush(PlayerBlack.SymbolColor), Side = PlayerSide.BlackSide, Symbol = PlayerBlack.MarkerSymbol});
                    BoardItems.Add(new BoardItem { Column = x, Row = Size.Rows - y - 1, FillBrush = new SolidColorBrush(PlayerWhite.SymbolColor), Side = PlayerSide.WhiteSide, Symbol = PlayerWhite.MarkerSymbol});
                }
            }
        }

        public BoardItem GetItem(GamePosition position)
        {
            return BoardItems.FirstOrDefault(c => c.Column == position.Column && c.Row == position.Row);
        }

        public void LoadGame(ObservableCollection<BoardItem> items)
        {
            BoardItems = items;
            CreateGame();
        }
    }
}
