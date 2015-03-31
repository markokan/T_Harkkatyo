using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Samuxi.WPF.Harjoitus.Model
{
    public interface IGame : INotifyPropertyChanged
    {
        PlayerSide Turn { get; set; }
        string Name { get; set; }
        GameSize Size { get; set; }
        Player PlayerWhite { get; set; }
        Player PlayerBlack { get; set; }
        bool IsValidMovement(BoardItem boardItem, Point point);
        void Move(BoardItem boardItem, GamePosition toPosition);
        void CreateGame();
        ObservableCollection<BoardItem> BoardItems { get; set; }
        Player Winner { get; set; }
    }
}
