using System;
using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using Invert.uFrame.Editor;
using uFrame.Kernel;
using uFrame.MVVM;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[InitializeOnLoad]
public class uFrameEditorSceneManager
{
    private static int managerId;

 
    private static List<int> markedViews = new List<int>();
    
 
    public static ViewBase[] SceneViews { get; set; }

    public static Type CurrentFocusType { get; set; }

    public static IEnumerable<ViewBase> FocusableViews
    {
        get
        {
            if (CurrentFocusType != null)
            foreach (var sceneView in SceneViews)
            {
                if (CurrentFocusType.IsAssignableFrom(sceneView.GetType()))
                {
                    yield return sceneView;
                }
            }
        }
    }

    public static void NavigateToFirstView(Type type)
    {
        if (SceneViews == null) return;
        
        var first = SceneViews.FirstOrDefault(p => p.GetType() == type);
        if (first != null)
        {
            NavigateToView(first);
        }
    }

    public static void NavigateToView(ViewBase view)
    {
        if (view == null) return;
        CurrentFocusType = view.GetType();
        var scenViewWindow = EditorWindow.GetWindow<SceneView>();
        var sceneView= SceneView.lastActiveSceneView ?? scenViewWindow;
        scenViewWindow.Show(true);
        Selection.activeGameObject = view.gameObject;
        
        //var localScale = (view.transform.localScale*2f);
        sceneView.pivot = view.transform.position;// - new Vector3(localScale.x,2f,localScale.z);
        sceneView.LookAt(view.transform.position,Quaternion.Euler(0f,0f,0f));
        //sceneView.AlignViewToObject(view.transform);
        CurrentView = view;
       

    }

    public static int CurrentViewIndex { get; set; }

    public static ViewBase CurrentView { get; set; }

    private static void RefreshSceneObjects()
    {
//        var gcm = (GameManager)Object.FindObjectOfType(typeof(GameManager));
        var objs = Object.FindObjectsOfType(typeof(uFrameComponent));

        //if (gcm != null)
        //    managerId = gcm.gameObject.GetInstanceID();

        SceneViews = objs.OfType<ViewBase>().ToArray();
        markedViews = SceneViews.Select(p => p.gameObject.GetInstanceID()).ToList();
        
      
    }

    #region Editor Icons

    private static Texture2D textureViewSI;
    private static Texture2D textureViewMI;

    static uFrameEditorSceneManager()
    {
        // Init
        //SetSearchFilter(string searchFilter, SearchableEditorWindow.SearchMode searchMode, bool setAll)
        
        //texture = Resources.Load("Controller.png") as Texture2D;
        //AssetDatabase.LoadAssetAtPath("Assets/Images/Testicon.png", typeof(Texture2D)) as Texture2D;
     
        textureViewSI = ElementDesignerStyles.GetSkinTexture("ViewSingleInstance");
        textureViewMI = ElementDesignerStyles.GetSkinTexture("ViewMultiInstance");
        EditorApplication.update += RefreshSceneObjects;
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        
    }

    private static void HierarchyItemCB(int instanceID, Rect selectionRect)
    {

        if (selectionRect.width < 265)
        {
            return;
        }

        // place the icoon to the right of the list:
        Rect r = new Rect(selectionRect);
        r.x = r.width - 80;
        r.width = 75;
        r.height = 16;

        if (markedViews.Contains(instanceID))
        {
            if (textureViewMI != null && textureViewSI != null)
            {
                //GUI.DrawTexture(r, textureView);
                try
                {
                    var viewBase = SceneViews.FirstOrDefault(p => p.gameObject.GetInstanceID() == instanceID);
                    if (viewBase != null)
                    {
                        GUI.DrawTexture(r, textureViewSI);
                    }
                }
                catch (Exception ex)
                {
                    Supress(ex);
                }
            }

            // Draw the texture if it's a light (e.g.)
            //GUI.Label(r, textureView);
        }
    }
    public static void Supress(Exception ex) { }

    #endregion

    public static void NavigateBack(ViewBase view)
    {
        //var designerWindow = EditorWindow.GetWindow<ElementsDesigner>();
        //var attribute = view.GetType().GetCustomAttributes(typeof(DiagramInfoAttribute), true).FirstOrDefault() as DiagramInfoAttribute;

        //if (attribute == null) return;
      //  designerWindow.LoadDiagramByName(attribute.DiagramName);
        //designerWindow.Diagram.Data.PushFilter();
    }

    public static void NavigatePrevious(ViewBase view)
    {
        
    }

    public static void NavigateNext(ViewBase view)
    {
        
    }
}