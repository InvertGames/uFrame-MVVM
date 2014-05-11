//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;

//using UnityEngine;

//public interface IViewModelDesignerRepository
//{
//    ClassViewModel GetClasses();
//}

//public class ClassView : EditorView<ClassViewModel>
//{
//    public Rect Rect
//    {
//        get; set;
//    }
//    public override void OnGUI()
//    {
//        base.OnGUI();


//        var top = 25f;
        
//        foreach (var propertyInfo in Model.ViewModelProperties)
//        {
//            GUI.Label(new Rect(4f, top, Rect.width, 30f), propertyInfo.Name);
//            GUI.Label(Rect, propertyInfo.Name);
//            top += 25f;
//        }
//        var style = new GUIStyle(EditorStyles.toolbarButton) { fontStyle = FontStyle.Bold };
//        GUI.Label(new Rect(4f, top, this.Rect.width, 30f), "Commands", style);
//        top += 25f;
//        foreach (var propertyInfo in Model.ViewModelCommands)
//        {
//            GUI.Label(new Rect(4f, top, Rect.width, 30f), propertyInfo.Name);
//            top += 25f;
//        }
//    }
//}
//#region ViewModels 
//public class ClassViewModel : ViewModel
//{
//    public readonly ModelCollection<CommandViewModel> _CommandsProperty = new ModelCollection<CommandViewModel>();

//    public readonly ModelCollection<PropertyViewModel> _PropertiesProperty = new ModelCollection<PropertyViewModel>();

//    public ICollection<PropertyViewModel> ViewModelProperties
//    {
//        get { return _PropertiesProperty; }
//        set { _PropertiesProperty.Value = value.ToList(); }
//    }

//    public ICollection<CommandViewModel> ViewModelCommands
//    {
//        get { return _CommandsProperty; }
//        set { _CommandsProperty.Value = value.ToList(); }
//    }
//}

//public class CommandViewModel : ViewModel
//{
//    public readonly P<string> _Name = new P<string>();

//    public string Name
//    {
//        get { return _Name.Value; }
//        set { _Name.Value = value; }
//    }
//}

//public class ModelDesignerViewModel : ViewModel
//{
//    public readonly ModelCollection<ClassViewModel> _ClassesProperty = new ModelCollection<ClassViewModel>();

//    public ICollection<ClassViewModel> Classes
//    {
//        get { return _ClassesProperty; }
//        set { _ClassesProperty.Value = value.ToList(); }
//    }

//    public ModelDesignerViewModel()
//    {
//        AddClassCommand = new Command(() =>
//        {
//            var classViewModel = new ClassViewModel();
//            classViewModel.ViewModelProperties.Add(new PropertyViewModel()
//            {
//                Name= "Active",
//                TypeName = "int"
//            });
            
//            Classes.Add(classViewModel);
//        });
//    }

//    public ICommand AddClassCommand { get; set; }
//}

//#endregion

//public interface IEditorView
//{
//    object ViewModelObject { get; set; }
//    void AsWindow();

//    void OnGUI();
//}

//public class EditorView<TViewModel> : IEditorView where TViewModel : ViewModel
//{
//    public LinkedList<IEditorView> ChildViews { get; set; }

//    public TViewModel Model { get; set; }

//    public object ViewModelObject { get; set; }

//    public void AsWindow()
//    {

//    }

//    public virtual void OnGUI()
//    {
//    }

//    public virtual void OnWindow(int id)
//    {
//        OnGUI();
//    }
//}

//public abstract class EditorViewWindow : EditorWindow,  IEditorView
//{
//    private object _viewModelObject;

//    public object ViewModelObject
//    {
//        get { return _viewModelObject; }
//        set { _viewModelObject = value;
//            Bind();
//        }
//    }

//    protected abstract void Bind();

//    public virtual void AsWindow()
//    {
        
//    }

//    public virtual void OnGUI()
//    {
        
//    }
//}

//public abstract class EditorViewWindow<TViewModel> : EditorViewWindow where TViewModel : class
//{

//    public TViewModel Model
//    {
//        get
//        {
//            return ViewModelObject as TViewModel;
//        }
//        set
//        {
//            ViewModelObject = value;
//        }
//    }



//    public override void AsWindow()
//    {
        
//    }

//    public override void OnGUI()
//    {
        
//    }
//}

//public interface IModelDesignerRepository
//{
    
//}
//public class ModelDesignerWindow : EditorViewWindow<ModelDesignerViewModel>
//{
//    public Vector2 _ScrollPosition;
//    private Type _currentClassType;
//    private List<IEditorView> _windows = new List<IEditorView>();

//    public Rect DrawingArea
//    {
//        get
//        {
//            return new Rect(0f, 0f, this.position.width, this.position.height);
//        }
//    }
    
//    [MenuItem("Tools/[u]Frame/View Model Designer", false, 1)]
//    public static void Init()
//    {
//        var window = (ModelDesignerWindow)GetWindow(typeof(ModelDesignerWindow));
//        window.title = "View Model Designer";
//        window.Model = new ModelDesignerViewModel()
//        {
            
//        };
//        window.Model.AddClassCommand.Execute();
//        window.Show();
//    }
    
//    public List<IEditorView> Windows
//    {
//        get { return _windows; }
//        set { _windows = value; }
//    }

//    protected override void Bind()
//    {
//        Model._ClassesProperty.ChangedWith += ClassesChanged;
//    }

//    private void ClassesChanged(ModelCollectionChangeEventWith<ClassViewModel> changeArgs)
//    {
//        if (changeArgs.Action == ModelCollectionAction.Add)
//        {
//            foreach (var window in changeArgs.NewItemsOfT)
//            {
//                Windows.Add(new ClassView() { Model= window});
//            }
            
//        } else if (changeArgs.Action == ModelCollectionAction.Remove)
//        {
//            Windows.RemoveAll(p => changeArgs.OldItemsOfT.Contains(p.ViewModelObject));
//        }
//    }



//    public void OnGUI()
//    {
//        var da = DrawingArea;
//        GUILayout.BeginArea(da, "");

//        _ScrollPosition = GUILayout.BeginScrollView(_ScrollPosition, true, true);
//        _currentClassType = typeof(FPSWeaponViewModel);
//        this.BeginWindows();
//        GUI.Window(0, new Rect(0f, 0f, 200f, 200f), new GUI.WindowFunction(ClassWindow), _currentClassType.Name);

//        if (Event.current.type == EventType.DragUpdated)
//        {
//            Debug.Log("Looking good BABY!!!");
//            if (DragAndDrop.objectReferences.First(p => p is MonoScript))
//            {
//                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
//                Debug.Log("YEAH BABY!!!");
//            }
//        }
//        foreach (var window in Windows)
//        {
            
//        }
//        this.EndWindows();
//        GUILayout.EndScrollView();
//        GUILayout.EndArea();
//    }

//    public void RenderClass(Type type)
//    {
//        var properties = ViewModel.GetReflectedModelProperties(type);
//        var commands = ViewModel.GetReflectedCommands(type);

//    }

//    private void ClassWindow(int id)
//    {
//        GUI.DragWindow();
//        RenderClass(_currentClassType);
//    }
//}

//public class PropertyViewModel : ViewModel
//{
//    public readonly P<string> _Name = new P<string>();

//    public readonly P<string> _TypeName = new P<string>();

//    public string Name
//    {
//        get { return _Name.Value; }
//        set { _Name.Value = value; }
//    }

//    public string TypeName
//    {
//        get { return _TypeName.Value; }
//        set { _TypeName.Value = value; }
//    }
//}

