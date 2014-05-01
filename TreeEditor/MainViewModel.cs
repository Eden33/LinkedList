using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TreeEditor.Resource;

namespace TreeEditor
{
    public class MainViewModel
    {

        public MainViewModel()
        {
            for (int i = 1; i < 2; i++)
            {
                //vatList1.Add(resourceMgr.getCollectionVat(i));
                //vatList2.Add(resourceMgr.getCollectionVat(i));
                //resourceMgr.getCollectionVat(i).X = i * 30;
                //resourceMgr.getCollectionVat(i).Y = i * 30;
            }
        }

        #region members

        private ResourceManager resourceMgr = ResourceManager.Instance;

        #endregion

        #region Properties

        private UICollectionVat selectedVat;
        public UICollectionVat SelectedVat
        {
            get 
            { 
                return selectedVat; 
            }
            set 
            { 
                selectedVat = value;
                DeleteCommand.SetCanExecute(value != null);
            }
        }

        #endregion

        #region Collections

        private CollectionVatList<UICollectionVat> vatList1 = new CollectionVatList<UICollectionVat>();
        public CollectionVatList<UICollectionVat> VatList1
        {
            get { return vatList1; }
        }

        private CollectionVatList<UICollectionVat> vatList2 = new CollectionVatList<UICollectionVat>();
        public CollectionVatList<UICollectionVat> VatList2
        {
            get { return vatList2;  }
        }

        #endregion

        #region Commands
        private Command deleteCommand;
        public Command DeleteCommand
        {
            get { return deleteCommand ?? (deleteCommand = new Command(DeleteVat)); }
        }

        private void DeleteVat()
        {
            if(selectedVat != null)
            {
                selectedVat.Deleted = true;
            }
        }
        #endregion
    }
}
