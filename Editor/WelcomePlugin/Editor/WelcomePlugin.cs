using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.Common;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.Core.GraphDesigner.Unity;
using Invert.IOC;
using Invert.Windows;
using UnityEditor;
using UnityEngine.UI;

public class WelcomePlugin : DiagramPlugin, IWelcomeWindowToolbarItemsQuery {
    private ProjectService _projectService;
    public override bool Required
    {
        get { return true; }
    }
    public string ExamplePackageScenesPath
    {
        get { return "Assets/ExampleProject/Scenes/"; }
    }

    public bool ExampleUnityPackageExists
    {
        get { return File.Exists(AbsoluteExampleUnityPackagePath); }
    }

    public string AbsoluteExampleUnityPackagePath
    {
        get { return Path.Combine(Application.dataPath, "uFrame/MVVM/Framework/ExampleProject.unitypackage"); }
    }

    public IEnumerable<string> GetExampleProjectScenes()
    {
        yield return ExamplePackageScenesPath + "Level1.unity";
        yield return ExamplePackageScenesPath + "Level2.unity";
        yield return ExamplePackageScenesPath + "Level3.unity";
        yield return ExamplePackageScenesPath + "MainMenuScene.unity";
        yield return ExamplePackageScenesPath + "ExampleProjectKernelScene.unity";
        yield return ExamplePackageScenesPath + "IntroScene.unity";
        yield return ExamplePackageScenesPath + "AssetsLoadingScene.unity";
    }

    public bool ExampleProjectExists
    {
        get { return ProjectService.Projects.Any(p => p.Name == "ExampleProject"); }
    }

    public bool ExampleProjectNeedInstall
    {
        get { return EditorPrefs.GetBool("UF_EXAMPLE_PROJECT_NEEDS_INSTALL"); }
        set { EditorPrefs.SetBool("UF_EXAMPLE_PROJECT_NEEDS_INSTALL", value); }
    }

    public ProjectService ProjectService
    {
        get { return _projectService ?? (_projectService = InvertApplication.Container.Resolve<ProjectService>()); }
    }



    public override void Initialize(UFrameContainer container)
    {
        container.RegisterWindow<WelcomeWindowViewModel>("WelcomeWindowViewModel")
            .HasPanel<WelcomeWindowSplashScreenDrawer, WelcomeWindowViewModel>(new AreaLayout(0, 0, 16, 14))
            .HasPanel<WelcomeWindowToolbarDrawer, WelcomeWindowViewModel>(new AreaLayout(0, 14, 16, 2))
            .WithDefaultInstance(CreateWelcomeWindow);

        ListenFor<IWelcomeWindowToolbarItemsQuery>();
    }

    public override void Loaded(UFrameContainer container)
    {
        base.Loaded(container);

        if (ExampleProjectNeedInstall)
        {
            ExampleProjectNeedInstall = false;
            var scenes = EditorBuildSettings.scenes;
            EditorBuildSettings.scenes =
                scenes.Concat(GetExampleProjectScenes().Select(s => new EditorBuildSettingsScene(s, true)))
                    .Distinct()
                    .ToArray();

            EditorApplication.OpenScene(ExamplePackageScenesPath + "IntroScene.unity");
            
            if(ElementsDesigner.Instance)
            ElementsDesigner.Instance.Close();

            EditorApplication.isPlaying = true;


        }

    }

    [MenuItem("uFrame/Welcome Screen")]
    public static void ShowWelcomeWindow()
    {
        var window = WindowsPlugin.GetWindowFor("WelcomeWindowViewModel");
        window.title= "Welcome";
        window.minSize = new Vector2(800,410);
        window.maxSize = new Vector2(800,410);
        window.Show();
        window.Focus();
    }

    private WelcomeWindowViewModel CreateWelcomeWindow(string persistedData)
    {
        return new WelcomeWindowViewModel();
    }

