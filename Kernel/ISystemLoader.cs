using UnityEngine;
using System.Collections;

public interface ISystemLoader
{

    void Load();

    IGameContainer Container
    {
        get;
        set;
    }

    IEventAggregator EventAggregator
    {
        get;
        set;
    }

}

public class SystemLoader
{
    public virtual void Load()
    {
        
    }

    public IGameContainer Container
    {
        get;
        set;
    }

    public IEventAggregator EventAggregator
    {
        get;
        set;
    }

}

