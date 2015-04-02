using System;
using System.Collections;

public interface ISceneLoader
{
    Type SceneType { get; }
    IEnumerator Load(object scene);
    IEnumerator Unload(object scene);
}