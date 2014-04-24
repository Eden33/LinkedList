using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Graph.Generate;
using Graph.Model;
using Graph.Util;


namespace TreeEditor
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            GenerateRandomBinaryTree(10);
        }

        #region Collections

        private ObservableCollection<UIJunctionPoint> junctions = new ObservableCollection<UIJunctionPoint>();
        public ObservableCollection<UIJunctionPoint> Junctions
        {
            get { return junctions; }
        }

        #endregion

        #region Commands
        private Command genRandBinaryTreeCommand;
        public Command GenRandBinaryTreeCommand
        {
            get { return genRandBinaryTreeCommand ?? (genRandBinaryTreeCommand = new Command(GenerateRandomBinaryTreeCmd)); }
        }

        private void GenerateRandomBinaryTreeCmd()
        {
            GenerateRandomBinaryTree(10);
        }
        #endregion

        #region tree generation
        private void GenerateRandomBinaryTree(int numPaths)
        {
            junctions.Clear();

            //generate tree
            TreeGenerator gen = new TreeGenerator();
            gen.GenerateRandomBinaryTree(numPaths);

            //generate coordinates
            TreeCoordinateBuilder coordB = new TreeCoordinateBuilder();
            coordB.BuildCoordinatesWithKnuth(gen.RootJunction, 30);

            //add the tree to the UI
            gen.Junctions.ForEach(delegate(JunctionPoint j)
            {
                junctions.Add(new UIJunctionPoint(j));
            });
        }
        #endregion
    }
}
