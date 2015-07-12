using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using uFrame.MVVM;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both)]
    public partial class ViewComponentTemplate : IClassTemplate<ViewComponentNode>, IClassRefactorable
    {
        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Graph.Name, "ViewComponents"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            this.Ctx.TryAddNamespace("uFrame.Kernel");
            this.Ctx.TryAddNamespace("uFrame.MVVM");
            this.Ctx.TryAddNamespace("uFrame.MVVM.Bindings");
            this.Ctx.TryAddNamespace("uFrame.Serialization");
            if (Ctx.IsDesignerFile)
            {
                Ctx.SetBaseType(typeof(ViewComponent));
            }
            foreach (var property in Ctx.Data.View.Element.PersistedItems.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null) continue;

                Ctx.TryAddNamespace(type.Namespace);
            }
            Ctx.AddIterator("ExecuteCommandOverload", _ => _.View.Element.LocalCommands);
            //Ctx.AddIterator("ExecuteCommand", _ => _.View.Element.InheritedCommandsWithLocal.Where(p => string.IsNullOrEmpty(p.RelatedTypeName)));
            ////Ctx.AddIterator("ExecuteCommandOverload", _ => _.View.Element.InheritedCommandsWithLocal);
            //Ctx.AddIterator("ExecuteCommandWithArg", _ => _.View.Element.InheritedCommandsWithLocal.Where(p => !string.IsNullOrEmpty(p.RelatedTypeName) && p.OutputCommand == null));

        }


        public TemplateContext<ViewComponentNode> Ctx { get; set; }

        public string ElementName
        {
            get
            {
                return Ctx.Data.View.Element.Name;
            }
        }

        [GenerateProperty]
        public virtual object _ElementName_
        {
            get
            {
                Ctx.CurrentProperty.Type = Ctx.Data.View.Element.Name.AsViewModel().ToCodeReference();
                Ctx._("return ({0})this.View.ViewModelObject", Ctx.Data.View.Element.Name.AsViewModel());
                return null;
            }
        }

        public IEnumerable<CommandsChildItem> CommandsWithArguments
        {
            get { return Ctx.Data.View.Element.InheritedCommandsWithLocal.Where(p => !string.IsNullOrEmpty(p.RelatedTypeName) && p.OutputCommand == null); }
        }

        public IEnumerable<CommandsChildItem> Commands
        {
            get { return Ctx.Data.View.Element.InheritedCommandsWithLocal.Where(p => string.IsNullOrEmpty(p.RelatedTypeName)); }
        }

        [ForEach("Commands"), GenerateMethod]
        public void Execute_Name_()
        {
            Ctx._("{0}.{1}.OnNext(new {1}Command() {{ Sender = {0} }})", 
                Ctx.Data.View.Element.Name, Ctx.Item.Name);
        }

        
        [ForEach("CommandsWithArguments"), GenerateMethod(CallBase = false)]
        public void Execute_Name2_(object arg)
        {
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.TypedItem.RelatedTypeName);
            Ctx._("{0}.{1}.OnNext(new {1}Command() {{ Sender = {0}, Argument = arg }})", Ctx.Data.View.Element.Name, Ctx.Item.Name);
        }

        [GenerateMethod("Execute{0}", TemplateLocation.DesignerFile, false)]
        public void ExecuteCommandOverload(ViewModelCommand command)
        {
            Ctx.CurrentMethod.Parameters[0].Type = (Ctx.Item.Name + "Command").ToCodeReference();
            Ctx._("command.Sender = {0}", Ctx.Data.View.Element.Name);
            Ctx._("{0}.{1}.OnNext(command)", Ctx.Data.View.Element.Name, Ctx.Item.Name);
            //Ctx._("this.ExecuteCommand({0}.{1})", Ctx.Data.Element.Name, Ctx.Item.Name);
        }

        public IEnumerable<string> ClassNameFormats
        {
            get
            {
                yield return "{0}";
                yield return "{0}Base";
            }
        }
    }


}