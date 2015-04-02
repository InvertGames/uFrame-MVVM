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
    protected abstract IEnumerator UnloadScene(T scene, Action<float, string> progressDelegate);

    public IEnumerator Load(object sceneObject, Action<float,string> progressDelegate)
    {
        return LoadScene((T)sceneObject,progressDelegate);
    }

    public IEnumerator Unload(object sceneObject, Action<float, string> progressDelegate)
    {
        return UnloadScene((T) sceneObject,progressDelegate);
    }
}