using System;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public interface IChildCommand
    {
        Type ChildCommandFor { get; }
    }
}