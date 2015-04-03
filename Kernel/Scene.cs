using System.Collections;
using UnityEngine;

public abstract class Scene : MonoBehaviour, IScene
{
    private const string KERNEL_SCENE_NAME = "uFrameKernelScene";

    public string Name { get; set; }

    public void Awake()
    {
        StartCoroutine(InternalAwake());
    }

    public IEnumerator InternalAwake()
    {
        if (!uFrameMVVMKernel.IsKernelLoaded)
        {
            Name = Application.loadedLevelName;
            yield return StartCoroutine(uFrameMVVMKernel.IstantiateSceneAsyncAdditively(KERNEL_SCENE_NAME));
            while (!uFrameMVVMKernel.IsKernelLoaded)
            {
                yield return null;
            }
        }

        yield return StartCoroutine(uFrameMVVMKernel.Instance.SetupScene(this));
    }

}