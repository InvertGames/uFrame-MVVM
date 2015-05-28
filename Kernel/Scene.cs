using System.Collections;
using UnityEngine;
using UniRx;
public class Scene : uFrameComponent, IScene
{
    [SerializeField]
    private string _KernelScene;


    protected string KernelScene
    {
        get
        {
            if (string.IsNullOrEmpty(_KernelScene))
            {
                return DefaultKernelScene;
            }
            return _KernelScene;
        }
    }

    public virtual string DefaultKernelScene { get; set; }
    public string Name { get; set; }

    public ISceneSettings _SettingsObject { get; set; }

    public IEventAggregator Aggregator { get; set; }

    public void Awake()
    {
        StartCoroutine(InternalAwake());
    }

    protected override IEnumerator Start()
    {
        if (!uFrameMVVMKernel.IsKernelLoaded)
        {
            Name = Application.loadedLevelName;
            yield return StartCoroutine(uFrameMVVMKernel.InstantiateSceneAsyncAdditively(KernelScene));
        }
        while (!uFrameMVVMKernel.IsKernelLoaded) yield return null;
        uFrameMVVMKernel.EventAggregator.Publish(new SceneAwakeEvent() { Scene = this });
    }


    public IEnumerator InternalAwake()
    {
  
        while (!uFrameMVVMKernel.IsKernelLoaded)
        {
            yield return null;
        }
       
        
    }

}

public class SceneAwakeEvent
{
    public IScene Scene { get; set; }
}