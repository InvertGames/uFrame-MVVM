//using System;
//using System.Linq;
//using Invert.uFrame.Editor.ElementDesigner;
//using UnityEditor;
//using UnityEngine;

//public class ImportCommand : ElementsDiagramToolbarCommand
//{
//    public override ToolbarPosition Position
//    {
//        get { return ToolbarPosition.BottomRight; }
//    }

//    public override void Perform(ElementsDiagram diagram)
//    {
//        var typesList = ActionSheetHelpers.GetDerivedTypes<ViewModel>(false, false).ToList();
//        typesList.AddRange(ActionSheetHelpers.GetDerivedTypes<Enum>(false, false));
//        typesList.AddRange(ActionSheetHelpers.GetDerivedTypes<SceneManager>(false, false));
//        typesList.AddRange(ActionSheetHelpers.GetDerivedTypes<ViewBase>(false, false));

//        ImportTypeListWindow.InitTypeListWindow("Choose Type", typesList, (item) =>
//        {
//            if (IsImportOnly(item))
//            {
//                EditorUtility.DisplayDialog("Can't do that", String.Format("Can't import {0} because it already belongs to another diagram.", item.FullName), "OK");

//                return;
//            }

//            var result = ImportType(item, diagram.Data);
//            if (result != null)
//            {
//                result.Data = diagram.Data;
//                if (diagram.Data.CurrentFilter.IsAllowed(result, item))
//                {
//                    diagram.Data.CurrentFilter.Locations[result] = new Vector2(15f, 15f);
//                }
//            }
//            diagram.Refresh();
//        });
//    }

//    public DiagramNode ImportType(Type type, IElementDesignerData diagramData)
//    {
//        if (type.IsEnum)
//        {
//            var enumData = new EnumData { Name = type.Name, Data = diagramData };
//            //var enumValues = Enum.GetValues(item);
//            var enumNames = Enum.GetNames(type);
//            for (var i = 0; i < enumNames.Length; i++)
//            {
//                enumData.EnumItems.Add(new EnumItem()
//                {
//                    Name = enumNames[i]
//                });
//            }
//            diagramData.AddNode(enumData);
//            return enumData;
//        }
//        if (typeof(ViewModel).IsAssignableFrom(type))
//        {
//            var vm = GetViewModelFromType(type, diagramData);
//            //if (vm is ImportedElementData)
//            //{
//            //    diagramData.ImportedElements.Add(vm as ImportedElementData);
//            //}
//            //else
//            //{
//                diagramData.AddNode(vm as ElementData);
//            //}
//            return vm;
//        }
//        if (typeof(ViewBase).IsAssignableFrom(type))
//        {
//            var view = new ViewData();
//            view.Name = type.Name;
//            view.Data = diagramData;
//            if (type.BaseType != null)
//                if (type.BaseType.AssemblyQualifiedName != null)
//                    view.ForAssemblyQualifiedName = type.BaseType.AssemblyQualifiedName.Replace("ViewModel", "");

//            diagramData.AddNode(view);
//            return view;
//        }
//        if (typeof(SceneManager).IsAssignableFrom(type))
//        {
//            var sceneManager = new SceneManagerData();
//            sceneManager.Name = type.Name;
//            sceneManager.Data = diagramData;
//            diagramData.AddNode(sceneManager);

//            return sceneManager;
//        }
//        return null;
//    }

//    public ElementDataBase GetViewModelFromType(Type type, IElementDesignerData diagramData)
//    {
//        ElementDataBase vmData = new ElementData()
//        {
//            Data = diagramData,
//            Location = new Vector2(15f, 15f)
//        };

//        FillElementData(type, vmData);
//        return vmData;
//    }

//    public static string ConvertType(Type type)
//    {
//        if (typeof(ViewModel).IsAssignableFrom(type))
//        {
//            if (type == typeof(ViewModel))
//            {
//                return UFrameAssetManager.DesignerVMAssemblyName;
//            }
//            var assemblyQualifiedName = UFrameAssetManager.DesignerVMAssemblyName;
//            if (assemblyQualifiedName != null)
//                return assemblyQualifiedName.Replace("ViewModel", type.Name.Replace("ViewModel", ""));
//        }
//        return type == null ? null : type.AssemblyQualifiedName;
//    }

//    protected void FillElementData(Type type, ElementDataBase vmData)
//    {
//        vmData.Dirty = true;
//        vmData.BaseTypeName = ConvertType(type.BaseType);
//        vmData.Name = type.Name.Replace("ViewModel", "");
//        var vProperties = ViewModel.GetReflectedModelProperties(type).Where(p => p.Value.DeclaringType == type);

//        foreach (var property in vProperties)
//        {
//            if (typeof(IModelCollection).IsAssignableFrom(property.Value.FieldType))
//            {
//                var data = new ViewModelCollectionData();
//                data.Name = property.Key.Replace("_", "").Replace("Property", "");
//                data.RelatedType = ConvertType(property.Value.FieldType.GetGenericArguments().First());
//                vmData.Collections.Add(data);
//            }
//            else
//            {
//                var data = new ViewModelPropertyData
//                {
//                    Name = property.Key.Replace("_", "").Replace("Property", ""),
//                    RelatedType = ConvertType(property.Value.FieldType.GetGenericArguments().First()),
//                    DefaultValue = property.Value
//                };
//                vmData.Properties.Add(data);
//            }
//        }

//        var commands = ViewModel.GetReflectedCommands(type).Where(p => p.Value.DeclaringType == type);
//        foreach (var command in commands)
//        {
//            var data = new ViewModelCommandData();
//            data.Name = command.Key;
//            data.RelatedType = ConvertType(command.Value.PropertyType.GetGenericArguments().FirstOrDefault());

//            vmData.Commands.Add(data);
//        }
//    }

//    public bool IsImportOnly(Type type)
//    {
//        if (type == null) return false;
//        var attribute =
//            type.GetCustomAttributes(false).FirstOrDefault();
//        if (attribute != null && attribute.GetType().Name == "DiagramInfoAttribute")
//        {
//            return true;
//        }
//        if (attribute == null)
//            return false;

//        return true;
//    }
//}