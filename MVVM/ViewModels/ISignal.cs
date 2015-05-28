using System;

public interface ISignal
{
    Type SignalType { get; }
    void Publish(object data);
    void Publish();
}