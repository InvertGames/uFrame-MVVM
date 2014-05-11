using System;
using System.Collections.Generic;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public interface IDiagramCommand
    {
        Type For { get; }
        void Execute(object item);
        List<CommandHook> Hooks { get; }
        string Name { get; }
    }

    public class CommandHook
    {
        public CommandHookLifetime Lifetime { get; set; }
        public Action Action { get; set; }
    }

    public enum CommandHookLifetime
    {
        NextExecute,
        Everytime
    }
}