    public void QueryForItems(List<WelcomeWindowToolbarItem> items)
    {
        items.Add(new WelcomeWindowToolbarItem()
        {
            ActionTitle = "uFrame",
            Action = () => Application.OpenURL("http://invertgamestudios.com"),
            Header = ElementDesignerStyles.GetSkinTexture("splashscreen_main"),
            ActionArea = new Rect(3, 3, 792, 359)
        });

        items.Add(new WelcomeWindowToolbarItem()
        {
            ActionTitle = "Graph Designer",
            Action = () => ElementsDesigner.Init(),
            Header = ElementDesignerStyles.GetSkinTexture("splashscreen_1"),
            ActionArea = new Rect(4, 290, 199, 65)
        });

        items.Add(new WelcomeWindowToolbarItem()
        {
            ActionTitle = "Documentation",
            Action = ()=>uFrameHelp.ShowWindow(),
            Header = ElementDesignerStyles.GetSkinTexture("splashscreen_3"),
            ActionArea = new Rect(12,293,315,63)
        });

        items.Add(new WelcomeWindowToolbarItem()
        {
            ActionTitle = "Wiki",
            Action = () => Application.OpenURL("https://www.penflip.com/bartlomiejwolk/uframe-documentation"),
            Header = ElementDesignerStyles.GetSkinTexture("splashscreen_5"),
            ActionArea = new Rect(592, 281, 191, 76)
        });

        items.Add(new WelcomeWindowToolbarItem()
        {
            ActionTitle = "Community",
            Action = () => Application.OpenURL("http://invertgamestudios.com/members/slackinvite"),
            Header = ElementDesignerStyles.GetSkinTexture("splashscreen_4"),
            ActionArea = new Rect(453,295,322,61)
        });

        items.Add(new WelcomeWindowToolbarItem()
        {
            ActionTitle = "Examples",
            Action = () => SetupDefaultProject(),
            Header = ElementDesignerStyles.GetSkinTexture("splashscreen_2"),
            ActionArea = new Rect(459,294,325,63)
        });      
        

    }

    private void SetupDefaultProject()
    {


        if (!ExampleUnityPackageExists)
        {
            EditorUtility.DisplayDialog("Example project package not found!",
                string.Format("uFrame is unable to locate {0}. Have you removed it?", AbsoluteExampleUnityPackagePath), "Ok");
            return;
        }

        if (
            !EditorUtility.DisplayDialog("Example project setup",
                "Example Project is about to be set up! It will be imported into Assets/ExampleProject folder.",
                "Continue", "Cancel"))
        {
            return;
        }

        ExampleProjectNeedInstall = true;
        AssetDatabase.ImportPackage(AbsoluteExampleUnityPackagePath,false);
        ElementsDesigner.Init();

    }

    

}

public interface IWelcomeWindowToolbarItemsQuery
{
    void QueryForItems(List<WelcomeWindowToolbarItem> items);
}


public class WelcomeWindowToolbarItem
{
    public Texture2D Header { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ActionTitle { get; set; }
    public Action Action { get; set; }
    public Rect ActionArea { get; set; }
    public bool ActionClosesWelcomeWindow { get; set; }
}

public class WelcomeWindowViewModel : IWindow
{
    private List<WelcomeWindowToolbarItem> _toolbarItems;

    public WelcomeWindowViewModel()
    {
        UpdateToolbarItems();
        CurrentHoweredItem = ToolbarItems.First();
    }

    public string Identifier { get; set; }
    public Texture2D DefaultHeader { get; set; }
    public string DefaultTitle { get; set; }
    public string DefaultDescription { get; set; }
    public Rect DefaultActionArea { get; set; }
    public Action DefaultAction { get; set; }
    public WelcomeWindowToolbarItem CurrentHoweredItem { get; set; }


    public Rect ActionArea
    {
        get { return CurrentHoweredItem == null ? DefaultActionArea : CurrentHoweredItem.ActionArea; }       
    } 
    
    public Action Action
    {
        get { return CurrentHoweredItem == null ? DefaultAction : CurrentHoweredItem.Action; }       
    }

    public bool ActionClosesWelcomeWindow
    {
        get { return CurrentHoweredItem != null && CurrentHoweredItem.ActionClosesWelcomeWindow; }       
    }

    public string Title
    {
        get { return CurrentHoweredItem == null ? DefaultTitle : CurrentHoweredItem.Title; }
    }

