using System;
using System.Collections;

public interface ISceneLoader
{
    Type SceneType { get; }
    IEnumerator Load(object scene, Action<float, string> progressDelegate);
    IEnumerator Unload(object scene, Action<float, string> progressDelegate);
}