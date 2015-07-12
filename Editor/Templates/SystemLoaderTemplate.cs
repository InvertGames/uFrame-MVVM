using System.CodeDom;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using uFrame.Kernel;
using UnityEngine;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, "{0}Loader")]
    public partial class SystemLoaderTemplate : IClassTemplate<SubsystemNode>
    {
        public void TemplateSetup()
        {
            this.Ctx.TryAddNamespace("uFrame.IOC");
            this.Ctx.TryAddNamespace("uFrame.Kernel");
            this.Ctx.TryAddNamespace("uFrame.MVVM");
            foreach (var property in Ctx.Data.PersistedItems.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null) continue;

                Ctx.TryAddNamespace(type.Namespace);
            }

            if (Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Add(typeof (MonoBehaviour).ToCodeReference());
                Ctx.SetBaseType(typeof (SystemLoader));
            }

            //Ctx.AddIterator("InstanceProperty", node => node.Instances);
            Ctx.AddIterator("ControllerProperty",
                node => node.GetContainingNodesInProject(Ctx.Data.Project).OfType<ElementNode>());
        }

        [GenerateMethod(CallBase = true), Inside(TemplateLocation.Both)]
        public void Load()
        {
            Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;

            if (!Ctx.IsDesignerFile)
                Ctx.CurrentMethod.invoke_base();

            if (Ctx.IsDesignerFile)
            {

                foreach (
                    var item in Ctx.Data.GetContainingNodesInProject(Ctx.Data.Project).OfType<ElementNode>().Distinct())
                {
                    Ctx._("Container.RegisterViewModelManager<{0}>(new ViewModelManager<{0}>())",
                        item.Name.AsViewModel());
                    Ctx._("Container.RegisterController<{0}>({0})", item.Name.AsController());
                }

                foreach (var item in Ctx.Data.Instances.Distinct())
                {
                    Ctx._("Container.RegisterViewModel<{0}>({1}, \"{1}\")", item.SourceItem.Name.AsViewModel(),
                        item.Name, item.Name);
                }


            }

        }


        //[Inject("LocalPlayer")]
        [ForEach("Instances"), GenerateProperty]
        public virtual ViewModel _Name_
        {
            get
            {
                var instance = Ctx.ItemAs<InstancesReference>();
                Ctx.SetType(instance.SourceItem.Name.AsViewModel());

                Ctx.AddAttribute(typeof (uFrame.IOC.InjectAttribute))
                    .Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(instance.Name)));

                Ctx._if("this.{0} == null", instance.Name.AsField())
                    .TrueStatements._("this.{0} = this.CreateViewModel<{1}>( \"{2}\")", instance.Name.AsField(),
                        instance.SourceItem.Name.AsViewModel(), instance.Name);

                Ctx.CurrentDeclaration._private_(Ctx.CurrentProperty.Type, instance.Name.AsField());
                Ctx._("return {0}", instance.Name.AsField());

                return null;
            }
            set
            {
                //_LocalPlayer = value;
            }
        }

        //[Inject()]
        [GenerateProperty(uFrameFormats.CONTROLLER_FORMAT)]
        public virtual Controller ControllerProperty
        {
            get
            {
                Ctx.SetType(Ctx.Item.Name.AsController());
                Ctx.AddAttribute(typeof (uFrame.IOC.InjectAttribute));
                Ctx.CurrentDeclaration._private_(Ctx.CurrentProperty.Type, Ctx.Item.Name.AsController().AsField());
                Ctx.LazyGet(Ctx.Item.Name.AsController().AsField(), "Container.CreateInstance(typeof({0})) as {0};",
                    Ctx.Item.Name.AsController());
                return null;
            }
            set { Ctx._("{0} = value", Ctx.Item.Name.AsController().AsField()); }
        }

        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Graph.Name, "SystemLoaders"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public TemplateContext<SubsystemNode> Ctx { get; set; }
    }
}