    public string Description
    {
        get { return CurrentHoweredItem == null ? DefaultDescription : CurrentHoweredItem.Description;  }
    }

    public Texture2D Header
    {
        get { return CurrentHoweredItem == null ? DefaultHeader : CurrentHoweredItem.Header; }
    }

    public List<WelcomeWindowToolbarItem> ToolbarItems
    {
        get { return _toolbarItems ?? (_toolbarItems = new List<WelcomeWindowToolbarItem>()); }
        set { _toolbarItems = value; }
    }

    public void UpdateToolbarItems()
    {
        ToolbarItems.Clear();
        var newItems = new List<WelcomeWindowToolbarItem>();
        InvertApplication.SignalEvent<IWelcomeWindowToolbarItemsQuery>(i=>i.QueryForItems(newItems));
        ToolbarItems.AddRange(newItems);
    }

    public void ItemHowered(WelcomeWindowToolbarItem item)
    {
        CurrentHoweredItem = item;
    }   
    
    public void ItemLeft(WelcomeWindowToolbarItem item)
    {
        CurrentHoweredItem = null;
    }

    public void ItemClicked(WelcomeWindowToolbarItem item)
    {
        CurrentHoweredItem = item;
        //item.Execute();
    }

}

public class WelcomeWindowSplashScreenDrawer : Area<WelcomeWindowViewModel>
{
    public override void Draw(WelcomeWindowViewModel data)
    {
        
        if (data.Header != null)
        {
            GUI.Box(new Rect(0,0,Screen.width,Screen.height),data.Header);
            EditorGUIUtility.AddCursorRect(data.ActionArea, MouseCursor.Link);
            if(GUI.Button(data.ActionArea,GUIContent.none,GUIStyle.none))
            {
                if (data.Action != null)
                {
                    data.Action();

                    if(data.ActionClosesWelcomeWindow)
                    InvertApplication.SignalEvent<IWindowsEvents>(x =>
                    {
                        x.WindowRequestCloseWithArea(this);
                    });
                }
            }
        }
    }
}

public class WelcomeWindowToolbarDrawer : Area<WelcomeWindowViewModel>
{
    private static GUIStyle _buttonStyle;

    public override void Draw(WelcomeWindowViewModel data)
    {
        GUILayout.BeginHorizontal();

        foreach (var item in data.ToolbarItems)
        {
            if (GUILayout.Button(item.ActionTitle, ButtonStyle))
            {
                data.ItemClicked(item);
            }
        }
        GUILayout.EndHorizontal();
    }


    public static GUIStyle ButtonStyle
    {
        get
        {
            if (_buttonStyle == null)
            {
                var textColor = new Color(0.776f, 0.851f, 0.941f);
                _buttonStyle = new GUIStyle
                {
                    normal =
                    {
                        background = ElementDesignerStyles.GetSkinTexture("WelcomeScreenButton"),
                        textColor = textColor
                    },
                    focused =
                    {
                        background = ElementDesignerStyles.GetSkinTexture("WelcomeScreenButton"),
                        textColor = textColor
                    },
                    active = { background = ElementDesignerStyles.GetSkinTexture("EventButton"), textColor = textColor },
                    hover = { background = ElementDesignerStyles.GetSkinTexture("WelcomeScreenButtonHowered"), textColor = textColor },
                    onHover =
                    {
                        background = ElementDesignerStyles.GetSkinTexture("WelcomeScreenButtonHowered"),
                        textColor = textColor
                    },
                    onFocused = { background = ElementDesignerStyles.GetSkinTexture("EventButton"), textColor = textColor }, 
                    onNormal = { background = ElementDesignerStyles.GetSkinTexture("EventButton"), textColor = textColor },
                    onActive = { background = ElementDesignerStyles.GetSkinTexture("EventButton"), textColor = textColor },
                    fixedHeight = 50,
                    fixedWidth = 133,
                    alignment = TextAnchor.MiddleCenter,
                    border = new RectOffset(10,10,10,10),
                }.WithFont("Impact",18);
            }
            return _buttonStyle;
        }
    }

}





