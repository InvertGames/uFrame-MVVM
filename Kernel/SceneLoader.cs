using System;
using System.Collections;
using UnityEngine;

public abstract class SceneLoader<T> : MonoBehaviour, ISceneLoader where T : IScene 
{
    
    public Type SceneType
    {
        get { return typeof (T); }
    }

    protected abstract IEnumerator LoadScene(T scene, Action<float, string> progressDelegate);
    protected abstract IEnumerator UnloadScene(T scene,  Action<float, string> progressDelegate);

    public IEnumerator Load(object sceneObject, Action<float,string> progressDelegate)
    {
        return LoadScene((T)sceneObject,progressDelegate);
    }

    public IEnumerator Unload(object sceneObject, Action<float, string> progressDelegate)
    {
        return UnloadScene((T)sceneObject,progressDelegate);
    }

}


public class DefaultSceneLoader : SceneLoader<IScene>
{

    public Type SceneType
    {
        get { return typeof(IScene); }
    }

    protected override IEnumerator LoadScene(IScene scene, Action<float, string> progressDelegate)
    {
        yield break;
    }

    protected override IEnumerator UnloadScene(IScene scene,  Action<float, string> progressDelegate)
    {
        yield break;
    }

    public IEnumerator Load(object sceneObject, Action<float, string> progressDelegate)
    {
        return LoadScene((IScene)sceneObject, progressDelegate);
    }

    public IEnumerator Unload(object sceneObject, object settings, Action<float, string> progressDelegate)
    {
        return UnloadScene((IScene)sceneObject, progressDelegate);
    }
}

