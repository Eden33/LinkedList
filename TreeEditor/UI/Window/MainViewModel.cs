using Model.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TreeEditor.Resource;

using TreeEditor.UI.Collection;
using TreeEditor.UI.Input;
using TreeEditor.UI.Proxy;

namespace TreeEditor
{
    public class MainViewModel
    {

        public MainViewModel()
        {
            List<UICollectionPoint> cps = resourceMgr.GetAllItems<UICollectionPoint>();
            foreach (UICollectionPoint c in cps)
            {
                collectionPointList.Add(c);
            }
            List<UIClient> cs = resourceMgr.GetAllItems<UIClient>();
            foreach(UIClient c in cs)
            {
                clientList.Add(c);
            }
        }

        #region members

        private ResourceManager resourceMgr = ResourceManager.Instance;
        private UICollectionPoint selectedCollectionPoint;

        #endregion

        #region Properties

        public UICollectionPoint SelectedCollectionPoint
        {
            get 
            { 
                return selectedCollectionPoint; 
            }
            set 
            { 
                selectedCollectionPoint = value;
                DeleteCommand.SetCanExecute(value != null);
                LockCommand.SetCanExecute(value != null);
            }
        }

        #endregion

        #region Collections

        private ObservableCollectionEx<UICollectionPoint> collectionPointList = new ObservableCollectionEx<UICollectionPoint>();
        public ObservableCollectionEx<UICollectionPoint> CollectionPointList
        {
            get { return collectionPointList; }
        }

        private ObservableCollectionEx<UIClient> clientList = new ObservableCollectionEx<UIClient>();

        public ObservableCollectionEx<UIClient> ClientList
        {
            get { return clientList; }
        }


        #endregion

        #region Commands
        private Command deleteCommand;
        public Command DeleteCommand
        {
            get { return deleteCommand ?? (deleteCommand = new Command(DeleteCollectionPointFromRAM)); }
        }

        private void DeleteCollectionPointFromRAM()
        {
            if(selectedCollectionPoint != null)
            {
                selectedCollectionPoint.Deleted = true;
            }
        }

        private Command lockCommand;
        public Command LockCommand
        {
            get { return lockCommand ?? (lockCommand = new Command(TryLockOnServer));  }
        }

        private void TryLockOnServer()
        {
            Console.WriteLine("TryLockOnServer - implement me.");
            // TODO: fix me
            //resourceMgr.RequestLock(selectedCollectionPoint.Id, ItemType.CollectionPoint);
        }
        #endregion
    }
}
