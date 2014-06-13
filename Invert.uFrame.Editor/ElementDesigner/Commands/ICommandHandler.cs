using System.Collections.Generic;
using Invert.uFrame.Editor.ElementDesigner;

namespace Invert.uFrame.Editor
{
    public interface ICommandHandler
    {
        IEnumerable<object> ContextObjects { get; }
        void CommandExecuted(IEditorCommand command);
        void CommandExecuting(IEditorCommand command);

    }
}