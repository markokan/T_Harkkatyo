using System;
using System.Collections.Generic;
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
        string Name { get; set; }
        GameSize Size { get; set; }
        GameType TypeOfGame { get; set; }
        List<Player> Players { get; set; }
        bool IsValidMovement(Pawn pawn, Point point);
        void Move(Pawn pawn, Point toPoint);
        
    }
}
