using Invert.uFrame.Editor;
using System;
using System.CodeDom;
using System.Collections.Generic;
using UnityEngine;

public class ViewModelGenerator : CodeGenerator
{
    public static Dictionary<Type, string> AcceptableTypes = new Dictionary<Type, string>
    {
        {typeof(int),"Int" },
        {typeof(Vector3),"Vector3" },
        {typeof(Vector2),"Vector2" },
        {typeof(string),"String" },
        {typeof(bool),"Bool" },
        {typeof(float),"Float" },
        {typeof(double),"Double" },
        {typeof(Quaternion),"Quaternion" },
    };

    public ElementData Data
    {
        get;
        set;
    }

    public CodeTypeDeclaration Decleration { get; set; }

    public IElementDesignerData DiagramData
    {
        get;
        set;
    }

    public ViewModelGenerator(bool isDesignerFile, ElementData data)
    {
        IsDesignerFile = isDesignerFile;
        Data = data;
    }

    public virtual void AddViewModel(ElementData data)
    {
        Decleration = new CodeTypeDeclaration(data.NameAsViewModel);

        if (IsDesignerFile)
        {
            Decleration.BaseTypes.Add(string.Format("{0}ViewModel", data.BaseTypeShortName.Replace("ViewModel", "")));
            Decleration.CustomAttributes.Add(
                new CodeAttributeDeclaration(new CodeTypeReference(typeof(DiagramInfoAttribute)),
                    new CodeAttributeArgument(new CodePrimitiveExpression(DiagramData.Name))));
        }

        Decleration.IsPartial = true;
        if (IsDesignerFile)
        {
            // Now Generator code here
            foreach (var viewModelPropertyData in data.Properties)
            {
                Decleration.Members.Add(ToCodeMemberField(viewModelPropertyData));
                Decleration.Members.Add(ToCodeMemberProperty(viewModelPropertyData));
            }
            foreach (var viewModelPropertyData in data.Collections)
            {
                Decleration.Members.Add(ToCollectionCodeMemberField(viewModelPropertyData));
                Decleration.Members.Add(ToCollectionCodeMemberProperty(viewModelPropertyData));
            }
            foreach (var viewModelPropertyData in data.Commands)
            {
                Decleration.Members.Add(ToCommandCodeMemberField(viewModelPropertyData));
                Decleration.Members.Add(ToCommandCodeMemberProperty(viewModelPropertyData));
            }
            AddWriteMethod(data);
        }

        Namespace.Types.Add(Decleration);
    }

