using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TreeEditor.Resource
{
    //more generic: http://stackoverflow.com/questions/269073/observablecollection-that-also-monitors-changes-on-the-elements-in-collection
    //thx man
    public class CollectionVatList<UICollectionVat> : ObservableCollection<UICollectionVat> where UICollectionVat : INotifyPropertyChanged
    {
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Unsubscribe(e.OldItems);
            Subscribe(e.NewItems);
            base.OnCollectionChanged(e);
        }

        protected override void ClearItems()
        {
            foreach (UICollectionVat element in this)
                element.PropertyChanged -= ContainedElementChanged;

            base.ClearItems();
        }

        private void Subscribe(IList iList)
        {
            if (iList != null)
            {
                foreach (UICollectionVat element in iList)
                    element.PropertyChanged += ContainedElementChanged;
            }
        }

        private void Unsubscribe(IList iList)
        {
            if (iList != null)
            {
                foreach (UICollectionVat element in iList)
                    element.PropertyChanged -= ContainedElementChanged;
            }
        }

        private void ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Deleted"))
            {
                UICollectionVat removed = (UICollectionVat) sender;
                Remove(removed);
            }
        }
    }
}
