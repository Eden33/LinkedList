using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.UI.Input
{
    public class Command : ICommand
    {
        private Action action;
        public Action Action 
        {
            get { return action;  }
        }
        private bool canExecute = false;

        public bool CanExecute(object parameter)
        {
            return this.canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void SetCanExecute(bool value)
        {
            if(this.canExecute != value)
            {
                this.canExecute = value;

                if(this.CanExecuteChanged != null)
                {
                    this.CanExecuteChanged(this, new EventArgs());
                }
            }
        }

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