    public virtual void AddWriteMethod(ElementData data)
    {
        var readMethod = new CodeMemberMethod()
        {
            Name = "Read",
            Attributes = MemberAttributes.Override | MemberAttributes.Public
        };
        readMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ISerializerStream), "stream"));

        var writeMethod = new CodeMemberMethod()
        {
            Name = "Write",
            Attributes = MemberAttributes.Override | MemberAttributes.Public
        };
        writeMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ISerializerStream), "stream"));
        writeMethod.Statements.Add(new CodeSnippetStatement("\t\tbase.Write(stream);"));
        readMethod.Statements.Add(new CodeSnippetStatement("\t\tbase.Read(stream);"));

        foreach (var viewModelPropertyData in data.Properties)
        {
            if (viewModelPropertyData.IsEnum(data.OwnerData))
            {
                var statement = new CodeSnippetStatement(string.Format("\t\tstream.SerializeInt(\"{0}\", (int)this.{0});", viewModelPropertyData.Name));
                writeMethod.Statements.Add(statement);

                var dstatement = new CodeSnippetStatement(string.Format("\t\tthis.{0} = ({1})stream.DeserializeInt(\"{0}\");", viewModelPropertyData.Name, viewModelPropertyData.RelatedTypeName));
                readMethod.Statements.Add(dstatement);
            }
            else
            {
                if (viewModelPropertyData.Type == null) continue;
                if (!AcceptableTypes.ContainsKey(viewModelPropertyData.Type)) continue;
                //viewModelPropertyData.IsEnum(data.OwnerData);
                var statement = new CodeSnippetStatement(string.Format("\t\tstream.Serialize{0}(\"{1}\", this.{1});", AcceptableTypes[viewModelPropertyData.Type], viewModelPropertyData.Name));
                writeMethod.Statements.Add(statement);

                var dstatement = new CodeSnippetStatement(string.Format("\t\tthis.{0} = stream.Deserialize{1}(\"{0}\");", viewModelPropertyData.Name, AcceptableTypes[viewModelPropertyData.Type]));
                readMethod.Statements.Add(dstatement);    
            }
            
        }

        Decleration.Members.Add(writeMethod);
        Decleration.Members.Add(readMethod);
    }

    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        AddViewModel(Data);
    }

    public virtual CodeMemberField ToCodeMemberField(ViewModelPropertyData itemData)
    {
        var field = new CodeMemberField { Name = itemData.FieldName };

        field.Attributes = MemberAttributes.Public;

        var typeViewModel = DiagramData.GetViewModel(itemData.RelatedTypeName);
        var relatedType = typeViewModel == null ? itemData.RelatedTypeName : typeViewModel.NameAsViewModel;

        field.Type = new CodeTypeReference(string.Format("readonly P<{0}>", relatedType));
        var t = new CodeTypeReference(typeof(P<>));
        t.TypeArguments.Add(new CodeTypeReference(relatedType));
        field.InitExpression = new CodeObjectCreateExpression(t);
        return field;
    }

    public virtual CodeMemberProperty ToCodeMemberProperty(ViewModelPropertyData itemData)
    {
        var property = new CodeMemberProperty();
        property.Name = itemData.Name;
        property.Attributes = MemberAttributes.Public;

        var typeViewModel = DiagramData.GetViewModel(itemData.RelatedTypeName);

        if (typeViewModel == null)
        {
            property.Type = new CodeTypeReference(itemData.RelatedTypeName);
        }
        else
        {
            property.Type = new CodeTypeReference(typeViewModel.NameAsViewModel);
        }
        property.GetStatements.Add(
            new CodeMethodReturnStatement(new CodeSnippetExpression(string.Format("{0}.Value", itemData.FieldName))));

        property.SetStatements.Add(new CodeSnippetExpression(string.Format("{0}.Value = value", itemData.FieldName)));

        return property;
    }

    public virtual CodeMemberField ToCollectionCodeMemberField(ViewModelCollectionData itemData)
    {
        var field = new CodeMemberField { Name = itemData.FieldName };

        field.Attributes = MemberAttributes.Public;
        var typeViewModel = DiagramData.GetViewModel(itemData.RelatedTypeName);

        var relatedType = typeViewModel == null ? itemData.RelatedTypeName : typeViewModel.NameAsViewModel;

        field.Type = new CodeTypeReference(string.Format("readonly ModelCollection<{0}>", relatedType));

        var t = new CodeTypeReference(typeof(ModelCollection<>));
        t.TypeArguments.Add(new CodeTypeReference(relatedType));
        field.InitExpression = new CodeObjectCreateExpression(t);

        return field;
    }

    public virtual CodeMemberProperty ToCollectionCodeMemberProperty(ViewModelCollectionData itemData)
    {
        var property = new CodeMemberProperty();
        property.Name = itemData.Name;
        property.Type = new CodeTypeReference(typeof(ICollection<>));
        property.Attributes = MemberAttributes.Public;
        var typeViewModel = DiagramData.GetViewModel(itemData.RelatedTypeName);
        if (typeViewModel == null)
        {
            property.Type.TypeArguments.Add(itemData.RelatedTypeName);
        }
        else
        {
            property.Type.TypeArguments.Add(new CodeTypeReference(typeViewModel.NameAsViewModel));
        }

        property.GetStatements.Add(
            new CodeMethodReturnStatement(new CodeSnippetExpression(string.Format("{0}", itemData.FieldName))));

        property.SetStatements.Add(new CodeSnippetExpression(string.Format("{0}.Value = value.ToList()", itemData.FieldName)));

        return property;
    }

    public virtual CodeMemberField ToCommandCodeMemberField(ViewModelCommandData itemData)
    {
        var property = new CodeMemberField();
        property.Name = itemData.FieldName;
        property.Type = new CodeTypeReference(typeof(ICommand));
        return property;
    }

    public virtual CodeMemberProperty ToCommandCodeMemberProperty(ViewModelCommandData itemData)
    {
        var property = new CodeMemberProperty
        {
            Name = itemData.Name,
            Attributes = MemberAttributes.Public,
            Type = new CodeTypeReference(typeof(ICommand))
        };

        property.GetStatements.Add(
            new CodeMethodReturnStatement(new CodeSnippetExpression(string.Format("{0}", itemData.FieldName))));

        property.SetStatements.Add(new CodeSnippetExpression(string.Format("{0} = value", itemData.FieldName)));
        return property;
    }
}