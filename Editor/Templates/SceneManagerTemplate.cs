using System.CodeDom;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using UnityEngine;

[TemplateClass(MemberGeneratorLocation.Both, uFrameFormats.SCENE_MANAGER_FORMAT)]
public partial class SceneManagerTemplate : IClassTemplate<SceneManagerNode>
{
    public string OutputPath
    {
        get { return Path2.Combine(Ctx.Data.Graph.Name, "SceneManagers"); }
    }

    public bool CanGenerate
    {
        get { return true; }
    }

    public void TemplateSetup()
    {
        Ctx.TryAddNamespace("UniRx");
        if (Ctx.IsDesignerFile)
        {
            Ctx.SetBaseType(typeof(SceneManager));
            foreach (var transition in Ctx.Data.SceneTransitions)
            {
                var to = transition.OutputTo<SceneManagerNode>();
                if (to == null) continue;
                Ctx._("public {0} {1}Transition = new {0}();", to.Name.AsSceneManagerSettings(), transition.Name.AsField());
            }
            Ctx._("public {0} {1} = new {0}();", Ctx.Data.Name.AsSceneManagerSettings(), Ctx.Data.Name.AsSceneManagerSettings().AsField());
        }

        Ctx.AddIterator("InstanceProperty", node => node.ImportedItems);
        Ctx.AddIterator("ControllerProperty", node => node.IncludedElements);
        Ctx.AddIterator("GetTransitionScenes", node => node.SceneTransitions);
        Ctx.AddIterator("TransitionMethod", node => node.SceneTransitions);
        Ctx.AddIterator("TransitionComplete", node => node.SceneTransitions);
    }

    public TemplateContext<SceneManagerNode> Ctx { get; set; }


    //[Inject("LocalPlayer")]
    [TemplateProperty("{0}", AutoFillType.NameOnly)]
    public virtual ViewModel InstanceProperty
    {
        get
        {
            var instance = Ctx.ItemAs<InstancesReference>();
            Ctx.SetType(instance.SourceItem.Name.AsViewModel());

            Ctx.AddAttribute(typeof(InjectAttribute))
                .Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(instance.Name)));

            Ctx._if("this.{0} == null", instance.Name.AsField())
                .TrueStatements._("this.{0} = CreateInstanceViewModel<{1}>( \"{2}\")", instance.Name.AsField(), instance.SourceItem.Name.AsViewModel(), instance.Name);

            Ctx.CurrentDecleration._private_(Ctx.CurrentProperty.Type, instance.Name.AsField());
            Ctx._("return {0}", instance.Name.AsField());

