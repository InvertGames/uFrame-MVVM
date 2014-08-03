using Invert.uFrame.Editor;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class ViewClassGenerator : CodeGenerator
{
    private Dictionary<string, CodeConditionStatement> _bindingConditionStatements;
    public List<ViewBindingExtender> BindingExtenders { get; set; }

    public CodeTypeDeclaration Decleration { get; set; }

    public IElementDesignerData DiagramData
    {
        get;
        set;
    }
    public bool HasField(CodeTypeMemberCollection collection, string name)
    {
        return collection.OfType<CodeMemberField>().Any(item => item.Name == name);
    }

    public CodeConditionStatement AddBindingCondition(CodeTypeDeclaration decl, CodeStatementCollection statements, IViewModelItem item, ElementDataBase relatedElement)
    {
        var bindField = new CodeMemberField
        {
            Name = "_Bind" + item.Name,
            Type = new CodeTypeReference(typeof(bool)),
            Attributes = MemberAttributes.Public,
            InitExpression = new CodePrimitiveExpression(true)
        };

        bindField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(uFrameEditor.uFrameTypes.UFToggleGroup),
            new CodeAttributeArgument(new CodePrimitiveExpression(item.Name))));
        bindField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(HideInInspector))));

        var prop = item as ViewModelPropertyData;
        var coll = item as ViewModelCollectionData;
        if (prop != null)
        {
            if (relatedElement == null)
            {
                bindField.CustomAttributes.Add(
                    new CodeAttributeDeclaration(new CodeTypeReference(uFrameEditor.uFrameTypes.UFRequireInstanceMethod),
                        new CodeAttributeArgument(new CodePrimitiveExpression(prop.NameAsChangedMethod))));
            }
        }
        else if (coll != null)
        {
            //bindField.CustomAttributes.Add(
            //  new CodeAttributeDeclaration(new CodeTypeReference(typeof(UFRequireInstanceMethod)),
            //      new CodeAttributeArgument(new CodePrimitiveExpression(relatedElement == null ? coll.NameAsAddHandler : coll.NameAsCreateHandler))));
        }

        decl.Members.Add(bindField);
        var conditionStatement = new CodeConditionStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), bindField.Name));
        statements.Add(conditionStatement);
        return conditionStatement;
    }

    public void AddPropertyBinding(ElementData data, CodeTypeDeclaration decl, CodeStatementCollection statements, ViewModelPropertyData propertyData, bool asTwoWay, ElementDataBase relatedElement)
    {
        var memberInvoke = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "BindProperty");

        memberInvoke.Parameters.Add(
            new CodeSnippetExpression(string.Format("()=>{0}.{1}", data.Name, propertyData.FieldName)));
        memberInvoke.Parameters.Add(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), propertyData.NameAsChangedMethod));

        if (!asTwoWay)
        {
            var setterMethod = new CodeMemberMethod { Name = propertyData.NameAsChangedMethod };
            setterMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference(relatedElement == null ? propertyData.RelatedTypeName : relatedElement.NameAsViewModel), "value"));
            setterMethod.Attributes = MemberAttributes.Public;

            if (relatedElement == null)
            {
                foreach (var viewBindingExtender in BindingExtenders)
                {
                    viewBindingExtender.ExtendPropertyBinding(data, setterMethod.Statements, propertyData, null);
                }
            }
            else
            {
                var viewPrefabField = AddPropertyBindingField(decl, typeof(GameObject).FullName, propertyData.Name, "Prefab");
                setterMethod.Statements.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("value == null && {0} != null && {0}.gameObject != null", propertyData.ViewFieldName)),
                    new CodeExpressionStatement(
                        new CodeMethodInvokeExpression(null,
                            "Destroy",
                            new CodeSnippetExpression(string.Format("{0}.gameObject", propertyData.ViewFieldName))))));

                var prefabSetCondition =
                    new CodeConditionStatement(
                        new CodeSnippetExpression(String.Format((string)"{0} == null ", (object)viewPrefabField.Name)));

                prefabSetCondition.TrueStatements.Add(new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), propertyData.ViewFieldName),
                    new CodeCastExpression(new CodeTypeReference(relatedElement.NameAsViewBase),
                        new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "InstantiateView",
                            new CodeVariableReferenceExpression("value"))
                        )));

                prefabSetCondition.FalseStatements.Add(new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), propertyData.ViewFieldName),
                    new CodeCastExpression(new CodeTypeReference(relatedElement.NameAsViewBase),
                        new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "InstantiateView",
                            new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), viewPrefabField.Name),
                            new CodeVariableReferenceExpression("value"))
                        )));
                setterMethod.Statements.Add(prefabSetCondition);
            }

            decl.Members.Add(setterMethod);
        }
        else
        {
            memberInvoke.Parameters.Add(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), propertyData.NameAsTwoWayMethod));

            var getterMethod = new CodeMemberMethod
            {
                Name = propertyData.NameAsTwoWayMethod,
                ReturnType = new CodeTypeReference(relatedElement == null ? propertyData.RelatedTypeName : relatedElement.NameAsViewModel),
                Attributes = MemberAttributes.Public
            };
            if (relatedElement == null)
            {
                getterMethod.Statements.Add(
                    new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(NotImplementedException))));
            }
            else
            {
                getterMethod.Statements.Add(new CodeConditionStatement(
                    new CodeSnippetExpression(
                        string.Format("this.{0} == null || this.{0}.ViewModelObject == null", propertyData.ViewFieldName)),
                    new CodeMethodReturnStatement(new CodeSnippetExpression("null"))
                    ));

                getterMethod.Statements.Add(new CodeMethodReturnStatement(new CodeCastExpression(
                    new CodeTypeReference(relatedElement.NameAsViewModel),
                    new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), propertyData.ViewFieldName),
                        "ViewModelObject")
                    )));
            }

            decl.Members.Add(getterMethod);
        }

        //    public virtual void CurrentPlayerChanged(FPSPlayerViewModel value) {
        //    if (value == null)
        //        Destroy(this._CurrentPlayer.gameObject);
        //    this._CurrentPlayer = ((FPSPlayerViewBase)(this.InstantiateView(value)));
        //}

        //public virtual FPSPlayerViewModel GetCurrentPlayerTwoWayValue()
        //{
        //    if (this._CurrentPlayer == null || this._CurrentPlayer.ViewModelObject == null) return null;
        //    return ((FPSPlayerViewModel)(this._CurrentPlayer.ViewModelObject));
        //}

        statements.Add(memberInvoke);
    }

    public CodeMemberField AddPropertyBindingField(CodeTypeDeclaration decl, string typeFullName, string propertyName, string name, bool keepHidden = false)
    {
        var memberField =
            new CodeMemberField(
                typeFullName,
                "_" + propertyName + name) { Attributes = MemberAttributes.Public };
        if (!keepHidden)
        {
            memberField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(uFrameEditor.uFrameTypes.UFGroup),
                new CodeAttributeArgument(new CodePrimitiveExpression(propertyName))));
        }

        memberField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(HideInInspector))));

        decl.Members.Add(memberField);
        return memberField;
    }

    public CodeMemberMethod CreateUpdateMethod(ViewData view, CodeTypeDeclaration decl)
    {
        var updateMethod = new CodeMemberMethod()
        {
            Attributes = MemberAttributes.Family | MemberAttributes.Override,
            Name = "Apply"
        };
        updateMethod.Statements.Add(new CodeSnippetExpression("base.Apply()"));
        var element = view.ViewForElement;
        var dirtyCondition =
            new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Dirty", element.Name)));
        //updateMethod.Statements.Add(dirtyCondition);
        // Create cached properties of monobehaviour types
        var componentTypeName = view.Properties.GroupBy(p => p.ComponentTypeName);
        foreach (var propertyGroup in componentTypeName)
        {
            var statements = updateMethod.Statements;
            if (propertyGroup.Key == typeof(Transform).FullName)
            {
                var ifTransformChanged = new CodeConditionStatement(new CodeSnippetExpression("Transform.hasChanged"));
                updateMethod.Statements.Add(ifTransformChanged);
                statements = ifTransformChanged.TrueStatements;



            }
            var first = propertyGroup.First();
            var viewPropertyFieldDecleration = new CodeMemberField(propertyGroup.Key, first.NameAsCachedPropertyField);
            var viewPropertyDecleration = viewPropertyFieldDecleration.EncapsulateField(first.NameAsCachedProperty,
                new CodeSnippetExpression(string.Format("this.GetComponent<{0}>()", propertyGroup.Key)));

            decl.Members.Add(viewPropertyFieldDecleration);
            decl.Members.Add(viewPropertyDecleration);

            foreach (var viewPropertyData in propertyGroup)
            {
                dirtyCondition.TrueStatements.Add(
                    new CodeAssignStatement(new CodeSnippetExpression(viewPropertyData.Expression),
                        new CodeSnippetExpression(string.Format("{0}.{1}", element.Name, viewPropertyData.NameAsProperty))));

                statements.Add(new CodeAssignStatement(new CodeSnippetExpression(string.Format("{0}.{1}", element.Name, viewPropertyData.NameAsProperty)),
                    new CodeSnippetExpression(viewPropertyData.Expression)));
            }

        }
        updateMethod.Statements.Add(new CodeSnippetExpression(string.Format("{0}.Dirty = false", element.Name)));

        return updateMethod;
    }
    public void AddViewBase(ElementData data, string className = null, string baseClassName = null)
    {
        Decleration = new CodeTypeDeclaration(className ?? data.NameAsViewBase)
        {
            TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public
        };

        if (data.IsDerived)
        {
            try
            {
                var baseType = DiagramData.GetAllElements().First(p => p.Name == data.BaseTypeShortName);
                Decleration.BaseTypes.Add(new CodeTypeReference(baseClassName ?? baseType.NameAsViewBase));
            }
            catch (Exception ex)
            {
                data.BaseTypeName = null;
                Decleration.BaseTypes.Add(new CodeTypeReference(uFrameEditor.uFrameTypes.ViewBase));
                Debug.Log(data.BaseTypeName);
                Debug.Log(data.BaseTypeShortName);
            }
        }
        else
        {
            Decleration.BaseTypes.Add(new CodeTypeReference(uFrameEditor.uFrameTypes.ViewBase));
        }

        if (IsDesignerFile)
        {
            Decleration.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(uFrameEditor.uFrameTypes.DiagramInfoAttribute),
                new CodeAttributeArgument(new CodePrimitiveExpression(DiagramData.Name))));
            if (data.BaseElement == null)
            {
                var defaultIdentifierProperty = new CodeMemberProperty()
                {
                    Name = "DefaultIdentifier",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    Type = new CodeTypeReference(typeof(string))
                };
                defaultIdentifierProperty.GetStatements.Add(
                    new CodeMethodReturnStatement(new CodePrimitiveExpression(data.Name)));
                Decleration.Members.Add(defaultIdentifierProperty);
            }

        }

        AddViewModelTypeProperty(data);

        AddComponentReferences(Decleration, data);

        AddMultiInstanceProperty(data);

        AddViewModelProperty(data);

        AddCreateModelMethod(data);

        AddInitializeViewModelMethod(data);

        AddExecuteMethods(data, Decleration);


        foreach (var viewBindingExtender in BindingExtenders)
        {
            viewBindingExtender.ExtendViewBase(Decleration, data);
        }

        Namespace.Types.Add(Decleration);
    }

    protected void AddInitializeViewModelMethod(ElementData data)
    {
        var initializeViewModelMethod = new CodeMemberMethod
        {
            Name = "InitializeViewModel",
            Attributes = MemberAttributes.Override | MemberAttributes.Family
        };

        initializeViewModelMethod.Parameters.Add(
            new CodeParameterDeclarationExpression(new CodeTypeReference(uFrameEditor.uFrameTypes.ViewModel), "viewModel"));

        if (data.IsDerived)
        {
            var baseCall = new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(),
                initializeViewModelMethod.Name);
            baseCall.Parameters.Add(new CodeVariableReferenceExpression("viewModel"));
            initializeViewModelMethod.Statements.Add(baseCall);
        }

        if (data.Properties.Count > 0)
            initializeViewModelMethod.Statements.Add(
                new CodeVariableDeclarationStatement(
                    new CodeTypeReference(data.NameAsViewModel), data.NameAsVariable, new CodeCastExpression(
                        new CodeTypeReference(data.NameAsViewModel),
                        new CodeVariableReferenceExpression("viewModel"))));
        Decleration.Members.Add(initializeViewModelMethod);

        //}
        //else
        //{
        //    createModelMethod.Statements.Add(
        //        new CodeMethodReturnStatement(
        //            new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "ResolveViewModel",
        //                new CodeSnippetExpression(string.Format("GameManager.Container.Resolve<{0}>()",
        //                data.NameAsController)))));
        //}


        foreach (var property in data.Properties)
        {
            var relatedViewModel = DiagramData.GetViewModel(property.RelatedTypeName);
            if (relatedViewModel == null) // Non ViewModel Properties
            {
                var field = new CodeMemberField(new CodeTypeReference(property.RelatedTypeName),
                    property.ViewFieldName) { Attributes = MemberAttributes.Public };
                field.CustomAttributes.Add(
                    new CodeAttributeDeclaration(new CodeTypeReference(uFrameEditor.uFrameTypes.UFGroup),
                        new CodeAttributeArgument(new CodePrimitiveExpression("View Model Properties"))));
                field.CustomAttributes.Add(
                    new CodeAttributeDeclaration(new CodeTypeReference(typeof(HideInInspector))));
                Decleration.Members.Add(field);

                initializeViewModelMethod.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(
                    new CodeVariableReferenceExpression(data.NameAsVariable), property.Name),
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), property.ViewFieldName)));
            }
            else // ViewModel Properties
            {
                var field = new CodeMemberField(new CodeTypeReference(uFrameEditor.uFrameTypes.ViewBase),
                    property.ViewFieldName) { Attributes = MemberAttributes.Public };

                field.CustomAttributes.Add(
                    new CodeAttributeDeclaration(new CodeTypeReference(uFrameEditor.uFrameTypes.UFGroup),
                        new CodeAttributeArgument(new CodePrimitiveExpression("View Model Properties"))));
                field.CustomAttributes.Add(
                    new CodeAttributeDeclaration(new CodeTypeReference(typeof(HideInInspector))));
                Decleration.Members.Add(field);
                initializeViewModelMethod.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(
                    new CodeVariableReferenceExpression(data.NameAsVariable), property.Name),
                    new CodeSnippetExpression(string.Format("this.{0} == null ? null : this.{0}.ViewModelObject as {1}",
                        property.ViewFieldName, relatedViewModel.NameAsViewModel))));
            }
        }
    }

    private void AddCreateModelMethod(ElementData data)
    {
        var createModelMethod = new CodeMemberMethod()
        {
            Name = "CreateModel",
            Attributes = MemberAttributes.Public | MemberAttributes.Override,
            ReturnType = new CodeTypeReference(uFrameEditor.uFrameTypes.ViewModel)
        };

        //if (data.IsMultiInstance)
        //{
        createModelMethod.Statements.Add(
            new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(),
                "RequestViewModel",
                new CodeSnippetExpression(string.Format("GameManager.Container.Resolve<{0}>()",
                    data.NameAsController)))));

        Decleration.Members.Add(createModelMethod);
    }

    public void AddViewModelProperty(ElementData data)
    {
        var viewModelProperty = new CodeMemberProperty
        {
            Name = data.Name,
            Attributes = MemberAttributes.Public | MemberAttributes.Final,
            Type = new CodeTypeReference(data.NameAsViewModel)
        };
        viewModelProperty.GetStatements.Add(
            new CodeMethodReturnStatement(new CodeCastExpression(new CodeTypeReference(data.NameAsViewModel),
                new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "ViewModelObject"))));
        viewModelProperty.SetStatements.Add(
            new CodeAssignStatement(
                new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "ViewModelObject"),
                new CodePropertySetValueReferenceExpression()));

        Decleration.Members.Add(viewModelProperty);
    }

    protected void AddMultiInstanceProperty(ElementData data)
    {
        var multiInstanceProperty = new CodeMemberProperty
        {
            Attributes = MemberAttributes.Override | MemberAttributes.Public,
            Type = new CodeTypeReference(typeof(bool)),
            Name = "IsMultiInstance",
            HasSet = false,
            HasGet = true
        };

        multiInstanceProperty.GetStatements.Add(
            new CodeSnippetExpression(string.Format("return {0}", data.IsMultiInstance ? "true" : "false")));
        Decleration.Members.Add(multiInstanceProperty);
    }

    protected void AddViewModelTypeProperty(ElementData data)
    {
        var viewModelTypeProperty = new CodeMemberProperty
        {
            Attributes = MemberAttributes.Override | MemberAttributes.Public,
            Type = new CodeTypeReference(typeof(Type)),
            Name = "ViewModelType"
        };
        viewModelTypeProperty.HasSet = false;
        viewModelTypeProperty.HasGet = true;
        viewModelTypeProperty.GetStatements.Add(
            new CodeSnippetExpression(string.Format("return typeof({0})", data.NameAsViewModel)));
        Decleration.Members.Add(viewModelTypeProperty);
    }

    public void CreateViewPropertyBinding(string modelName, CodeStatementCollection statements,
        ViewModelPropertyData propertyData)
    {
        statements.Add(
            new CodeSnippetExpression(string.Format("this.BindToView(() => {0}.{1}, v => {2} = v, () => {2})",
                modelName, propertyData.FieldName,
                propertyData.ViewFieldName)));
    }

    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        BindingExtenders = uFrameEditor.Container.ResolveAll<ViewBindingExtender>().Where(p => p.Initialize(this)).ToList();
        Namespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
    }

    protected void AddExecuteMethods(ElementData data, CodeTypeDeclaration decl, bool useViewReference = false)
    {
        foreach (var viewModelCommandData in data.Commands)
        {
            var executeMethod = CreateExecuteMethod(data, useViewReference, viewModelCommandData);
            decl.Members.Add(executeMethod);
        }
    }

    protected CodeMemberMethod CreateExecuteMethod(ElementData data, bool useViewReference,
        ViewModelCommandData viewModelCommandData)
    {
        ElementDataBase relatedElement;
        return CreateExecuteMethod(data, useViewReference, viewModelCommandData, out relatedElement);
    }

    protected CodeMemberMethod CreateExecuteMethod(ElementData data, bool useViewReference,
        ViewModelCommandData viewModelCommandData, out ElementDataBase relatedElement)
    {
        var executeMethod = new CodeMemberMethod
        {
            Name = viewModelCommandData.NameAsExecuteMethod,
            Attributes = MemberAttributes.Public
        };

        CodeExpression executeCommandReference = new CodeThisReferenceExpression();
        if (useViewReference)
            executeCommandReference = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "View");

        relatedElement = DiagramData.GetElement(viewModelCommandData);

        if (relatedElement == null)
        {
            if (viewModelCommandData.RelatedTypeName != null)
            {
                executeMethod.Parameters.Add(new CodeParameterDeclarationExpression(
                    viewModelCommandData.RelatedTypeName, "arg"));

                executeMethod.Statements.Add(new CodeMethodInvokeExpression(
                    executeCommandReference, "ExecuteCommand",
                    new CodeSnippetExpression(string.Format("{0}.{1}", data.Name, viewModelCommandData.Name)),
                    new CodeVariableReferenceExpression("arg")
                    ));
            }
            else
            {
                executeMethod.Statements.Add(new CodeMethodInvokeExpression(
                    executeCommandReference, "ExecuteCommand",
                    new CodeSnippetExpression(string.Format("{0}.{1}", data.Name, viewModelCommandData.Name))
                    ));
            }
        }
        else
        {
            executeMethod.Parameters.Add(new CodeParameterDeclarationExpression(
                relatedElement.NameAsViewModel, relatedElement.NameAsVariable));

            executeMethod.Statements.Add(new CodeMethodInvokeExpression(
                executeCommandReference, "ExecuteCommand",
                new CodeSnippetExpression(string.Format("{0}.{1}", data.Name, viewModelCommandData.Name)),
                new CodeVariableReferenceExpression(relatedElement.NameAsVariable)
                ));
        }
        return executeMethod;
    }

    private void AddCollectionBinding(CodeTypeDeclaration decl, string modelName, CodeStatementCollection statements, ViewModelCollectionData collectionProperty, ElementDataBase relatedElement)
    {
        var varTypeName = relatedElement == null ? collectionProperty.RelatedTypeName : relatedElement.NameAsViewModel;
        var parameterTypeName = relatedElement == null ? collectionProperty.RelatedTypeName : relatedElement.NameAsViewBase;
        var varName = relatedElement == null ? "item" : relatedElement.NameAsVariable;

        var addHandlerMethod = new CodeMemberMethod()
        {
            Attributes = MemberAttributes.Public,
            Name = collectionProperty.Name + "Added",
        };
        addHandlerMethod.Parameters.Add(new CodeParameterDeclarationExpression(parameterTypeName, varName));

        decl.Members.Add(addHandlerMethod);
        var removeHandlerMethod = new CodeMemberMethod()
        {
            Attributes = MemberAttributes.Public,
            Name = collectionProperty.Name + "Removed"
        };
        removeHandlerMethod.Parameters.Add(new CodeParameterDeclarationExpression(parameterTypeName, varName));
        if (relatedElement != null)
        {
            var listField = AddPropertyBindingField(decl, uFrameEditor.uFrameTypes.ListOfViewModel.FullName.Replace("ViewModel", relatedElement.NameAsViewBase), collectionProperty.Name, "List", true);
            addHandlerMethod.Statements.Add(new CodeMethodInvokeExpression(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), listField.Name), "Add",
                new CodeVariableReferenceExpression(relatedElement.NameAsVariable)));
            removeHandlerMethod.Statements.Add(new CodeMethodInvokeExpression(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), listField.Name), "Remove",
                new CodeVariableReferenceExpression(relatedElement.NameAsVariable)));
            removeHandlerMethod.Statements.Add(new CodeSnippetExpression(string.Format("UnityEngine.Object.Destroy({0}.gameObject)", relatedElement.NameAsVariable)));
        }

        decl.Members.Add(removeHandlerMethod);

        if (relatedElement != null && DiagramData.Settings.GenerateDefaultBindings)
        {
            var createHandlerMethod = new CodeMemberMethod()
            {
                Attributes = MemberAttributes.Public,
                Name = collectionProperty.NameAsCreateHandler,
                ReturnType = new CodeTypeReference(uFrameEditor.uFrameTypes.ViewBase)
            };


            createHandlerMethod.Parameters.Add(new CodeParameterDeclarationExpression(varTypeName, varName));
            if (DiagramData.Settings.GenerateDefaultBindings)
            {
                createHandlerMethod.Statements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "InstantiateView", new CodeVariableReferenceExpression(varName))));
            }

            decl.Members.Add(createHandlerMethod);

            statements.Add(
                new CodeSnippetExpression(string.Format("var binding = this.BindToViewCollection(() => {0}.{1})", modelName,
                    collectionProperty.FieldName)));

            statements.Add(
                new CodeSnippetExpression(string.Format("binding.SetAddHandler(item=>{0}(item as {1}))",
                    addHandlerMethod.Name, relatedElement.NameAsViewBase)));
            statements.Add(
                new CodeSnippetExpression(string.Format("binding.SetRemoveHandler(item=>{0}(item as {1}))",
                    removeHandlerMethod.Name, relatedElement.NameAsViewBase)));
            statements.Add(
                new CodeSnippetExpression(
                    string.Format("binding.SetCreateHandler(viewModel=>{{ return {0}(viewModel as {1}); }}); ",
                        createHandlerMethod.Name, relatedElement.NameAsViewModel)));
        }
        else
        {
            statements.Add(
                new CodeSnippetExpression(string.Format("var binding = this.BindCollection(() => {0}.{1})", modelName,
                    collectionProperty.FieldName)));

            statements.Add(
                new CodeSnippetExpression(string.Format("binding.SetAddHandler({0})",
                    addHandlerMethod.Name)));
            statements.Add(
                new CodeSnippetExpression(string.Format("binding.SetRemoveHandler({0})",
                    removeHandlerMethod.Name)));
        }

        if (relatedElement != null)
        {
            var sceneFirstField = AddPropertyBindingField(decl, typeof(bool).FullName, collectionProperty.Name, "SceneFirst");
            //sceneFirstField.CustomAttributes.Add(
            //new CodeAttributeDeclaration(new CodeTypeReference(typeof(UFRequireInstanceMethod)),
            //    new CodeAttributeArgument(new CodePrimitiveExpression(collectionProperty.NameAsCreateHandler))));

            var containerField = AddPropertyBindingField(decl, typeof(Transform).FullName, collectionProperty.Name, "Container");
            //containerField.CustomAttributes.Add(
            //   new CodeAttributeDeclaration(new CodeTypeReference(typeof(UFRequireInstanceMethod)),
            //       new CodeAttributeArgument(new CodePrimitiveExpression(collectionProperty.NameAsCreateHandler))));
            var containerNullCondition =
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(containerField.Name),
                        CodeBinaryOperatorType.ValueEquality, new CodeSnippetExpression("null")));

            containerNullCondition.FalseStatements.Add(new CodeSnippetExpression(string.Format("binding.SetParent({0})", containerField.Name)));
            statements.Add(containerNullCondition);

            var sceneFirstCondition =
                new CodeConditionStatement(
                    new CodeVariableReferenceExpression(sceneFirstField.Name));

            sceneFirstCondition.TrueStatements.Add(new CodeSnippetExpression("binding.ViewFirst()"));
            statements.Add(sceneFirstCondition);
        }

        //this.BindToViewCollection(() => Model._WeaponsProperty)
        //            .SetAddHandler((weapon) =>
        //            {
        //                // set the rotation to the camera rotation
        //                weapon.transform.localPosition = Vector3.zero;
        //                weapon.transform.localRotation = Quaternion.identity;
        //                _Weapons.Add(weapon);
        //            })
        //            .SetParent(_GunsTransform).ViewFirst();
    }

    protected void AddComponentReferences(CodeTypeDeclaration decl, ElementData data)
    {
        if (data.IsDerived) return;

        foreach (var viewComponentData in data.ViewComponents)
        {
            var backingField = new CodeMemberField(viewComponentData.Name, "_" + viewComponentData.Name);
            backingField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializeField))));
            var property = new CodeMemberProperty()
            {
                Type = new CodeTypeReference(viewComponentData.Name),
                Name = viewComponentData.Name,
                Attributes = MemberAttributes.Public,
                HasGet = true,
                HasSet = true
            };

            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression(
                string.Format("{0} ?? ({0} = GetComponent<{1}>())", backingField.Name, viewComponentData.Name))));
            property.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingField.Name),
                    new CodePropertySetValueReferenceExpression()));
            decl.Members.Add(backingField);
            decl.Members.Add(property);
        }
    }

    public Dictionary<string, CodeConditionStatement> BindingConditionStatements
    {
        get { return _bindingConditionStatements ?? (_bindingConditionStatements = new Dictionary<string, CodeConditionStatement>()); }
        set { _bindingConditionStatements = value; }
    }

    protected void GenerateBindingMembers(CodeTypeDeclaration decl, ElementData data, bool isOverride = false)
    {
        var bindingGenerators = uFrameEditor.GetBindingGeneratorsFor(data, isOverride: isOverride, generateDefaultBindings: false).ToArray();
        foreach (var bindingGenerator in bindingGenerators)
        {
            bindingGenerator.CreateMembers(decl.Members);
        }
        if (!data.IsDerived)
        {
            var bindMethod = new CodeMemberMethod
            {
                Name = "Bind",
                Attributes = MemberAttributes.Public | MemberAttributes.Override
            };
            decl.Members.Add(bindMethod);
            //bindMethod.Statements.Add(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "PreBind"));
        }

    }
    protected void GenerateBindMethod(CodeTypeDeclaration decl, ViewData data)
    {

        var preBindMethod = new CodeMemberMethod
        {
            Name = "Bind",
            Attributes = MemberAttributes.Public | MemberAttributes.Override
        };

        preBindMethod.Statements.Add(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "Bind"));

        var bindingGenerators = uFrameEditor.GetBindingGeneratorsFor(data.ViewForElement, isOverride: false, generateDefaultBindings: DiagramData.Settings.GenerateDefaultBindings, includeBaseItems: data.BaseView == null).ToArray();

        foreach (var bindingGenerator in bindingGenerators)
        {
            if (data.BindingMethods.All(p => p.Name != bindingGenerator.MethodName) &&
                data.NewBindings.All(p => p.MethodName != bindingGenerator.MethodName)) continue;

            CodeConditionStatement bindingCondition = null;
            if (this.BindingConditionStatements.ContainsKey(bindingGenerator.BindingConditionFieldName))
            {
                bindingCondition = this.BindingConditionStatements[bindingGenerator.BindingConditionFieldName];
            }
            else
            {
                bindingCondition = AddBindingCondition(decl, preBindMethod.Statements, bindingGenerator.Item,
                   bindingGenerator.Item.RelatedNode() as ElementData);
                BindingConditionStatements.Add(bindingGenerator.BindingConditionFieldName, bindingCondition);
            }
            //if (HasField(Decleration.Members, bindingGenerator.BindingConditionFieldName)) continue;

            bindingGenerator.CreateBindingStatement(decl.Members, bindingCondition);
        }
        decl.Members.Add(preBindMethod);
        //foreach (var property in data.Properties)
        //{
        //    var relatedElement = DiagramData.GetAllElements().FirstOrDefault(p => p.Name == property.RelatedTypeName);
        //    var bindingCondition = AddBindingCondition(decl, bindMethod.Statements, property, relatedElement);

        //    //var twoWayField = AddPropertyBindingField(decl, typeof(bool).FullName, property.Name, "IsTwoWay");
        //    //twoWayField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(UFRequireInstanceMethod)),
        //    //    new CodeAttributeArgument(new CodePrimitiveExpression(property.NameAsTwoWayMethod))));

        //    //var twoWayCondition =
        //    //    new CodeConditionStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),
        //    //        twoWayField.Name));

        //    //bindingCondition.TrueStatements.Add(twoWayCondition);

        //    //AddPropertyBinding(decl, data.Name, bindingCondition.FalseStatements, property, false, relatedElement);
        //    AddPropertyBinding(data, decl, bindingCondition.TrueStatements, property, false, relatedElement);
        //}
        //foreach (var collectionProperty in data.Collections)
        //{
        //    var relatedElement = DiagramData.GetAllElements().FirstOrDefault(p => p.Name == collectionProperty.RelatedTypeName);
        //    var bindingCondition = AddBindingCondition(decl, bindMethod.Statements, collectionProperty, relatedElement);

        //    if (relatedElement == null)
        //    {
        //        AddCollectionBinding(decl, data.Name, bindingCondition.TrueStatements, collectionProperty, relatedElement);
        //    }
        //    else
        //    {
        //        AddCollectionBinding(decl, data.Name, bindingCondition.TrueStatements, collectionProperty, relatedElement);
        //    }
        //}

    }
}