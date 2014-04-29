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
        private double x;
        public double X
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

        private double y;
        public double Y
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

        private int id;
        public int Id
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

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private bool deleted = false;
        public bool Deleted
        {
            get { return deleted;  }
            set
            {
                deleted = value;
                OnPropertyChanged();
            }
        } 

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
    }

    public class UIJunctionPoint : UIObject
    {
        public UIJunctionPoint(JunctionPoint j)
        {
            X = j.X;
            Y = j.Y;
            Id = j.Id;
        }
    }

    public class UICollectionVat : UIObject
    {
        public UICollectionVat(int id)
        {
            Id = id;
            Name = "Vat "+id;
        }
    }
}
