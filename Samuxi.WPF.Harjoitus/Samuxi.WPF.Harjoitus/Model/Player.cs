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

        private Color _pawnColor;
        public Color PawnColor
        {
            get { return _pawnColor; }
            set
            {
                _pawnColor = value;
                OnPropertyChanged();
            }
        }

        private List<Pawn> _pawns;
        public List<Pawn> Pawns
        {
            get { return _pawns;}
            set
            {
                _pawns = value;
                OnPropertyChanged();
            }
        }
    }
}
