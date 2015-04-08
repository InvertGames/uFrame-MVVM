using System.Collections;
using UnityEngine;
using UniRx;
public class Scene : MonoBehaviour, IScene
{
    private const string KERNEL_SCENE_NAME = "uFrameMVVMKernelScene";

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
            yield return StartCoroutine(uFrameMVVMKernel.InstantiateSceneAsyncAdditively(KERNEL_SCENE_NAME));
        }
        uFrameMVVMKernel.EventAggregator.Publish(new SceneAwakeEvent() { Scene = this });
        
    }

}

public class SceneAwakeEvent
{
    public IScene Scene { get; set; }
}