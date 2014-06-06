using System;
using System.Collections.Generic;
using Invert.uFrame.Editor.ElementDesigner;
using UnityEditor;

public interface IElementsDataRepository
{

    ElementDesignerData GetData();
    void Save();
    
    void CreateScene(SceneManagerData sceneManagerData);
    IEnumerable<ElementItemType> GetAvailableTypes(bool b, bool b1 = false);
    SerializedObject SerializedObject { get; set; }
    string AssetPath { get; }
    bool IsImportOnly(Type item);
    DiagramItem ImportType(Type item);
    
    void NavigateToView(ViewData data);
}