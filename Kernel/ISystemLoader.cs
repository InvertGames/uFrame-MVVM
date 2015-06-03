using System;
using UnityEngine;
using System.Collections;
using uFrame.IOC;
using uFrame.Kernel;

namespace uFrame.Kernel
{
    public interface ISystemLoader
    {

        void Load();

        IUFrameContainer Container { get; set; }

        IEventAggregator EventAggregator { get; set; }

    }

    public partial class SystemLoader : MonoBehaviour, ISystemLoader
    {
        public virtual void Load()
        {

        }

        public IUFrameContainer Container { get; set; }

        public IEventAggregator EventAggregator { get; set; }
    }

}