using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.IOC;
using Invert.StateMachine;
using Invert.uFrame.Editor;
using Invert.uFrame.MVVM;
using uFrame.MVVM;
using UniRx;
using UnityEditor;
using UnityEngine;
using uFrame.MVVM;
using ViewModel = uFrame.MVVM.ViewModel;

[InitializeOnLoad]
public class UFrameEditorPlugin : DiagramPlugin
{
    public override decimal LoadPriority
    {
        get { return -1; }
    }

    public override bool Required
    {
        get { return true; }
    }

    static UFrameEditorPlugin()
    {
        InvertApplication.CachedAssemblies.Add(typeof(ViewModel).Assembly);
        InvertApplication.TypeAssemblies.Add(typeof(ViewModel).Assembly);
        InvertApplication.CachedAssemblies.Add(typeof(UFrameEditorPlugin).Assembly);
    }

    public override void Initialize(UFrameContainer container)
    {
        container.RegisterInstance<IDiagramNodeCommand>(new CreateSceneCommand(), "CreateScene");
        container.RegisterInstance<IDiagramNodeCommand>(new AddManagerToSceneCommand(), "AddToScene");
        container.RegisterInstance<IDiagramNodeCommand>(new AddManagerToSceneSelectionCommand(), "AddToSceneSelection");
        container.RegisterInstance<IDiagramNodeCommand>(new AddViewToSceneCommand(), "AddViewToScene");
        //container.RegisterInstance<IDiagramNodeCommand>(new AddViewToSceneSelectionCommand(), "AddViewToSceneSelection");
        container.RegisterInstance<IToolbarCommand>(new ScaffoldOrUpdateKernelCommand(),"ScaffoldOrUpdateKernel");
    }

    //public class uFrameTypeProvider : IUFrameTypeProvider
    //{

    //    public Type ViewModel
    //    {
    //        get { return typeof(ViewModel); }
    //    }

    //    public Type Controller
    //    {
    //        get { return typeof(Controller); }
    //    }
    //    [Obsolete]
    //    public Type SceneManager
    //    {
    //        get { return typeof(SceneManager); }
    //    }
    //    [Obsolete]
    //    public Type GameManager
    //    {
    //        get { return typeof(GameManager); }
    //    }

    //    public Type ViewComponent
    //    {
    //        get { return typeof(ViewComponent); }
    //    }

    //    public Type ViewBase
    //    {
    //        get { return typeof(ViewBase); }
    //    }

    //    public Type UFToggleGroup
    //    {
    //        get { return typeof(UFToggleGroup); }
    //    }

    //    public Type UFGroup
    //    {
    //        get { return typeof(UFGroup); }
    //    }

    //    public Type UFRequireInstanceMethod
    //    {
    //        get { return typeof(UFRequireInstanceMethod); }
    //    }

    //    public Type DiagramInfoAttribute
    //    {
    //        get { return typeof(DiagramInfoAttribute); }
    //    }

    //    public Type GetModelCollectionType<T>()
    //    {
    //        return typeof(ModelCollection<T>);
    //    }
    //    [Obsolete]
    //    public Type UpdateProgressDelegate
    //    {
    //        get { return typeof(UpdateProgressDelegate); }
    //    }
    //    [Obsolete]
    //    public Type CommandWithSenderT
    //    {
    //        get { return typeof(ViewModelCommand); }
    //    }

    //    public Type CommandWith
    //    {
    //        get { return null; }
    //    }
    //    [Obsolete]
    //    public Type CommandWithSenderAndArgument
    //    {
    //        get { return typeof(ViewModelCommand); }
    //    }

    //    //public Type YieldCommandWithSenderT
    //    //{
    //    //    get { return typeof (YieldCommandWithSender<>); }
    //    //}

    //    //public Type YieldCommandWith
    //    //{
    //    //    get { return typeof (YieldCommandWith<>); }
    //    //}

    //    //public Type YieldCommandWithSenderAndArgument
    //    //{
    //    //    get { return typeof (YieldCommandWithSenderAndArgument<,>); }
    //    //}

    //    //public Type YieldCommand
    //    //{
    //    //    get { return typeof (YieldCommand); }
    //    //}
    //    [Obsolete]
    //    public Type Command
    //    {
    //        get { return typeof(ViewModelCommand); }
    //    }
    //    [Obsolete]
    //    public Type ICommand
    //    {
    //        get { return typeof(ICommand); }
    //    }

    //    public Type ListOfViewModel
    //    {
    //        get { return typeof(List<ViewModel>); }
    //    }

    //    public Type ISerializerStream
    //    {
    //        get { return typeof(ISerializerStream); }
    //    }

    //    public Type P
    //    {
    //        get { return typeof(P<>); }
    //    }

    //    public Type ModelCollection
    //    {
    //        get { return typeof(ModelCollection<>); }
    //    }

    //    public Type Computed
    //    {
    //        get { throw new Exception(); }
    //    }

    //    public Type State
    //    {
    //        get { return typeof(State); }
    //    }

    //    public Type StateMachine
    //    {
    //        get { return typeof(StateMachine); }
    //    }

    //    public Type IObservable
    //    {
    //        get { return typeof(IObservable<>); }
    //    }


    //    public override string ToString()
    //    {
    //        var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
    //        var sb = new StringBuilder();
    //        sb.AppendFormat("public class uFrameStringTypeProvider : IUFrameTypeProvider {{");
    //        sb.AppendLine();
    //        foreach (var property in properties)
    //        {
    //            if (property.PropertyType != typeof(Type))
    //                continue;
    //            var type = property.GetValue(this, null) as Type;
    //            if (type == null) continue;
    //            sb.AppendFormat("\tprivate Type _{0};", property.Name);
    //            sb.AppendLine();
    //            sb.AppendFormat("\tpublic Type {0} {{ get {{ return _{0} ?? (_{0} = uFrameEditor.FindType(\"{1}\")); }} }}", property.Name,
    //                type.FullName);
    //            sb.AppendLine();
    //        }
    //        sb.Append("}");
    //        sb.AppendLine();
    //        return sb.ToString();
    //    }
    //}

}

