using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class uFrameHierarchyIcons
{
    private static int managerId;

    private static List<int> markedGames = new List<int>();
    private static List<int> markedViews = new List<int>();
    
    private static Texture2D textureGameManager;
    private static Texture2D textureSceneManager;
    private static Texture2D textureView;

    static uFrameHierarchyIcons()
    {
        // Init
    
        //texture = Resources.Load("Controller.png") as Texture2D;
        //AssetDatabase.LoadAssetAtPath("Assets/Images/Testicon.png", typeof(Texture2D)) as Texture2D;
        textureGameManager = UFStyles.GetSkinTexture("GameManager");
        textureSceneManager = UFStyles.GetSkinTexture("SceneManager");
        textureView = UFStyles.GetSkinTexture("View");
        EditorApplication.update += UpdateCB;
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

        if (instanceID == managerId)
        {
            if (textureGameManager != null)
                GUI.DrawTexture(r, textureGameManager);
            //GUI.Label(r, textureGameManager);
        }
        if (markedGames.Contains(instanceID))
        {
            if (textureSceneManager != null)
                GUI.DrawTexture(r, textureSceneManager);
            // Draw the texture if it's a light (e.g.)
            //GUI.Label(r, textureSceneManager);
        }
        if (markedViews.Contains(instanceID))
        {
            if (textureView != null)
                GUI.DrawTexture(r, textureView);
            // Draw the texture if it's a light (e.g.)
            //GUI.Label(r, textureView);
        }
    }

    private static void UpdateCB()
    {

        var gcm = (GameManager)Object.FindObjectOfType(typeof(GameManager));
        var objs = Object.FindObjectsOfType(typeof(ViewModelObserver));

        if (gcm != null)
            managerId = gcm.gameObject.GetInstanceID();
        // Check here

        markedViews = objs.OfType<ViewBase>().Select(p => p.gameObject.GetInstanceID()).ToList();
//        markedControllers = objs.OfType<Controller>().Select(p => p.gameObject.GetInstanceID()).ToList();

        //var gc = Object.FindObjectsOfType(typeof(View)) as View[];
        //if (gc != null)
        markedGames = objs.OfType<SceneManager>().Select(p => p.gameObject.GetInstanceID()).ToList();
    }
}