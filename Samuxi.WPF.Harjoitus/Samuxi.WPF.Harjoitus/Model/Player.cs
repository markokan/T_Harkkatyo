using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Model
{
    [Serializable]
    public class Player : BaseModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = Name;
                OnPropertyChanged();
            }
        }

        private Color _symbolColor;
        public Color SymbolColor
        {
            get { return _symbolColor; }
            set
            {
                _symbolColor = value;
                OnPropertyChanged();
            }
        }

        private PlayerSide _playerSide;
        public PlayerSide Side
        {
            get {  return _playerSide;}
            set
            {
                _playerSide = value;
                OnPropertyChanged();
            }
        }

        private MarkerSymbol _playerSymbol;
        public MarkerSymbol MarkerSymbol
        {
            get { return _playerSymbol; }
            set
            {
                _playerSymbol = value;
                OnPropertyChanged();
            }
        }
    }
}
