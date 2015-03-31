using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    public class BoardItem : BaseModel
    {
        private PlayerSide _playerSide;

        public PlayerSide Side
        {
            get {  return _playerSide; }
            set
            {
                _playerSide = value;
                OnPropertyChanged();
            }
        }

        private int _row;
        public int Row
        {
            get { return _row; }
            set
            {
                _row = value;
                OnPropertyChanged();
            }
            
        }

        private int _column;
        public int Column
        {
            get { return _column;}
            set
            {
                _column = value;
                OnPropertyChanged();
            }
        }

        private SolidColorBrush _fillBrush;
        public SolidColorBrush FillBrush
        {
            get { return _fillBrush; }
            set
            {
                _fillBrush = value;
                OnPropertyChanged();
            }
        }

        private MarkerSymbol _symbol;
        public MarkerSymbol Symbol
        {
            get { return _symbol;}
            set
            {
                _symbol = value;
                OnPropertyChanged();
            }
        }
    }
}
