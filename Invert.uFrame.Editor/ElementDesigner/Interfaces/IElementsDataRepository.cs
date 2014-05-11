using System;
using System.Collections.Generic;
using Invert.uFrame.Editor.ElementDesigner;
using UnityEditor;

public interface 
    IElementsDataRepository
{
    string GetContainerCustomFileFullPath(string name);
    string GetContainerCustomFilename(string name);
    string GetViewCustomFileFullPath(string name);
    string GetViewComponentCustomFileFullPath(string name);
    string GetViewCustomFilename(string name);
    string GetViewComponentCustomFilename(string name);
    string GetControllerCustomFileFullPath(string name);
    string GetControllerCustomFilename(string name);
    string GetViewModelCustomFileFullPath(string name);
    string GetViewModelCustomFilename(string name);
    ElementDesignerData GetData();
    void Save();
    
    void CreateScene(SceneManagerData sceneManagerData);
    IEnumerable<ElementItemType> GetAvailableTypes(bool b, bool b1 = false);
    IEnumerable<string> GetCustomFilePaths(IDiagramItem selected, bool p1, bool includeRefactorings = false);
    SerializedObject SerializedObject { get; set; }
    string AssetPath { get; }
    bool IsImportOnly(Type item);
    DiagramItem ImportType(Type item);
    
    //void RefactorElementName(ElementData elementData);
    //void RefactorSceneManagerName(SceneManagerData sceneManagerData);
    //void RefactorViewName(ViewData viewData);
    //void RefactorViewComponentName(ViewComponentData viewComponentData);
    
}