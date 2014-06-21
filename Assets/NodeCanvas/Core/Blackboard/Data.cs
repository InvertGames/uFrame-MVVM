using UnityEngine;

namespace NodeCanvas.Variables{
	
	///Data are mostly stored in Blackboard. Derived classes of this store the correct type respectively depending on the class
	abstract public class Data : MonoBehaviour{

		public string dataName;

		///The Type this data holds
		virtual public System.Type dataType{
			get {return objectValue.GetType();}
		}

		virtual public object objectValue{
			get {return this.GetType().GetField("value").GetValue(this);}
			set {this.GetType().GetField("value").SetValue(this, value);}
		}

		virtual public object GetSerialized(){
			return objectValue;
		}

		virtual public void SetSerialized(object obj){
			objectValue = obj;
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		virtual public void ShowDataGUI(){
			
			var field = this.GetType().GetField("value");
			field.SetValue(this, EditorUtils.GenericField(dataName, field.GetValue(this), dataType));
			
		}

		#endif
	}
}