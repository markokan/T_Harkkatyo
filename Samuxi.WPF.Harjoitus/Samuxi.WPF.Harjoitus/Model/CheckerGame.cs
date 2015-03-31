using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    public sealed class CheckerGame : BaseGame
    {
        private const int BoardItemsPerSize = 12;

        public CheckerGame()
        {
            Size = new GameSize { Columns = 8, Rows = 8 };

        }

        public override void CreateGame()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < BoardItemsPerSize / 3; x++)
                {

                    BoardItems.Add(new BoardItem
                    {
                        Column = x, 
                        Row = y, 
                        FillBrush = new SolidColorBrush(PlayerBlack.SymbolColor), 
                        Side = PlayerSide.BlackSide, 
                        Symbol = PlayerBlack.MarkerSymbol
                    });

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

        public override bool IsValidMovement(BoardItem item, System.Windows.Point point)
        {
            throw new NotImplementedException();
        }

        public override void Move(BoardItem boardItem, GamePosition toPosition)
        {
            throw new NotImplementedException();
        }
    }
}
