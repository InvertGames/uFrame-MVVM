using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = uFrameFormats.CONTROLLER_FORMAT)]
    [RequiresNamespace("uFrame.IOC")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("uFrame.Serialization")]
    [RequiresNamespace("UniRx")]
    [NamespacesFromItems()]
    [AsAbstract]
    public partial class ControllerTemplate : Controller, IClassTemplate<ElementNode>, IClassRefactorable, IMethodRefactorable
    {
        public TemplateContext<ElementNode> Ctx { get; set; }

        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Graph.Name, "Controllers"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
        
   
        }
        public IEnumerable<ITypedItem> CommandsWithoutArgs
        {
            get { return Ctx.Data.AllCommandHandlers.Where(p => string.IsNullOrEmpty(p.RelatedTypeName)); }
        }
        public IEnumerable<ITypedItem> CommandsWithArgs
        {
            get { return Ctx.Data.AllCommandHandlers.Where(p => !string.IsNullOrEmpty(p.RelatedTypeName)); }
        }
        public IEnumerable<InstancesReference> Instances
        {
            get
            {
                if (Ctx.Data.BaseNode == null)
                {
                    foreach (
                        var item in Ctx.Data.Graph.NodeItems.OfType<SubsystemNode>().SelectMany(p => p.Instances).Distinct()
                        )
                    {
                        yield return item;
                    }
                }
            }
        }

        public string NameAsViewModel { get { return Ctx.Data.Name.AsViewModel(); } }


        //[TemplateProperty(TemplateLocation.DesignerFile, AutoFill = AutoFillType.NameAndTypeWithBackingField, NameFormat = "{0}ViewModelManager")]
        [GenerateProperty, WithField]
        public IViewModelManager _Name_ViewModelManager
        {
            get
            {
                Ctx.SetType(typeof(IViewModelManager)); // I force this so it doesn't change it
                Ctx.CurrentProperty.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(uFrame.IOC.InjectAttribute).ToCodeReference(), new CodeAttributeArgument(new CodePrimitiveExpression(Ctx.Data.Name))));
                return null;
            }
        }

        [ForEach("Instances"), GenerateProperty, WithField]
        public _REFNAME_VIEWMODEL _Name_
        {
            get
            {
                
                Ctx.CurrentProperty.CustomAttributes.Add(new CodeAttributeDeclaration(
                    typeof(uFrame.IOC.InjectAttribute).ToCodeReference(),
                    new CodeAttributeArgument(new CodePrimitiveExpression(Ctx.ItemAs<InstancesReference>().Name))
                    ));

                return null;
            }
            set
            {

            }
        }

        [GenerateMethod(TemplateLocation.Both)]
        public override void Setup()
        {
            base.Setup();
            Ctx._comment("This is called when the controller is created");
            if (Ctx.IsDesignerFile)
            {
                //            foreach (var command in Ctx.Data.AllCommandHandlers)
                //            {
                //                Ctx._("this.OnEvent<{0}Command>().Subscribe(this.{0}Handler)", command.Name);
                //            }
                //foreach (var command in Ctx.Data.Handlers.Where(p => !(p.SourceItem is CommandsChildItem)))
                //{
                //    Ctx._("this.OnEvent<{0}>().Subscribe(this.{0}Handler)", command.Name);
                //}

                //Ctx._("this.EventAggregator.OnViewModelCreated<{0}>().Subscribe(vm => this.Initialize{1}(vm as {0}));", Ctx.Data.Name.AsViewModel(), Ctx.Data.Name);
                //Ctx._("this.EventAggregator.OnViewModelDestroyed<{0}>().Subscribe(this.DisposingViewModel);", Ctx.Data.Name.AsViewModel());

            }


        }

        [GenerateProperty]
        public IEnumerable<_ITEMNAME_VIEWMODEL> _Name_ViewModels
        {
            get
            {
                Ctx._("return {1}ViewModelManager.OfType<{0}>()", Ctx.Data.Name.AsViewModel(), Ctx.Data.Name);
                return null;
            }
        }

        [GenerateMethod]
        public override void Initialize(ViewModel viewModel)
        {
            Ctx._comment("This is called when a viewmodel is created");
            if (!Ctx.IsDesignerFile) return;
            Ctx._("this.Initialize{0}((({1})(viewModel)))", Ctx.Data.Name, NameAsViewModel);
        }

        [GenerateMethod(CallBase = false)]
        public _ITEMNAME_VIEWMODEL Create_Name_()
        {
            Ctx._("return (({0})(this.Create(Guid.NewGuid().ToString())))", NameAsViewModel);
            return null;
        }

        [GenerateMethod(CallBase = false)]
        public override ViewModel CreateEmpty()
        {
            Ctx._("return new {0}(this.EventAggregator)", NameAsViewModel);
            return null;
        }

        [GenerateMethod("Initialize{0}", TemplateLocation.Both, true), Inside(TemplateLocation.Both)]
        public virtual void InitializeElement(ViewModel viewModel)
        {
            Ctx._comment("This is called when a {0} is created", NameAsViewModel);
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(NameAsViewModel);
            if (Ctx.IsDesignerFile)
            {
                foreach (var command in Ctx.Data.LocalCommands)
                {
                    Ctx._("viewModel.{0}.Action = this.{0}Handler", command.Name);
                }
                Ctx._("{0}ViewModelManager.Add(viewModel)", Ctx.Data.Name);
            }

        }

        [GenerateMethod(TemplateLocation.DesignerFile, true)]
        public override void DisposingViewModel(ViewModel viewModel)
        {
            base.DisposingViewModel(viewModel);
            Ctx._("{0}ViewModelManager.Remove(viewModel)", Ctx.Data.Name);
        }

        [ForEach("CommandsWithoutArgs"), GenerateMethod, InsideAll]
        public virtual void _Name2_(ViewModel viewModel)
        {
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Item.Node.Name + "ViewModel");

        }

        [ForEach("LocalCommands"), GenerateMethod, WithNameFormat("{0}Handler")]
        public virtual void _Name_Handler(ViewModelCommand command)
        {
            if (Ctx.Item is CommandsChildItem)
            {
                Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Item.Name + "Command");
            }
            else
            {
                Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Item.Name);
            }

            if (Ctx.IsDesignerFile)
            {
                if (Ctx.Item is CommandsChildItem)
                {
                    var c = Ctx.TypedItem;
                    if (Ctx.ItemAs<CommandsChildItem>().OutputCommand != null)
                    {
                        Ctx._("this.{0}(command.Sender as {1}, command)", c.Name, c.Node.Name.AsViewModel());

                    }
                    else if (string.IsNullOrEmpty(c.RelatedType))
                    {
                        Ctx._("this.{0}(command.Sender as {1})", c.Name, c.Node.Name.AsViewModel());
                    }
                    else
                    {
                        Ctx._("this.{0}(command.Sender as {1}, command.Argument)", c.Name, c.Node.Name.AsViewModel());
                    }
                    if (Ctx.ItemAs<CommandsChildItem>().Publish)
                    {
                        Ctx._("this.Publish(command)");
                    }
                }

            }
        }

        [ForEach("CommandsWithArgs"), GenerateMethod, WithNameFormat("{0}"), InsideAll]
        public virtual void _CommandName_(ViewModel viewModel, object arg)
        {
            _Name2_(viewModel);
            Ctx.CurrentMethod.Parameters[1].Type = new CodeTypeReference(Ctx.TypedItem.RelatedTypeName);

        }

        public IEnumerable<string> ClassNameFormats
        {
            get
            {

                yield return "{0}Controller";
                yield return "{0}ControllerBase";
            }
        }

        public IEnumerable<string> MethodFormats
        {
            get { yield return "Initialize{0}"; }
        }
    }
}
