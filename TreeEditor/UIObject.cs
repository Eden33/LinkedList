using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Graph.Model;

namespace TreeEditor
{
    public abstract class UIObject : INotifyPropertyChanged
    {
        public abstract double X { get; set; }
        public abstract double Y { get; set; }
        public abstract int Id { get; set;  }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class UINode : UIObject
    {
        public UINode(Node n)
        {
            X = n.X;
            Y = n.Y;
            Id = n.Id;
        }
        private double x;
        private double y;
        private int id;

        public override double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChanged();
            }
        }

        public override double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChanged();
            }
        }

        public override int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
    }

    public class UIJunctionPoint : UIObject
    {
        public UIJunctionPoint(JunctionPoint j)
        {
            X = j.X;
            Y = j.Y;
            Id = j.Id;
        }

        private double x;
        private double y;
        private int id;

        public override double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChanged();
            }
        }

        public override double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChanged();
            }
        }

        public override int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
    }
}