            //if ((this._LocalPlayer == null)) {
            //    this._LocalPlayer = CreateInstanceViewModel<FPSPlayerViewModel>(FPSPlayerController, "LocalPlayer");
            //}
            //return this._LocalPlayer;
            return null;
        }
        set
        {
            //_LocalPlayer = value;
        }
    }

    //[Inject()]
    [TemplateProperty(uFrameFormats.CONTROLLER_FORMAT, AutoFillType.NameOnly)]
    public virtual Controller ControllerProperty
    {
        get
        {
            Ctx.SetType(Ctx.Item.Name.AsController());
            Ctx.AddAttribute(typeof(InjectAttribute));
            Ctx.CurrentDecleration._private_(Ctx.CurrentProperty.Type, Ctx.Item.Name.AsController().AsField());
            Ctx.LazyGet(Ctx.Item.Name.AsController().AsField(), "Container.CreateInstance(typeof({0})) as {0};", Ctx.Item.Name.AsController());
            return null;
        }
        set
        {
            Ctx._("{0} = value", Ctx.Item.Name.AsController().AsField());
        }
    }

    // <summary>
    // This method is the first method to be invoked when the scene first loads. Anything registered here with 'Container' will effectively 
    // be injected on controllers, and instances defined on a subsystem.And example of this would be Container.RegisterInstance<IDataRepository>(new CodeRepository()). Then any property with 
    // the 'Inject' attribute on any controller or view-model will automatically be set by uFrame. 
    // </summary>
    [TemplateMethod(MemberGeneratorLocation.DesignerFile)]
    public virtual void Setup()
    {
        Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
        foreach (var item in Ctx.Data.ImportedItems)
        {
            Ctx._("Container.RegisterViewModel<{0}>({1}, \"{1}\")", item.SourceItem.Name.AsViewModel(), item.Name, item.Name);
        }
        foreach (var item in Ctx.Data.IncludedElements)
        {
            Ctx._("Container.RegisterViewModelManager<{0}>(new ViewModelManager<{0}>())", item.Name.AsViewModel());
            Ctx._("Container.RegisterController<{0}>({0})", item.Name.AsController());
        }
        
        Ctx._("Container.InjectAll()");

        
    }

    [TemplateMethod(MemberGeneratorLocation.Both)]
    public virtual void Initialize()
    {
        Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
        Ctx.CurrentMethodAttribute.CallBase = true;
        if (Ctx.IsDesignerFile)
            Ctx.CurrentMethod.invoke_base(); 
        Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("This method is called right after setup is invoked."));
        if (Ctx.IsDesignerFile)
        {
            foreach (var item in Ctx.Data.ImportedItems)
            {
                Ctx._("Publish(new ViewModelCreatedEvent() {{ ViewModel = {0} }});", item.Name);
            }
               
             
            foreach (var item in Ctx.Data.SceneTransitions)
            {
                var command = item.SourceItem as CommandsChildItem;
                if (command == null)
                {
                    Debug.Log("command is null");
                    continue;
                }
                var transitionTo = item.OutputTo<SceneManagerNode>();
                if (transitionTo == null)
                {
                    Debug.Log("Transition is null");
                    continue;
                }
                Ctx._("this.OnEvent<{0}>().Subscribe(_=>{{ {1}(_); }}).DisposeWith(this.gameObject);", command.ClassName, command.Name);
            }

        }

    }

    #region Transitions

    [TemplateMethod("Get{0}Scenes", MemberGeneratorLocation.DesignerFile, false, AutoFill = AutoFillType.NameOnly)]
    public virtual System.Collections.Generic.IEnumerable<string> GetTransitionScenes()
    {
        Ctx._("return {0}Transition._Scenes", Ctx.Item.Name.AsField());
        //return this._QuitGameTransition._Scenes;
        return null;
    }

    [TemplateMethod(AutoFill = AutoFillType.NameOnly)]
    public virtual void TransitionMethod(object command)
    {
        
        var transition = Ctx.ItemAs<SceneTransitionsReference>();
        var c = transition.SourceItem as CommandsChildItem;
        if (c == null) return;

        this.Ctx.CurrentMethod.Parameters[0].Type = c.ClassName.ToCodeReference();

        //        var transitionCommand = transition.SourceItem as CommandsChildItem;
        var transitionOutput = transition.OutputTo<SceneManagerNode>();
        if (transitionOutput != null)
        {
            Ctx._("GameManager.TransitionLevel<{0}>((container) =>{{container.{1} = _{2}Transition; {2}TransitionComplete(container); }}, this.Get{2}Scenes().ToArray())",
                transitionOutput.Name.AsSceneManager(), transitionOutput.Name.AsSceneManagerSettings().AsField(), transition.Name);
        }

        //GameManager.TransitionLevel<FPSMainMenuManager>((container) =>{container._FPSMainMenuManagerSettings = _QuitGameTransition; QuitGameTransitionComplete(container); }, this.GetQuitGameScenes().ToArray());
    }

    [TemplateMethod("{0}TransitionComplete", MemberGeneratorLocation.Both, true)]
    public virtual void TransitionComplete(object sceneManager)
    {
        //if (!Ctx.IsDesignerFile) return;
        var transition = Ctx.ItemAs<SceneTransitionsReference>();
        var transitionOutput = transition.OutputTo<SceneManagerNode>();
        if (transition == null) return;
        Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(transitionOutput.Name.AsSceneManager());
    }

    //public override void Initialize()
    //{
    //    base.Initialize();
    //    if (Ctx.)
    //    //foreach (var item in Ctx.Data.Trnasitions)
    //    //{

    //    //    Ctx._("{0}.{1}.Subscribe(_=> {2}()).DisposeWith(this.gameObject)",item);
    //    //}
    //    // FPSGame.MainMenu.Subscribe(_=> MainMenu()).DisposeWith(this.gameObject);
    //    // FPSGame.QuitGame.Subscribe(_=> QuitGame()).DisposeWith(this.gameObject);
    //}

    #endregion



}