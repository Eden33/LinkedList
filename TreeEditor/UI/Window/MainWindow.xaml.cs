using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TreeEditor.UI.Proxy;

namespace TreeEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel model = null;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = model = new MainViewModel();

            //sort hack
            cpListView.MouseUp += cpListView_MouseUp;
            cpListView.KeyUp += cpListView_KeyUp;
        }

        void cpListView_KeyUp(object sender, KeyEventArgs e)
        {
            customersListViewSortAscending();
        }

        void cpListView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            customersListViewSortAscending();
        }

        private void customersListViewSortAscending()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(customersListView.ItemsSource);
            if (view != null)
            {
                view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            }
        }

        private void cpListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UICollectionPoint s = cpListView.SelectedItem as UICollectionPoint;
            model.SelectedCollectionPoint = s;
        }
    }
}
