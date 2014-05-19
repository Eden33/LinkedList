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
            for (int i = 1; i <= 4; i++)
            {
                // TODO: enable again
                //collectionPoints.Add(resourceMgr.getCollectionPoint(i));
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

        private ObservableCollectionEx<UICollectionPoint> collectionPoints = new ObservableCollectionEx<UICollectionPoint>();
        public ObservableCollectionEx<UICollectionPoint> CollectionPoints
        {
            get { return collectionPoints; }
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
