using System;

namespace uFrame.MVVM.Bindings
{
    public interface IBindable
    {
        IDisposable AddBinding(IDisposable binding);
    }
}