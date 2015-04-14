using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xaml;
using Microsoft.Win32;
using Samuxi.WPF.Harjoitus.Model;

namespace Samuxi.WPF.Harjoitus.Utils
{
    public static class GameFileHandler
    {
        /// <summary>
        /// Saves the game.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="fullpath">The fullpath.</param>
        public static void SaveGame(object game, string fullpath)
        {
            GameObjectSerializer.Write(fullpath, game);
        }

        /// <summary>
        /// Loads the game.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static IGame LoadGame(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                return GameObjectSerializer.Read<object>(filename) as IGame;
            }

            return null;
        }


        /// <summary>
        /// Writes the game settings.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public static void WriteSettings(GameSetting setting)
        {
            GameObjectSerializer.Write<GameSetting>(@"asetukset.xml", setting);
        }

        /// <summary>
        /// Reads the game setting.
        /// </summary>
        /// <returns></returns>
        public static GameSetting ReadSetting()
        {
            if (System.IO.File.Exists("asetukset.xml"))
            {
                return GameObjectSerializer.Read<GameSetting>(@"asetukset.xml");
            }

            return null;
        }

        /// <summary>
        /// Saves the game result to text file.
        /// </summary>
        /// <param name="currentGame">The current game.</param>
        /// <param name="filename">The filename.</param>
        /// <exception cref="NotImplementedException"></exception>
        internal static void SaveGameResult(IGame currentGame, string filename)
        {
            if (currentGame != null)
            {
                using (TextWriter writer = File.CreateText(filename))
                {
                    foreach (var item in currentGame.PlayedMoves)
                    {
                        writer.WriteLine(item.PrintFormat);
                    }
                }
            }
        }
    }
}
