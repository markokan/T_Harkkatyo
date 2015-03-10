using System;
using System.Windows;

namespace Samuxi.WPF.Harjoitus.Model
{
    [Serializable]
    public class Pawn : BaseModel
    {
        private bool _isDeleted;
        public bool IsDeleted
        {
            get {  return _isDeleted;}
            set
            {
                _isDeleted = value;
                OnPropertyChanged();
            }
        }

        private Point _position;
        public Point Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }
    }
}
