using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using UnityEditor;
using UnityEngine;

public class JsonRepository : DefaultElementsRepository
{
    public override Type RepositoryFor
    {
        get { return typeof (JsonElementDesignerData); }
    }
    public override void CreateNewDiagram()
    {
        UFrameAssetManager.CreateAsset<JsonElementDesignerData>();
    }
    //public IElementDesignerData LoadDiagram(string path)
    //{
    //    if (string.IsNullOrEmpty(path)) throw new NullReferenceException("Path can't be null.");
    //    var asset = AssetDatabase.LoadAssetAtPath(path, RepositoryFor) as JsonElementDesignerData;
    //    if (asset == null)
    //    {
    //        return null;
    //    }
    //    return asset;
    //}

    //public void SaveDiagram(IElementDesignerData data)
    //{
    //    throw new NotImplementedException();
    //}

    //public void RecordUndo(IElementDesignerData data, string title)
    //{
    //    throw new NotImplementedException();
    //}

    //public void MarkDirty(IElementDesignerData data)
    //{
    //    throw new NotImplementedException();
    //}

    //public Dictionary<string, string> GetProjectDiagrams()
    //{
    //    throw new NotImplementedException();
    //}

    //public void CreateNewDiagram()
    //{
    //    throw new NotImplementedException();
    //}
}