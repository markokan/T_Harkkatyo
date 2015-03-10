using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samuxi.WPF.Harjoitus.Model
{
    public class BreakThroungGame : IGame
    {
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public GameSize Size
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public GameType TypeOfGame
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public List<Player> Players
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsValidMovement(Pawn pawn, System.Windows.Point point)
        {
            throw new NotImplementedException();
        }

        public void Move(Pawn pawn, System.Windows.Point toPoint)
        {
            throw new NotImplementedException();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
