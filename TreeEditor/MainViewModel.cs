using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Graph.Generate;
using Graph.Model;


namespace TreeEditor
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            JunctionPoint j_1 = new JunctionPoint(JunctionPoint.ROOT_ID);
            JunctionPoint j_2 = new JunctionPoint(JunctionPoint.ROOT_ID + 1);
            j_1.X = 10;
            j_1.Y = 20;
            j_2.X = 50;
            j_2.Y = 100;
            UIJunctionPoint ui_1 = new UIJunctionPoint(j_1);
            UIJunctionPoint ui_2 = new UIJunctionPoint(j_2);
            junctions.Add(ui_1);
            junctions.Add(ui_2);
        }

        #region Collections

        private ObservableCollection<UIJunctionPoint> junctions = new ObservableCollection<UIJunctionPoint>();
        public ObservableCollection<UIJunctionPoint> Junctions
        {
            get { return junctions; }
        }

        #endregion

        #region Commands
        private Command testCommand;
        public Command TestCommand
        {
            get { return testCommand ?? (testCommand = new Command(Test)); }
        }

        private void Test()
        {
            if(junctions[1].X == 50)
            {
                junctions[1].X = 100;
            }
            else
            {
                junctions[1].X = 50;
            }
        }
        #endregion
    }
}
