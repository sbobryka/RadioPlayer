using System;
using System.Collections.Generic;
using System.Text;

namespace RadioPlayer.Commands
{
    internal class RelayCommand : CommandBase
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => execute(parameter);
    }
}
