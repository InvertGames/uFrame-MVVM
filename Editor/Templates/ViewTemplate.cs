using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using UnityEngine;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both)]
    public partial class ViewTemplate : IClassTemplate<ViewNode>, IClassRefactorable
    {
        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Graph.Name, "Views"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        [GenerateProperty(TemplateLocation.DesignerFile)]
        public string DefaultIdentifier
        {
            get
            {
                this.Ctx.CurrentProperty.Attributes = MemberAttributes.Override | MemberAttributes.Public;
                var instance = Ctx.Data.Element.RegisteredInstances.FirstOrDefault();
                if (instance != null)
                {
                    Ctx._("return \"{0}\"", instance.Name);
                }
                else
                {
                    Ctx._("return base.DefaultIdentifier");
                }

                return null;
            }
        }


        public virtual void TemplateSetup()
        {
            this.Ctx.TryAddNamespace("uFrame.Kernel");
            this.Ctx.TryAddNamespace("uFrame.MVVM");
            this.Ctx.TryAddNamespace("uFrame.MVVM.Services");
            this.Ctx.TryAddNamespace("uFrame.MVVM.Bindings");
            this.Ctx.TryAddNamespace("uFrame.Serialization");

            this.Ctx.TryAddNamespace("UniRx");
            this.Ctx.TryAddNamespace("UnityEngine");

            // Let the Dll Know about uFrame Binding Specific Types
            uFrameBindingType.ObservablePropertyType = typeof (IObservableProperty);
            uFrameBindingType.UFGroupType = typeof (UFGroup);
            uFrameBindingType.ICommandType = typeof (ISignal);
            foreach (var property in Ctx.Data.Element.PersistedItems.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null) continue;

                Ctx.TryAddNamespace(type.Namespace);
            }

            if (Ctx.IsDesignerFile && Ctx.Data.BaseNode == null)
            {
                Ctx.SetBaseType(typeof (ViewBase));
            }
            // Add namespaces based on the types used for properties
            Ctx.AddIterator("ViewComponentProperty", _ => _.OutputsTo<ViewComponentNode>());
            // Add the iterators for template method/property
            if (Ctx.Data.BaseNode == null)
            {
                Ctx.AddIterator("ExecuteCommand",
                    _ => _.Element.InheritedCommandsWithLocal.Where(p => string.IsNullOrEmpty(p.RelatedTypeName)));
                Ctx.AddIterator("ExecuteCommandOverload", _ => _.Element.InheritedCommandsWithLocal);
                Ctx.AddIterator("ExecuteCommandWithArg",
                    _ =>
                        _.Element.InheritedCommandsWithLocal.Where(
                            p => !string.IsNullOrEmpty(p.RelatedTypeName) && p.OutputCommand == null));
            }
            else
            {
                Ctx.AddIterator("ExecuteCommand",
                    _ => _.Element.LocalCommands.Where(p => string.IsNullOrEmpty(p.RelatedTypeName)));
                Ctx.AddIterator("ExecuteCommandOverload", _ => _.Element.LocalCommands);
                Ctx.AddIterator("ExecuteCommandWithArg",
                    _ =>
                        _.Element.LocalCommands.Where(
                            p => !string.IsNullOrEmpty(p.RelatedTypeName) && p.OutputCommand == null));
            }

            Ctx.AddIterator("ResetProperty", _ => _.SceneProperties);
            Ctx.AddIterator("CalculateProperty", _ => _.SceneProperties);
            Ctx.AddIterator("GetPropertyObservable", _ => _.SceneProperties);
            Ctx.AddCondition("ViewModelProperty", _ => !_.IsDerivedOnly);
            Ctx.AddCondition("DefaultIdentifier", _ => !_.IsDerivedOnly);
            Ctx.AddCondition("ViewModelType", _ => !_.IsDerivedOnly);
            if (!Ctx.IsDesignerFile)
            {
                // For each binding lets do some magic
                foreach (var item in Ctx.Data.Bindings)
                {
                    // Cast the source of our binding (ie: Property, Collection, Command..etc)
                    var source = item.SourceItem as ITypedItem;
                    if (source == null) continue;
                    // Grab the uFrame Binding Type
                    var bindingType = item.BindingType;
                    // Create the binding signature based on the Method Info

                    bindingType.CreateBindingSignature(new CreateBindingSignatureParams(
                        Ctx.CurrentDeclaration, _ => source.RelatedTypeName.ToCodeReference(), Ctx.Data, source)
                    {
                        Ctx = Ctx,
                        BindingsReference = item,
                        DontImplement = true
                    });
                }
            }
        }

        public TemplateContext<ViewNode> Ctx { get; set; }

        #region SceneProperties

        [GenerateMethod("Reset{0}", TemplateLocation.DesignerFile, false)]
        public virtual void ResetProperty()
        {
            // Make sure the disposable is created
            Ctx.CurrentDeclaration._private_(typeof (System.IDisposable), "_{0}Disposable", Ctx.Item.Name);
            Ctx._if("_{0}Disposable != null", Ctx.Item.Name)
                .TrueStatements
                ._("_{0}Disposable.Dispose()", Ctx.Item.Name);
            Ctx._("_{0}Disposable = Get{0}Observable().Subscribe({1}.{2}).DisposeWith(this)", Ctx.Item.Name,
                Ctx.Data.Element.Name, Ctx.Item.Name.AsSubscribableProperty());

        }

        [GenerateMethod("Calculate{0}", TemplateLocation.Both, true)]
        protected virtual String CalculateProperty()
        {
            Ctx.SetType(Ctx.TypedItem.RelatedTypeName);
            Ctx._("return default({0})", Ctx.TypedItem.RelatedTypeName);
            return default(String);
        }

        [GenerateMethod("Get{0}Observable", TemplateLocation.DesignerFile, false)]
        protected virtual UniRx.IObservable<String> GetPropertyObservable()
        {
            this.Ctx.SetTypeArgument(Ctx.TypedItem.RelatedTypeName);
            Ctx._("return this.UpdateAsObservable().Select(p=>Calculate{0}())", Ctx.Item.Name);
            return null;
        }

        #endregion

        [GenerateProperty]
        public virtual Type ViewModelType
        {
            get
            {
                Ctx.CurrentProperty.Attributes |= MemberAttributes.Override;
                Ctx._("return typeof({0})", Ctx.Data.Element.Name.AsViewModel());
                return null;
            }
        }

        public string ElementName
        {
            get { return Ctx.Data.Element.Name; }
        }

        [GenerateProperty]
        public Type _ElementName_
        {
            get
            {
                Ctx.CurrentProperty.Type = Ctx.Data.Element.Name.AsViewModel().ToCodeReference();
                Ctx._("return ({0})ViewModelObject", Ctx.Data.Element.Name.AsViewModel());
                return null;
            }
        }

        //[TemplateMethod(CallBase = false)]
        //public virtual ViewModel CreateModel()
        //{
        //    Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
        //    //var property = Context.Get<IDiagramNodeItem>();
        //    Ctx._("return this.FetchViewModel(Identifier,ViewModelType)");
        //    return null;
        //}

        [GenerateMethod, Inside(TemplateLocation.Both)]
        protected virtual void InitializeViewModel(ViewModel model)
        {
            Ctx._comment("NOTE: this method is only invoked if the 'Initialize ViewModel' is checked in the inspector.");
            Ctx._comment("var vm = model as {0};", Ctx.Data.Element.Name.AsViewModel());
            Ctx._comment(
                "This method is invoked when applying the data from the inspector to the viewmodel.  Add any view-specific customizations here.");

            Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
            if (!Ctx.IsDesignerFile) return;
            Ctx.CurrentMethod.invoke_base(true);
            if (!Ctx.Data.Element.LocalProperties.Any()) return;
            if (Ctx.Data.IsDerivedOnly) return;
            var variableName = Ctx.Data.Name.ToLower();
            Ctx._("var {0} = (({1})model)", variableName, Ctx.Data.Element.Name.AsViewModel());

            foreach (var property in Ctx.Data.Element.LocalProperties)
            {
                if (property.RelatedTypeNode is StateMachineNode) continue;
                // Make sure derived views don't duplicate in initialize vm

                var field = Ctx.CurrentDeclaration._public_(property.RelatedTypeName, property.Name.AsField());
                field.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof (SerializeField))));
                field.CustomAttributes.Add(
                    new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof (UFGroup)),
                        new CodeAttributeArgument(
                            new CodePrimitiveExpression("View Model Properties"))));

                field.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof (HideInInspector))));
                var relatedNode = property.RelatedTypeNode;
                var relatedViewModel = relatedNode as ElementNode;

                if (relatedViewModel == null) // Non ViewModel Properties
                {

                    Ctx._("{0}.{1} = this.{2}", variableName, property.Name, property.Name.AsField());
                }
                else
                {
                    field.Type = new CodeTypeReference(typeof (ViewBase));
                    Ctx._("{0}.{1} = this.{2} == null ? null :  ViewService.FetchViewModel(this.{2}) as {3}",
                        variableName, property.Name, property.Name.AsField(), relatedViewModel.Name.AsViewModel());
                }
            }


        }

        [GenerateMethod, Inside(TemplateLocation.Both)]
        public virtual void Bind()
        {
            Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
            Ctx._comment("Use this.{0} to access the viewmodel.", Ctx.Data.Element.Name);
            Ctx._comment("Use this method to subscribe to the view-model.");
            Ctx._comment("Any designer bindings are created in the base implementation.");

            if (!Ctx.IsDesignerFile)
            {

                return;
            }

            Ctx.CurrentMethod.invoke_base(true);


            // For each binding lets do some magic
            foreach (var item in Ctx.Data.Bindings)
            {
                // Cast the source of our binding (ie: Property, Collection, Command..etc)
                var source = item.SourceItem as ITypedItem;
                // Create a boolean field for each property that has a binding this will serve the condition
                // in the bind method to turn the binding on or off.
             //   var bindingField = Ctx.CurrentDeclaration._public_(typeof (bool), "_Bind{0}", source.Name);


                var bindingField =
                Ctx.CurrentDeclaration.Members.OfType<CodeMemberField>()
                    .FirstOrDefault(x => x.Name == string.Format("_Bind{0}", source.Name));

                if (bindingField == null)
                {
                    bindingField = Ctx.CurrentDeclaration._public_(typeof (bool), "_Bind{0}", source.Name);

                    // Bindings should always be on by default
                    bindingField.InitExpression = new CodePrimitiveExpression(true);
                    // Add a toggle group attribute to it, this hides and shows anything within the same group
                    bindingField.CustomAttributes.Add(
                        new CodeAttributeDeclaration(new CodeTypeReference(typeof (UFToggleGroup)),
                            new CodeAttributeArgument(new CodePrimitiveExpression(source.Name))));
                    // Hide them in the insepctor, our custom 'ViewInspector' class will handle them manually
                    bindingField.CustomAttributes.Add(
                        new CodeAttributeDeclaration(new CodeTypeReference(typeof (HideInInspector))));
                }

                // Create the binding condition
                var bindingCondition = Ctx._if("{0}", bindingField.Name);
                // Grab the uFrame Binding Type
                var bindingType = item.BindingType;
                // Create the binding signature based on the Method Info
                var bindingStatement =
                    bindingType.CreateBindingSignature(new CreateBindingSignatureParams(
                        Ctx.CurrentDeclaration, _ => source.RelatedTypeName.ToCodeReference(), Ctx.Data, source)
                    {
                        Ctx = Ctx
                    });


                // Add the binding statement to the condition
                bindingCondition.TrueStatements.Add(bindingStatement);
            }

            foreach (var property in Ctx.Data.SceneProperties)
            {
                Ctx._("Reset{0}()", property.Name);
            }
        }

        [GenerateMethod("Execute{0}", TemplateLocation.DesignerFile, false)]
        public void ExecuteCommand()
        {
            Ctx._("{0}.{1}.OnNext(new {1}Command() {{ Sender = {0} }})", Ctx.Data.Element.Name, Ctx.Item.Name);
            //Ctx._("this.ExecuteCommand({0}.{1})", Ctx.Data.Element.Name, Ctx.Item.Name);
        }

        [GenerateMethod("Execute{0}", TemplateLocation.DesignerFile, false)]
        public void ExecuteCommandOverload(ViewModelCommand command)
        {
            Ctx.CurrentMethod.Parameters[0].Type = (Ctx.Item.Name + "Command").ToCodeReference();
            Ctx._("command.Sender = {0}", Ctx.Data.Element.Name);
            Ctx._("{0}.{1}.OnNext(command)", Ctx.Data.Element.Name, Ctx.Item.Name);
            //Ctx._("this.ExecuteCommand({0}.{1})", Ctx.Data.Element.Name, Ctx.Item.Name);
        }

        [GenerateMethod("Execute{0}", TemplateLocation.DesignerFile, false)]
        public void ExecuteCommandWithArg(object arg)
        {
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.TypedItem.RelatedTypeName);
            Ctx._("{0}.{1}.OnNext(new {1}Command() {{ Sender = {0}, Argument = arg }})", Ctx.Data.Element.Name,
                Ctx.Item.Name);
        }

        public IEnumerable<string> ClassNameFormats
        {
            get
            {
                yield return "{0}";
                yield return "{0}Base";
            }
        }

        [GenerateProperty("{0}")]
        public virtual _ITEMTYPE_ ViewComponentProperty
        {
            get
            {

                var field = Ctx.CurrentDeclaration._private_(Ctx.Item.Name, Ctx.Item.Name.AsField());
                field.CustomAttributes.Add(new CodeAttributeDeclaration(typeof (SerializeField).ToCodeReference()));
                Ctx.CurrentProperty.Type = field.Type;
                Ctx._("return {0} ?? ({0} = this.gameObject.EnsureComponent<{1}>())", field.Name, Ctx.Item.Name);
                return null;
            }

        }
    }
    
}
