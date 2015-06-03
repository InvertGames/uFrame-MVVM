using System;

namespace uFrame.MVVM
{


    public partial class Controller
    {
        [Obsolete("Use Publish")]
        public void ExecuteCommand(ICommand command, object argument)
        {
            //CommandDispatcher.ExecuteCommand(command, argument);
        }
        [Obsolete("Use Publish")]
        public virtual void ExecuteCommand(ICommand command)
        {
            //CommandDispatcher.ExecuteCommand(command, null);
        }
        [Obsolete("Use Publish")]
        public void ExecuteCommand<TArgument>(ICommandWith<TArgument> command, TArgument argument)
        {
            //CommandDispatcher.ExecuteCommand(command, argument);
        }
    }
}