using System;
using System.Collections;
using UnityEngine;

public abstract class SceneLoader<T> : MonoBehaviour, ISceneLoader where T : IScene
{
    public Type SceneType
    {
        get { return typeof (T); }
    }

    protected abstract IEnumerator LoadScene(T scene);
    protected abstract IEnumerator UnloadScene(T scene);

    public IEnumerator Load(object sceneObject)
    {
        return LoadScene((T)sceneObject);
    }

    public IEnumerator Unload(object sceneObject)
    {
        return UnloadScene((T) sceneObject);
    }
}