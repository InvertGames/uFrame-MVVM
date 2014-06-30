using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using UnityEditor;
using UnityEngine;

public class ConvertToJSON : EditorCommand<IElementDesignerData>, IToolbarCommand
{
    public override void Perform(IElementDesignerData node)
    {
        var originalPath = AssetDatabase.GetAssetPath(node as UnityEngine.Object);
        if (!EditorUtility.DisplayDialog("Upgrade File Format",
            "Your file will be upgraded.  This file will be saved with the extenesion .backup.", "OK", "Cancel"))
        {
            
            return;
        }
        var result = AssetDatabase.RenameAsset(originalPath, node.Name + ".Backup");
        if (!string.IsNullOrEmpty(result))
        {
            Debug.LogError(result);
        }
        var json = JsonElementDesignerData.Serialize(node).ToString();
        var elementDesignerData = 
            ScriptableObject.CreateInstance<JsonElementDesignerData>();

        elementDesignerData._jsonData = json;
        elementDesignerData.OnAfterDeserialize();
        var assetPath = originalPath;//.Replace(".asset", "JSON.asset");
        
        AssetDatabase.CreateAsset(elementDesignerData, assetPath);
        AssetDatabase.SaveAssets();
        Selection.activeObject = elementDesignerData;
        EditorWindow.GetWindow<ElementsDesigner>().LoadDiagramByName(assetPath);
    }

    public override string CanPerform(IElementDesignerData node)
    {
        if (node == null) return "Problem with node";
        if (node is ElementDesignerData) return null;
        return "Not applicable for this type.";
    }

    public override decimal Order
    {
        get { return -1; }
    }

    public ToolbarPosition Position
    {
        get
        {
            return ToolbarPosition.Left;
        }
    }
}