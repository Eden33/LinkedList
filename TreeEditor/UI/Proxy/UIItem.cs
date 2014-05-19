using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model.Data;
using TreeEditor.Resource;

namespace TreeEditor.UI.Proxy
{
    public abstract class UIItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Instances using the specific UIObject are supposed to release all instance references pointing to <br/>
        /// the specific item on Delete-PropertyChangeEvent. <br/>
        /// In addition, before releasing all references, further operations are possible which are item <br/>
        /// and implementation dependent.
        /// </summary>
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

        private UILockInfo lockInfo;
        public UILockInfo LockInfo
        {
            get { return lockInfo;  }
            set
            {
                lockInfo = value;
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
    
}
