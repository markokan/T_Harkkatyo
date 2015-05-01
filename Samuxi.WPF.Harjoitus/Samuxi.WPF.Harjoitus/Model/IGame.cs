﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// @version 26.4.2015
    /// @author Marko Kangas
    /// 
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
        /// Gets a value indicating whether this instance is game end.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is game end; otherwise, <c>false</c>.
        /// </value>
        bool IsGameEnd { get; }
        
        /// <summary>
        /// Determines whether [is valid movement] to specified point.
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        bool IsValidMovement(BoardItem boardItem, Point point);

        /// <summary>
        /// Determines whether [is valid movement]. 
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <returns></returns>
        bool IsValidMovement(BoardItem boardItem);


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
        /// <param name="isReplay">if set to <c>true</c> [is replay].</param>
        void Undo(bool isReplay = false);

        /// <summary>
        /// Redoes last movement
        /// </summary>
        void Redo();

        /// <summary>
        /// Gets the possible moves to board item.
        /// </summary>
        /// <param name="boardItem">The board item.</param>
        /// <returns>Possible moves to board item</returns>
        List<GamePosition> GetPossibleMoves(BoardItem boardItem);

        /// <summary>
        /// Gets all possible moves.
        /// </summary>
        /// <param name="side">The side.</param>
        /// <returns>POssible moves</returns>
        List<Move> GetAllPossibleMoves(PlayerSide side);

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

        /// <summary>
        /// Replay this Game.
        /// </summary>
        void Replay();

        void ChooseItemAndCheckPossibleMoves(BoardItem item);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is replay running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is replay running; otherwise, <c>false</c>.
        /// </value>
        bool IsReplayRunning { get; set; }
    }
}
