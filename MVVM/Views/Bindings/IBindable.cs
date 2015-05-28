using System;

public interface IBindable
{
    IDisposable AddBinding(IDisposable binding);
}