using System.Collections;
using UnityEngine;
using UniRx;
public class Scene : MonoBehaviour, IScene
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

    public IEnumerator InternalAwake()
    {
        if (!uFrameMVVMKernel.IsKernelLoaded)
        {
            Name = Application.loadedLevelName;
            yield return StartCoroutine(uFrameMVVMKernel.InstantiateSceneAsyncAdditively(KernelScene));
        } 
        while (!uFrameMVVMKernel.IsKernelLoaded)
        {
            yield return null;
        }
        uFrameMVVMKernel.EventAggregator.Publish(new SceneAwakeEvent() { Scene = this });
        
    }

}

public class SceneAwakeEvent
{
    public IScene Scene { get; set; }
}