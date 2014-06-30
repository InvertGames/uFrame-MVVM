//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//[Serializable]
//public class ImportedElementData : ElementDataBase
//{
//    [SerializeField]
//    private string _typeAssemblyName;
//    [NonSerialized]
//    private List<ViewModelPropertyData> _properties = new List<ViewModelPropertyData>();
//    [NonSerialized]
//    private List<ViewModelCollectionData> _collections = new List<ViewModelCollectionData>();
//    [NonSerialized]
//    private List<ViewModelCommandData> _commands = new List<ViewModelCommandData>();

//    public string TypeAssemblyName
//    {
//        get { return _typeAssemblyName; }
//        set { _typeAssemblyName = value; }
//    }

//    public Type ViewModelType
//    {
//        get { return Type.GetType(TypeAssemblyName); }
//    }

//    public override ICollection<ViewModelPropertyData> Properties
//    {
//        get { return _properties; }
//        set { _properties = value.ToList(); }
//    }

//    public override ICollection<ViewModelCollectionData> Collections
//    {
//        get { return _collections; }
//        set { _collections = value.ToList(); }
//    }

//    public override ICollection<ViewModelCommandData> Commands
//    {
//        get { return _commands; }
//        set { _commands = value.ToList(); }
//    }

//    public override string BaseTypeName
//    {
//        get
//        {
//            var type = Type.GetType(TypeAssemblyName);
//            if (type != null)
//            {
//                var baseType = type.BaseType.Name.Replace("ViewModel","");
//                var assemblyQualifiedName = UFrameAssetManager.DesignerVMAssemblyName;
//                if (assemblyQualifiedName != null)
//                    return assemblyQualifiedName.Replace("ViewModel", baseType);
//            }
//            return null;
//        }
//        set
//        {
//            // Not Allowed
//        }
//    }

    

//    public override string Name
//    {
//        get { return TypeAssemblyName.Split(',').First().Replace("ViewModel",""); }
//        set
//        {
//            // Not allowed
//        }
//    }

//    public override void RemoveFromDiagram()
//    {
//        base.RemoveFromDiagram();
//        Data.ImportedElements.Remove(this);
//    }

//    public override void RemoveLink(IDiagramNode target)
//    {
        
//    }
//}