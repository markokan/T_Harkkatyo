using System;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// @version 26.4.2015
    /// @author Marko Kangas
    /// 
    /// <summary>
    /// Single movement is logged to this object.
    /// </summary>
    [Serializable]
    public class Move : BaseModel
    {
        private string _id;

        /// <summary>
        /// Gets or sets the identifier of boarditem.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _toId;
        /// <summary>
        /// Gets or sets to identifier.
        /// </summary>
        /// <value>
        /// To identifier.
        /// </value>
        public string ToId
        {
            get { return _toId; }
            set
            {
                _toId = value;
                OnPropertyChanged();
            }

        }

        private GamePosition _position;
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public GamePosition Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }

        private Player _mover;

        /// <summary>
        /// Gets or sets the mover.
        /// </summary>
        /// <value>
        /// The mover.
        /// </value>
        public Player Mover
        {
            get { return _mover; }
            set
            {
                _mover = value;
                OnPropertyChanged();
            }
        }

        private BoardItem _removedBoardItem;
        /// <summary>
        /// Gets or sets the removed board item.
        /// </summary>
        /// <value>
        /// The removed board item.
        /// </value>
        public BoardItem RemovedBoardItem
        {
            get { return _removedBoardItem; }
            set
            {
                _removedBoardItem = value;
                OnPropertyChanged();
            }
        }

        private BoardItem _addedBoardItem;
        /// <summary>
        /// Gets or sets the added board item.
        /// </summary>
        /// <value>
        /// The added board item.
        /// </value>
        public BoardItem AddedBoardItem
        {
            get { return _addedBoardItem; }
            set
            {
                _addedBoardItem = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the print format.
        /// </summary>
        /// <value>
        /// The print format.
        /// </value>
        public string PrintFormat
        {
            get { return string.Format("{0} -> {1} to {2}", Mover.Name, Id, ToId); }
        }
    }
}
