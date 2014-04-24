using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TreeEditor
{
    public class Command : ICommand
    {
        private Action action;
        public Action Action 
        {
            get { return action;  }
        }
        private bool canExecute = true;

        public bool CanExecute(object parameter)
        {
            return this.canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (action != null)
                action();
        }

        public Command(Action action)
        {
            this.action = action;
        }
    }
}
