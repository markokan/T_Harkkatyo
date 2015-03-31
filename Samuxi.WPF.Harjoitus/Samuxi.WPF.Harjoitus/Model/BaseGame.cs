using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Samuxi.WPF.Harjoitus.Model
{
    public abstract class BaseGame : IGame
    {
        

        private PlayerSide _turn;
        public virtual PlayerSide Turn
        {
            get { return _turn;}
            set
            {
                _turn = value;
                OnPropertyChanged();
            }
        }
        public virtual string Name { get; set; }
        public virtual GameSize Size { get; set; }
        
        public virtual ObservableCollection<BoardItem> BoardItems { get; set; }

        public abstract bool IsValidMovement(BoardItem item, System.Windows.Point point);
        public abstract void Move(BoardItem boardItem, GamePosition toPosition);
        public abstract void CreateGame();

        private Player _playerBlack;
        public Player PlayerBlack
        {
            get { return _playerBlack; }
            set
            {
                _playerBlack = value;
                OnPropertyChanged();
            }
        }

        private Player _playerWhite;
        public Player PlayerWhite
        {
            get { return _playerWhite; }
            set
            {
                _playerWhite = value;
                OnPropertyChanged();
            }
        }

        private Player _winner;
        public Player Winner
        {
            get
            {
                return _winner;
                
            }
            set
            {
                _winner = value;
                OnPropertyChanged();
            }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
