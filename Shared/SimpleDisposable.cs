using System;

public class SimpleDisposable : IDisposable
{
    public Action DisposeAction;

    public SimpleDisposable(Action disposeAction)
    {
        DisposeAction = disposeAction;
    }

    public void Dispose()
    {
        if (DisposeAction != null)
            DisposeAction();
    }
}