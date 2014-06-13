using System;
using System.Collections.Generic;
using Invert.uFrame.Editor.ElementDesigner;
using UnityEditor;

public interface IElementsDataRepository
{
    
    IElementDesignerData LoadDiagram(string path);
    void SaveDiagram(IElementDesignerData data);
    void RecordUndo(IElementDesignerData data, string title);
    void MarkDirty(IElementDesignerData data);
    Dictionary<string, string> GetProjectDiagrams();
    void CreateNewDiagram();
}