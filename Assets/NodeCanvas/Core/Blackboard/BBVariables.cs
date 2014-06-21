using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace NodeCanvas.Variables{

	///Base class for Variables that allow linking to a Blackboard variable or specifying one directly.
	[Serializable]
	abstract public class BBVariable{

		[SerializeField][HideInInspector]
		private Blackboard _bb;
		[SerializeField]
		private string _dataName;
		[SerializeField][HideInInspector]
		private Data _dataRef;
		[SerializeField]
		private bool _useBlackboard = false;
		[SerializeField][HideInInspector]
		private bool _blackboardOnly = false;
		[SerializeField][HideInInspector]
		private bool _isDynamic;


		//To avoid spamming reflection
		private MethodInfo _getMethod;
		private MethodInfo _setMethod;
		//

		///Set the blackboard provided for all BBVariable fields on the object provided
		public static void SetBBFields(Blackboard bb, object o){

			CheckNullBBFields(o);

			foreach (FieldInfo field in o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){

				if (typeof(IList).IsAssignableFrom(field.FieldType)){
					
					var list = field.GetValue(o) as IList;
					if (list == null)
						continue;

					if (typeof(BBVariable).IsAssignableFrom(field.FieldType.GetGenericArguments()[0])){
						foreach(BBVariable bbVar in list)
							bbVar.bb = bb;
					}
				}

				if (typeof(BBVariable).IsAssignableFrom(field.FieldType))
					(field.GetValue(o) as BBVariable).bb = bb;

				if (typeof(BBVariableSet) == field.FieldType)
					(field.GetValue(o) as BBVariableSet).bb = bb;
			}
		}

		///Check for null bb fields and init them if null on provided object
		public static void CheckNullBBFields(object o){
			foreach (FieldInfo field in o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){
				if (typeof(BBVariable).IsAssignableFrom(field.FieldType) && field.GetValue(o) == null)
					field.SetValue(o, Activator.CreateInstance(field.FieldType));
			}
		}

		private Blackboard overrideBB{
			get
			{
				if (string.IsNullOrEmpty(dataName) || !dataName.Contains("/"))
					return null;
				string prefix = dataName.Substring(0, dataName.IndexOf("/"));
				if (Blackboard.allBlackboards.ContainsKey(prefix))
					return Blackboard.allBlackboards[prefix];
				return null;
			}
		}

		private Data dataRef{
			get {return _dataRef;}
			set {_dataRef = value;}
		}

		///The blackboard to read/write from.
		public Blackboard bb{
			get {return _bb;}
			set
			{
				if (_bb != value){
					_bb = value;
					dataRef = null;
				}
			}
		}

		public bool isDynamic{
			get {return _isDynamic;}
			set {_isDynamic = value;}
		}

		///The name of the data to read/write from
		public string dataName{
			get
			{
				if (dataRef != null){
					if (!string.IsNullOrEmpty(_dataName)){
						if (_dataName.Contains("/"))
							return _dataName.Substring(0, _dataName.IndexOf("/")+1) + dataRef.dataName;
					}
					return dataRef.dataName;
				}
				return _dataName;
			}
			set
			{
				if (_dataName != value){
					_dataName = value;
					if (!string.IsNullOrEmpty(value)){
						useBlackboard = true;
						if (overrideBB){
							dataRef = overrideBB.GetData(_dataName.Substring( _dataName.LastIndexOf("/") + 1), dataType);
						} else if (bb){
							dataRef = bb.GetData(_dataName, dataType);
						}
					} else {
						dataRef = null;
					}
				}
			}
		}

		///Should read/write be allows only from the blackboard?
		public bool blackboardOnly{
			get { return _blackboardOnly;}
			set { _blackboardOnly = value; if (value == true) useBlackboard = true;}
		}

		///Are we currently reading/writing from the blackboard or the direct assigned value?
		public bool useBlackboard{
			get { return _useBlackboard;}
			set { _useBlackboard = value; if (value == false) dataName = null; }
		}

		///Is the final value null?
		virtual public bool isNull{
			get {return objectValue == null;}
		}

		///The type of the value that this object has.
		virtual public Type dataType{
			get
			{
				if (_getMethod == null)
					_getMethod = this.GetType().GetMethod("get_value");
				return _getMethod.ReturnType;
			}
		}

		//The System.Object value.
		public object objectValue{
			get
			{
				if (_getMethod == null)
					_getMethod = this.GetType().GetMethod("get_value");
				return _getMethod.Invoke(this, null);
			}
			set
			{
				if (_setMethod == null)
					_setMethod = this.GetType().GetMethod("set_value");
				_setMethod.Invoke(this, new object[]{value});
			}
		}

		public override string ToString(){
			return "'<b>" + (useBlackboard? "$" + dataName : isNull? "NULL" : objectValue.ToString() ) + "</b>'";
		}

		///Read the specified type from the blackboard
		protected T Read<T>(){

			//if (string.IsNullOrEmpty(dataName))
			//	return default(T);

			if (dataRef != null)
				return (T)dataRef.objectValue;

			if (overrideBB != null){
				dataRef = overrideBB.GetData(dataName.Substring( dataName.LastIndexOf("/") + 1), typeof(T) );
				return overrideBB.GetDataValue<T>( dataName.Substring( dataName.LastIndexOf("/") + 1) );
			}

			if (bb != null){
				dataRef = bb.GetData(dataName, typeof(T));
				return bb.GetDataValue<T>(dataName);
			}
			
			return default(T);
		}

		///Write the specified object to the blackboard
		protected void Write(object o){

			//if (string.IsNullOrEmpty(dataName))
			//	return;

			if (dataRef != null){
				dataRef.objectValue = o;
				return;
			}

			if (overrideBB != null){
				dataRef = overrideBB.SetDataValue(dataName.Substring( dataName.LastIndexOf("/") + 1) , o);
				return;
			}

			if (bb != null){
				dataRef = bb.SetDataValue(dataName, o);
				return;
			}

			Debug.LogError("BBVariable has neither linked data, nor blackboard");
		}
	}

	[Serializable]
	public class BBBool : BBVariable{
		
		[SerializeField]
		private bool _value;
		public bool value{
			get {return useBlackboard? Read<bool>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBFloat : BBVariable{

		[SerializeField]
		private float _value;
		public float value{
			get {return useBlackboard? Read<float>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBInt : BBVariable{

		[SerializeField]
		private int _value;
		public int value{
			get {return useBlackboard? Read<int>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBVector : BBVariable{

		[SerializeField]
		private Vector3 _value= Vector3.zero;
		public Vector3 value{
			get {return useBlackboard? Read<Vector3>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBVector2 : BBVariable{

		[SerializeField]
		private Vector2 _value= Vector2.zero;
		public Vector2 value{
			get {return useBlackboard? Read<Vector2>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBColor : BBVariable{

		[SerializeField]
		private Color _value = Color.white;
		public Color value{
			get {return useBlackboard? Read<Color>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBString : BBVariable{

		[SerializeField]
		private string _value = string.Empty;
		public string value{
			get {return useBlackboard? Read<string>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
		public override bool isNull{ get {return string.IsNullOrEmpty(value); }}
	}


	[Serializable]
	public class BBGameObject : BBVariable{

		[SerializeField]
		private GameObject _value;
		public GameObject value{
			get {return useBlackboard? Read<GameObject>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
		public override bool isNull{ get {return (value as GameObject) == null; }}
		public override string ToString(){
			if (useBlackboard) return base.ToString();
			return "'<b>" + (_value != null? _value.name : "NULL") + "</b>'";
		}
	}

	[Serializable]
	public class BBComponent : BBVariable{

		[SerializeField]
		private Component _value;
		[SerializeField]
		private string _typeName;

		public Component value{
			get {return useBlackboard? Read<Component>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
		public override Type dataType{
			get
			{
				if (string.IsNullOrEmpty(_typeName))
					_typeName = typeof(Component).AssemblyQualifiedName;
					
				return type;
			}
		}

		public Type type{
			get {return Type.GetType(_typeName);}
			set {_typeName = value.AssemblyQualifiedName;}
		}

		public override bool isNull{ get {return (value as Component) == null; }}
		public override string ToString(){
			if (useBlackboard) return base.ToString();
			return "'<b>" + (_value != null? _value.name : "NULL") + "</b>'";
		}
	}

	[Serializable]
	public class BBGameObjectList : BBVariable{

		[SerializeField]
		private List<GameObject> _value = new List<GameObject>();
		public List<GameObject> value{
			get {return useBlackboard? Read<List<GameObject>>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
		public override string ToString(){
			if (useBlackboard) return base.ToString();
			return "'<b>" + (_value != null? "List(" + _value.Count.ToString() : "NULL") + ")</b>'";
		}
	}

	///A collection of multiple BBVariables
	[Serializable]
	public class BBVariableSet{

		[SerializeField]
		private string selectedTypeName = null;

		//value set
		[SerializeField]
		private BBBool boolValue           = new BBBool();
		[SerializeField]
		private BBFloat floatValue         = new BBFloat();
		[SerializeField]
		private BBInt intValue             = new BBInt();
		[SerializeField]
		private BBString stringValue       = new BBString();
		[SerializeField]
		private BBVector2 vector2Value     = new BBVector2();
		[SerializeField]
		private BBVector vectorValue       = new BBVector();
		[SerializeField]
		private BBColor colorValue         = new BBColor();
		[SerializeField]
		private BBGameObject goValue       = new BBGameObject();
		[SerializeField]
		private BBComponent componentValue = new BBComponent();
		//

		private List<BBVariable> allVariables{
			get
			{
				return new List<BBVariable>{
					boolValue,
					floatValue,
					intValue,
					stringValue,
					vector2Value,
					vectorValue,
					colorValue,
					goValue,
					componentValue
				};
			}
		}

		public List<Type> availableTypes{
			get
			{
				var typeList = new List<Type>();
				typeList.Add(typeof(void));
				typeList.Add(typeof(Component));
				foreach (BBVariable bbVar in allVariables){
					typeList.Add(bbVar.dataType);
				}
				return typeList;
			}
		}

		public Blackboard bb{
			set
			{
				foreach (BBVariable bbVar in allVariables)
					bbVar.bb = value;
			}
		}

		public bool blackboardOnly{
			set
			{
				foreach (BBVariable bbVar in allVariables)
					bbVar.blackboardOnly = value;
			}
		}

		public Type selectedType{
			get
			{
				foreach(Type t in availableTypes){
					if (selectedTypeName == t.ToString())
						return t;
				}
				return null;
			}
			set
			{
				selectedTypeName = value != null? value.ToString() : null ;
				if (typeof(Component).IsAssignableFrom(value))
					componentValue.type = value;
			}
		}

		public BBVariable selectedBBVariable{
			get
			{
				foreach (BBVariable bbVar in allVariables){
					if (bbVar.dataType == selectedType)
						return bbVar;
				}
				return null;
			}
		}

		public object selectedObjectValue{
			get
			{
				if (selectedType == null)
					return null;
				return selectedBBVariable.objectValue;
			}
			set
			{
				if (selectedType == null)
					return;
				selectedBBVariable.objectValue = value;
			}
		}
	}
}