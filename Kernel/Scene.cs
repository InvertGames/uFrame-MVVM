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
        if (!uFrameKernel.IsKernelLoaded)
        {
            Name = Application.loadedLevelName;
            yield return StartCoroutine(uFrameKernel.LoadSceneAsyncAdditively(KERNEL_SCENE_NAME));
            while (!uFrameKernel.IsKernelLoaded)
            {
                yield return null;
            }
        }

        yield return StartCoroutine(uFrameKernel.Instance.SetupScene(this));
    }

}