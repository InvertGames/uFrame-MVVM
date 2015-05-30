using System;
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

public partial class SystemLoader : MonoBehaviour,ISystemLoader
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

