using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// <summary>
    /// Interface to Games.
    /// </summary>
    public interface IGame : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the turn.
        /// </summary>
        /// <value>
        /// The turn.
        /// </value>
        PlayerSide Turn { get; set; }

        /// <summary>
        /// Gets or sets the game size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        GameSize Size { get; set; }

        /// <summary>
        /// Gets or sets the player white.
        /// </summary>
        /// <value>
        /// The player white.
        /// </value>
        Player PlayerWhite { get; set; }

        /// <summary>
        /// Gets or sets the player black.
        /// </summary>
        /// <value>
        /// The player black.
        /// </value>
        Player PlayerBlack { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        GameSetting Setting { get; set; }

        /// <summary>
        /// Gets the current player.
        /// </summary>
        /// <value>
        /// The current player.
        /// </value>
        Player CurrentPlayer { get; }
        
        /// <summary>
        /// Determines whether [is valid movement] [the specified board item].
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        bool IsValidMovement(BoardItem boardItem, Point point);


        /// <summary>
        /// Moves the specified board item.
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <param name="toPosition">To position.</param>
        void Move(BoardItem boardItem, GamePosition toPosition);


        /// <summary>
        /// Creates the game.
        /// </summary>
        void CreateGame();

        /// <summary>
        /// Undoes last movement.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redoes last movement
        /// </summary>
        void Redo();

        /// <summary>
        /// Saves this instance.
        /// </summary>
        void Save();

        //void Load(ObservableCollection<BoardItem> loadedItems);

        /// <summary>
        /// Gets the possible moves.
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <returns>Possible moves to board item</returns>
        List<GamePosition> GetPossibleMoves(BoardItem boardItem);

        /// <summary>
        /// Gets or sets the board items.
        /// </summary>
        /// <value>
        /// The board items.
        /// </value>
        ObservableCollection<BoardItem> BoardItems { get; set; }

        /// <summary>
        /// Gets or sets the played moves.
        /// </summary>
        /// <value>
        /// The played moves.
        /// </value>
        ObservableCollection<Move> PlayedMoves { get; set; }

        /// <summary>
        /// Gets or sets the winner player.
        /// </summary>
        /// <value>
        /// The winner.
        /// </value>
        Player Winner { get; set; }
    }
}
