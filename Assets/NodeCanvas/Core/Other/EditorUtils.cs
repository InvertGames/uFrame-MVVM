using System;

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using NodeCanvas.Variables;

namespace NodeCanvas{

	//Have some commonly stuff used across most inspectors and helper functions. Keep outside of Editor folder since many runtime classes have editor GUIs in #if UNITY_EDITOR
	static public class EditorUtils{

		private static Texture2D _tex;
		private static Texture2D tex{
			get
			{
				if (_tex == null){
					_tex = new Texture2D(1,1);
					_tex.hideFlags = HideFlags.HideAndDontSave;
				}
				return _tex;
			}
		}

		public static GUIContent playIcon = EditorGUIUtility.IconContent("d_PlayButton");
		public static GUIContent pauseIcon = EditorGUIUtility.IconContent("d_PauseButton");
		public static GUIContent stepIcon = EditorGUIUtility.IconContent("d_StepButton");
		public static GUIContent viewIcon = EditorGUIUtility.IconContent("d_ViewToolOrbit On");
		public static GUIContent csIcon = EditorGUIUtility.IconContent("cs Script Icon");
		public static GUIContent jsIcon = EditorGUIUtility.IconContent("Js Script Icon");

		public static Color lightOrange = new Color(1, 0.9f, 0.4f);
		public static Color lightBlue   = new Color(0.8f,0.8f,1);
		public static Color lightRed    = new Color(1,0.5f,0.5f, 0.8f);

		//a cool label :-P
		public static void CoolLabel(string text){

			GUI.skin.label.richText = true;
			GUI.color = lightOrange;
			GUILayout.Label("<b><size=14>" + text + "</size></b>");
			GUI.color = Color.white;
			GUILayout.Space(2);
		}

		//a thin separator
		public static void Separator(){
			
			GUI.backgroundColor = Color.black;
			GUILayout.Box("", GUILayout.MaxWidth(Screen.width), GUILayout.Height(2));
			GUI.backgroundColor = Color.white;
		}

		//A thick separator similar to ngui. Thanks
		public static void BoldSeparator(){

			Rect lastRect= GUILayoutUtility.GetLastRect();

			GUILayout.Space(14);
			GUI.color = new Color(0, 0, 0, 0.25f);
			GUI.DrawTexture(new Rect(0, lastRect.yMax + 6, Screen.width, 4), tex);
			GUI.DrawTexture(new Rect(0, lastRect.yMax + 6, Screen.width, 1), tex);
			GUI.DrawTexture(new Rect(0, lastRect.yMax + 9, Screen.width, 1), tex);
			GUI.color = Color.white;
		}

		public static void TitledSeparator(string title){

			TitledSeparator(title, false);
		}

		//Combines the rest functions for a header style label
		public static void TitledSeparator(string title, bool startOfInspector){

			if (!startOfInspector)
				BoldSeparator();
			else
				EditorGUILayout.Space();

			CoolLabel(title + " ▼");
			Separator();
		}

		//Just a fancy ending for inspectors
		public static void EndOfInspector(){

			Rect lastRect= GUILayoutUtility.GetLastRect();

			GUILayout.Space(8);
			GUI.color = new Color(0, 0, 0, 0.4f);
			GUI.DrawTexture(new Rect(0, lastRect.yMax + 6, Screen.width, 4), tex);
			GUI.DrawTexture(new Rect(0, lastRect.yMax + 4, Screen.width, 1), tex);
			GUI.color = Color.white;
		}

		public static void TextFieldComment(string check, string comment = "Comments..."){
			if (string.IsNullOrEmpty(check)){
				var lastRect = GUILayoutUtility.GetLastRect();
				GUI.color = new Color(1,1,1,0.3f);
				GUI.Label(lastRect, " <i>" + comment + "</i>");
				GUI.color = Color.white;
			}
		}

		//a Custom titlebar for tasks. Returns if the task is folded or not
		public static Rect TaskTitlebar(Task task, bool doHide = false){

			task.hideFlags = doHide? HideFlags.HideInInspector : 0;
			
			GUI.backgroundColor = new Color(0.8f,0.8f,1f,1f);
			GUILayout.BeginHorizontal("box");
			GUI.backgroundColor = Color.white;
			if (GUILayout.Button("X", GUILayout.Width(20))){
				Undo.DestroyObjectImmediate(task);
				return new Rect();
			}

			GUILayout.Label("<b>" + (task.unfolded? "▼ " :"► ") + task.taskName + "</b>" + (task.unfolded? "" : "\n<i><size=10>(" + task.taskInfo + ")</size></i>") );

			if (GUILayout.Button(csIcon, "label", GUILayout.Width(20), GUILayout.Height(20)))
				AssetDatabase.OpenAsset(MonoScript.FromMonoBehaviour(task));

			GUILayout.EndHorizontal();
			var titleRect = GUILayoutUtility.GetLastRect();

			EditorGUIUtility.AddCursorRect(titleRect, MouseCursor.Link);
			var e = Event.current;

			if (e.type == EventType.ContextClick && titleRect.Contains(e.mousePosition)){
				var menu = new GenericMenu();
				menu.AddItem(new GUIContent("Open Script"), false, delegate{AssetDatabase.OpenAsset(MonoScript.FromMonoBehaviour(task)) ;} );
				menu.AddItem(new GUIContent("Copy"), false, delegate{Task.copyTask = task;} );
				menu.AddItem(new GUIContent("Delete"), false, delegate{Undo.DestroyObjectImmediate(task);} );
				menu.ShowAsContext();
				e.Use();
			}

			if (e.button == 0 && e.type == EventType.MouseUp && titleRect.Contains(e.mousePosition)){
				task.unfolded = !task.unfolded;
				e.Use();
			}

			if (task.unfolded){
				task.ShowInspectorGUI();
				//if (task.taskDescription != string.Empty)
				//	EditorGUILayout.HelpBox(task.taskDescription, MessageType.Info);
			}
				
			return titleRect;
		}


		//Show an automatic editor gui for arbitrary objects, taking into account custom attributes
		public static void ShowAutoEditorGUI(object o){

			foreach (FieldInfo field in o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)){

				if (field.GetCustomAttributes(typeof(HideInInspector), true ).FirstOrDefault() as HideInInspector != null)
					continue;

				if (field.GetCustomAttributes(typeof(BeginGroupAttribute), true ).FirstOrDefault() as BeginGroupAttribute != null)
					GUILayout.BeginVertical("box");

				field.SetValue(o, GenericField(field.Name, field.GetValue(o), field.FieldType, field) );
				GUI.backgroundColor = Color.white;

				if (field.GetCustomAttributes(typeof(EndGroupAttribute), true ).FirstOrDefault() as EndGroupAttribute != null)
					GUILayout.EndVertical();
			}

			foreach (PropertyInfo prop in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)){

				if (prop.GetCustomAttributes(typeof(HideInInspector), true ).FirstOrDefault() as HideInInspector != null)
					continue;

				if (!prop.CanRead || !prop.CanWrite)
					continue;

				prop.SetValue(o, GenericField(prop.Name, prop.GetValue(o, null), prop.PropertyType, prop), null);
				GUI.backgroundColor = Color.white;				
			}
		}

		//For generic automatic editors. Passing a MemberInfo will also check for custom attributes
		public static object GenericField(string name, object value, System.Type t, MemberInfo member = null){

			name = CamelCaseToWords(name);

			if (member != null && member.GetCustomAttributes(typeof(RequiredFieldAttribute), true).FirstOrDefault() as RequiredFieldAttribute != null){
				if (t == typeof(string) && string.IsNullOrEmpty((string)value) ||
					typeof(BBVariable).IsAssignableFrom(t) && (value as BBVariable).isNull ||
					typeof(UnityEngine.Object).IsAssignableFrom(t) && (value as UnityEngine.Object) == null)
				{
					GUI.backgroundColor = lightRed;
				}
			}

			if (t == typeof(string)){
				
				if (member != null){
					if (member.GetCustomAttributes(typeof(TagFieldAttribute), true).FirstOrDefault() as TagFieldAttribute != null)
						return EditorGUILayout.TagField(name, (string)value);
					var areaAtt = member.GetCustomAttributes(typeof(TextAreaFieldAttribute), true).FirstOrDefault() as TextAreaFieldAttribute;
					if (areaAtt != null){
						GUILayout.Label(name);
						return EditorGUILayout.TextArea((string)value, GUILayout.Height(areaAtt.height));
					}
				}

				return EditorGUILayout.TextField(name, (string)value);
			}

			if (t == typeof(bool))
				return EditorGUILayout.Toggle(name, (bool)value);

			if (t == typeof(int)){
				
				if (member != null && member.GetCustomAttributes(typeof(LayerFieldAttribute), true).FirstOrDefault() as LayerFieldAttribute != null)
					return EditorGUILayout.LayerField(name, (int)value);

				return EditorGUILayout.IntField(name, (int)value);
			}

			if (t == typeof(float)){
				
				if (member != null){
					var sField = member.GetCustomAttributes(typeof(SliderFieldAttribute), true).FirstOrDefault() as SliderFieldAttribute;
					if (sField != null)
						return EditorGUILayout.Slider(name, (float)value, sField.left, sField.right);
				}

				return EditorGUILayout.FloatField(name, (float)value);
			}

			if (t == typeof(Vector2))
				return EditorGUILayout.Vector2Field(name, (Vector2)value);

			if (t == typeof(Vector3))
				return EditorGUILayout.Vector3Field(name, (Vector3)value);

			if (t == typeof(Vector4))
				return EditorGUILayout.Vector4Field(name, (Vector3)value);

			if (t == typeof(Color))
				return EditorGUILayout.ColorField(name, (Color)value);

			if (t == typeof(Rect))
				return EditorGUILayout.RectField(name, (Rect)value);

			if (t == typeof(AnimationCurve))
				return EditorGUILayout.CurveField(name, (AnimationCurve)value);

			if (t == typeof(Bounds))
				return EditorGUILayout.BoundsField(name, (Bounds)value);

			if (t == typeof(LayerMask))
				return LayerMaskField(name, (LayerMask)value);

			if (typeof(System.Enum).IsAssignableFrom(t))
				return EditorGUILayout.EnumPopup(name, (System.Enum)value);

			if (typeof(BBVariable).IsAssignableFrom(t)){
				if (value == null)
					value = Activator.CreateInstance(t);
				return BBVariableField(name, (BBVariable)value, member);
			}

			if (t == typeof(Component))
				return ComponentField(name, (Component)value);

			if (typeof(UnityEngine.Object).IsAssignableFrom(t))
				return EditorGUILayout.ObjectField(name, (UnityEngine.Object)value, t, true);

			if (t == typeof(List<GameObject>)){
				if (value == null)
					value = Activator.CreateInstance(t);
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel(name);
				if (GUILayout.Button( (value as List<GameObject>).Count.ToString() + " GameObject(s)") )
					NodeCanvasEditor.GameObjectListEditor.Show((List<GameObject>)value);
				GUILayout.EndHorizontal();
				return value;
			}

			if (typeof(IList).IsAssignableFrom(t)){
				if (value == null)
					value = Activator.CreateInstance(t);
				return ListEditor(name, (IList)value);
			}

			if (value == null)
				value = Activator.CreateInstance(t);

			GUILayout.BeginVertical();
			GUILayout.Label(TypeName(t));
			EditorGUI.indentLevel ++;
			foreach (FieldInfo field in value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
				field.SetValue(value, GenericField(field.Name, field.GetValue(value), field.FieldType, field) );
			EditorGUI.indentLevel --;
			GUILayout.EndVertical();

			return value;
		}

		public static LayerMask LayerMaskField(string prefix, LayerMask mask){

			var options = new List<string>();
			for (int i = 0; i <= 31; i++){
				var name = LayerMask.LayerToName(i);
				if (!string.IsNullOrEmpty(name) || i <= 8)
					options.Add(name);
			}

			mask = EditorGUILayout.MaskField(prefix, mask.value, options.ToArray());
			return mask;
		}

		public static IList ListEditor(string prefix, IList list){

			System.Type type = list.GetType().GetGenericArguments()[0];

			Separator();
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(prefix);

			if (GUILayout.Button("Add Element")){
				
				if (type.IsValueType){
					list.Add(Activator.CreateInstance(type));
				} else {
					list.Add(null);
				}
			}

			GUILayout.EndHorizontal();

			EditorGUI.indentLevel ++;

			for (int i = 0; i < list.Count; i++){
				GUILayout.BeginHorizontal();
				list[i] = GenericField("Element " + i, list[i], type);
				if (GUILayout.Button("X", GUILayout.Width(18)))
					list.Remove(list[i]);
				GUILayout.EndHorizontal();
			}

			EditorGUI.indentLevel --;
			Separator();

			GUILayout.EndVertical();
			return list;
		}

		//Convert camelCase to words as the name implies.
		public static string CamelCaseToWords(string s){

			s = s.Substring(0, 1).ToUpper() + s.Substring(1);
			return Regex.Replace(s, @"(\B[A-Z])", @" $1");
		}

		//a special object field for the BBVariable class to let user choose either a real value or enter a string to read data from a Blackboard
		public static BBVariable BBVariableField(string prefix, BBVariable bbVar, MemberInfo member = null){

			if (bbVar == null){
				EditorGUILayout.LabelField(prefix, "NULL");
				return null;
			}

			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			if (!bbVar.blackboardOnly && !bbVar.useBlackboard){

				FieldInfo field = bbVar.GetType().GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic);
				field.SetValue(bbVar, GenericField(prefix, field.GetValue(bbVar), bbVar.dataType, member) );

			} else {

				GUI.color = new Color(0.9f,0.9f,1f,1f);
				if (bbVar.bb){

					List<string> dataNames = bbVar.bb.GetDataNames(bbVar.dataType).ToList();

					dataNames.Add("/ ");
					foreach (KeyValuePair<string, Blackboard> pair in Blackboard.allBlackboards){
						if (pair.Value == bbVar.bb || !pair.Value.isGlobal)
							continue;
						foreach (string dName in pair.Value.GetDataNames(bbVar.dataType))
							dataNames.Add(pair.Key + "/" + dName);
					}

					dataNames.Add("(DynamicVar)");

					if (dataNames.Contains(bbVar.dataName) || string.IsNullOrEmpty(bbVar.dataName) ){

						if (bbVar.isDynamic){
							GUIUtility.keyboardControl = 0;
							bbVar.isDynamic = false;
						}

						bbVar.dataName = StringPopup(prefix, bbVar.dataName, dataNames, false, true);
						if (bbVar.dataName == "(DynamicVar)"){
							bbVar.isDynamic = true;
							bbVar.dataName = "_";
						}

					} else {

						bbVar.dataName = EditorGUILayout.TextField(prefix + " (" + TypeName(bbVar.dataType) + ")", bbVar.dataName);
					}	

				} else {

					bbVar.dataName = EditorGUILayout.TextField(prefix + " (" + TypeName(bbVar.dataType) + ")", bbVar.dataName);
				}
			}

			GUI.color = Color.white;
			GUI.backgroundColor = Color.white;

			if (!bbVar.blackboardOnly)
				bbVar.useBlackboard = EditorGUILayout.Toggle(bbVar.useBlackboard, EditorStyles.radioButton, GUILayout.Width(18));

			GUILayout.EndHorizontal();
		
			if (bbVar.bb && bbVar.useBlackboard && string.IsNullOrEmpty(bbVar.dataName)){	
				
				GUI.backgroundColor = new Color(0.8f,0.8f,1f,0.5f);
				GUI.color = new Color(1f,1f,1f,0.5f);
				GUILayout.BeginHorizontal("textfield");

				if (string.IsNullOrEmpty(bbVar.dataName)){
					GUILayout.Label("Select a '" + TypeName(bbVar.dataType) + "' Blackboard Variable");
				} else {
					GUILayout.Label("Variable name does not exist in blackboard");
				}
				
				GUILayout.EndHorizontal();
				GUILayout.Space(2);
			}

			GUILayout.EndVertical();

			//RequiresCoponent attribute should be checked here unfortunately
			if (member != null && bbVar.GetType() == typeof(BBGameObject)){
				var rc = member.GetCustomAttributes(typeof(RequiresComponentAttribute), true).FirstOrDefault() as RequiresComponentAttribute;
				if (rc != null && typeof(Component).IsAssignableFrom(rc.type) && !bbVar.isNull){
					if ( (bbVar as BBGameObject).value.GetComponent(rc.type) == null ){
						GUI.backgroundColor = new Color(1f,0.8f,0.8f,0.5f);
						GUI.color = new Color(1f,1f,1f,0.5f);
						GUILayout.BeginHorizontal("textfield");
						GUILayout.Label("GameObject requires '" + TypeName(rc.type) + "' Component");
						GUILayout.EndHorizontal();
						GUILayout.Space(2);
					}
				}
			}

			GUI.backgroundColor = Color.white;
			GUI.color = Color.white;			
			return bbVar;
		}


		///A simple reorderable list. Pass the list and a function to call for GUI
		private static Dictionary<IList, object> pickedObjectList = new Dictionary<IList, object>();
		public static void ReorderableList(IList list, System.Action<object> GUICallback, UnityEngine.Object undoObject = null){

			if (list.Count == 1){
				GUICallback(list[0]);
				return;
			}

			if (!pickedObjectList.ContainsKey(list))
				pickedObjectList[list] = null;

			var e = Event.current;
			var lastRect = new Rect();
			object picked = pickedObjectList[list];
			GUILayout.BeginVertical();
			for (int i= 0; i < list.Count; i++){

				GUILayout.BeginVertical();
				GUICallback(list[i]);
				GUILayout.EndHorizontal();

				GUI.color = Color.white;
				GUI.backgroundColor = Color.white;

				lastRect = GUILayoutUtility.GetLastRect();
				EditorGUIUtility.AddCursorRect(lastRect, MouseCursor.MoveArrow);

				if (picked != null && picked == list[i])
					GUI.Box(lastRect, "");

				if (picked != null && lastRect.Contains(e.mousePosition) && picked != list[i]){

					var markRect = new Rect(lastRect.x,lastRect.y-2,lastRect.width, 2);
					if (list.IndexOf(picked) < i)
						markRect.y = lastRect.yMax - 2;

					GUI.Box(markRect, "");
					if (e.type == EventType.MouseUp){
						if (undoObject != null)
							Undo.RecordObject(undoObject, "Reorder");
						list.Remove(picked);
						list.Insert(i, picked);
					}
				}

				if (lastRect.Contains(e.mousePosition) && e.type == EventType.MouseDown)
					pickedObjectList[list] = list[i];
			}
			GUILayout.EndVertical();

			if (e.type == EventType.MouseUp)
				pickedObjectList[list] = null;
		}

		public static bool BooleanField(string prefix, bool value){

			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(prefix);
			if (GUILayout.Button(value? "True" : "False", GUILayout.Height(15)))
				value = !value;
			GUILayout.EndHorizontal();
			return value;
		}

		public static Component ComponentField(Component comp){
			return ComponentField(string.Empty, comp, typeof(Component));
		}

		public static Component ComponentField(Component comp, System.Type type){
			return ComponentField(string.Empty, comp, type);
		}

		public static Component ComponentField(string prefix, Component comp){
			return ComponentField(prefix, comp, typeof(Component));
		}

		//an editor field where if the component is null simply shows an object field, but if its not, shows a dropdown popup to select the specific component
		//from within the gameobject
		public static Component ComponentField(string prefix, Component comp, System.Type type, bool allowNone = true){

			if (!comp){

				if (prefix != string.Empty){

					comp = EditorGUILayout.ObjectField(prefix, comp, type, true, GUILayout.ExpandWidth(true)) as Component;

				} else {

					comp = EditorGUILayout.ObjectField(comp, type, true, GUILayout.ExpandWidth(true)) as Component;
				}

				return comp;
			}

			List<Component> allComp = new List<Component>(comp.GetComponents(type));
			List<string> compNames = new List<string>();

			foreach (Component c in allComp.ToArray()){
				
				if (c == null)
					continue;

				if (c.hideFlags == HideFlags.HideInInspector){
					allComp.Remove(c);
					continue;
				}

				compNames.Add(TypeName(c.GetType()) + " @ " + c.gameObject.name);
			}

			if (allowNone)
				compNames.Add("|NONE|");

			int index;
			if (prefix != string.Empty)
				index = EditorGUILayout.Popup(prefix, allComp.IndexOf(comp), compNames.ToArray(), GUILayout.ExpandWidth(true));
			else
				index = EditorGUILayout.Popup(allComp.IndexOf(comp), compNames.ToArray(), GUILayout.ExpandWidth(true));
			
			if (allowNone && index == compNames.Count - 1)
				return null;

			return allComp[index];
		}


		public static string StringPopup(string selected, List<string> options, bool showWarning = true, bool allowNone = false, params GUILayoutOption[] GUIOptions){
			return StringPopup(string.Empty, selected, options, showWarning, allowNone, GUIOptions);
		}

		//a popup that it's based on the string rather than the index
		public static string StringPopup(string prefix, string selected, List<string> options, bool showWarning = true, bool allowNone = false, params GUILayoutOption[] GUIOptions){

			EditorGUILayout.BeginVertical();
			if (options.Count == 0 && showWarning){
				EditorGUILayout.HelpBox("There are no options to select for '" + prefix + "'", MessageType.Warning);
				EditorGUILayout.EndVertical();
				return null;
			}

			if (allowNone)
				options.Insert(0, "|NONE|");

			int index;

			if (options.Contains(selected))	index = options.IndexOf(selected);
			else index = allowNone? 0 : -1;

			if (!string.IsNullOrEmpty(prefix)) index = EditorGUILayout.Popup(prefix, index, options.ToArray(), GUIOptions);
			else index = EditorGUILayout.Popup(index, options.ToArray(), GUIOptions);

			if (index == -1 || (allowNone && index == 0)){

				if (showWarning){
					if (!string.IsNullOrEmpty(selected))
						EditorGUILayout.HelpBox("The previous selection '" + selected + "' has been deleted or changed. Please select another", MessageType.Warning);
					else
						EditorGUILayout.HelpBox("Please make a selection", MessageType.Warning);
				}
			}

			EditorGUILayout.EndVertical();
			if (allowNone)
				return index == 0? string.Empty : options[index];

			return index == -1? string.Empty : options[index];
		}

		//Shows a button that when click pops a context menu with a list of components deriving the type specified. When something is selected the callback is called
		//Passing null target gameObject creates a new one
		public static void ShowComponentSelectionButton(GameObject target, Type baseType, Action<Component> callback, bool hideComponent = false){

			GUILayout.BeginHorizontal();
			GUI.backgroundColor = lightBlue;

			if (GUILayout.Button("Add " + CamelCaseToWords( TypeName(baseType) ) )){

				Action<Type> ContextAction = delegate (Type script){

					if (!target)
						target = new GameObject(baseType.ToString());

					var newScript = target.AddComponent(script);
					Undo.RegisterCreatedObjectUndo(newScript, "New Script");
					Undo.RecordObject(newScript, "New Script");
					newScript.hideFlags = hideComponent? HideFlags.HideInInspector : 0;
					
					callback(newScript);
				};

				ShowTypeSelectionMenu(baseType, ContextAction);
			}

			if (Task.copyTask != null && baseType.IsAssignableFrom(Task.copyTask.GetType())){
				if (GUILayout.Button("P", GUILayout.Width(20) ))
					callback( Task.copyTask.CopyTo(target) );
			}

			GUI.backgroundColor = Color.white;
			GUILayout.EndHorizontal();
		}

		//shows a context menu with a list of components of specified type directly
		public static void ShowTypeSelectionMenu(Type baseType, Action<Type> callback){

				GenericMenu.MenuFunction2 Selected = delegate(object selectedType){
					callback((Type)selectedType);
				};

				var scriptInfos = GetAllScriptsOfTypeCategorized(baseType);
				GenericMenu menu= new GenericMenu();

				foreach (ScriptInfo script in scriptInfos){
					if (string.IsNullOrEmpty(script.category))
						menu.AddItem(new GUIContent(script.name), false, Selected, script.type);
				}

				menu.AddSeparator("/");

				foreach (ScriptInfo script in scriptInfos){
					if (!string.IsNullOrEmpty(script.category))
						menu.AddItem(new GUIContent( script.category + "/" + script.name), false, Selected, script.type);
				}

				menu.ShowAsContext();
		}


		//Get all scripts of a type excluding the base type and those with the DoNotList attribute, from within the project categorized as a list of ScriptInfo
		public static List<ScriptInfo> GetAllScriptsOfTypeCategorized(Type baseType){

			var allRequestedScripts = new List<ScriptInfo>();

			foreach (System.Type subType in GetAssemblyTypes(baseType)){
				
				if (subType.GetCustomAttributes(typeof(DoNotListAttribute), false).FirstOrDefault() as DoNotListAttribute == null){

					if (subType.IsAbstract)
						continue;
				
					string scriptName = CamelCaseToWords( TypeName(subType) );
					string scriptCategory = string.Empty;

					var nameAttribute = subType.GetCustomAttributes(typeof(NameAttribute), false).FirstOrDefault() as NameAttribute;
					if (nameAttribute != null)
						scriptName = nameAttribute.name;

					var categoryAttribute = subType.GetCustomAttributes(typeof(CategoryAttribute), true).FirstOrDefault() as CategoryAttribute;
					if (categoryAttribute != null)
						scriptCategory = categoryAttribute.category;

					allRequestedScripts.Add(new ScriptInfo(subType, scriptName, scriptCategory));
				}
			}

			allRequestedScripts = allRequestedScripts.OrderBy(script => script.name).ToList();
			allRequestedScripts = allRequestedScripts.OrderBy(script => script.category).ToList();

			return allRequestedScripts;
		}

		//Get all types in the current loaded assemplies
		public static List<System.Type> GetAssemblyTypes(System.Type baseType){
			
			var types = new List<System.Type>();
			foreach (Assembly ass in System.AppDomain.CurrentDomain.GetAssemblies()){
				foreach (System.Type t in ass.GetTypes()){
					if (baseType.IsAssignableFrom(t))
						types.Add(t);
				}
			}
			types = types.OrderBy(type => TypeName(type) ).ToList();
			types = types.OrderBy(type => type.Namespace).ToList();
			return types;
		}

		//get the right text for a type name
		public static string TypeName(Type t){

			if (t == null)
				return "NONE";

			string s = t.Name;
			
			if (s == "Single") s = "Float";
			if (s == "Int32") s = "Int";

			if (t.IsGenericParameter)
				s = "T";

			if (t.IsGenericType){
				
				Type[] args= t.GetGenericArguments();
				
				if (args.Length != 0){
				
					s = s.Replace("`" + args.Length.ToString(), "");

					s += "<";
					for (int i= 0; i < args.Length; i++)
						s += (i == 0? "":", ") + TypeName(args[i]);
					s += ">";
				}
			}

			return Strip(s, ".");
		}

		//strips anything before the specified character. Mostly use to strip namespaces
		public static string Strip(string text, string before){

			int index= text.LastIndexOf(before);
			if (index >= 0)
				return text.Substring(index + 1);
			return text;
		}


		public static void ShowFieldSelectionMenu(GameObject go, List<System.Type> availableTypes, System.Action<FieldInfo> callback){

			GenericMenu.MenuFunction2 Selected = delegate(object selectedField){
				callback((FieldInfo)selectedField);
			};

			var menu = new GenericMenu();
			foreach (Component comp in go.GetComponents<Component>()){

				if (comp.hideFlags == HideFlags.HideInInspector)
					continue;

				foreach (FieldInfo field in comp.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)){
					if (availableTypes.Contains(field.FieldType))
						menu.AddItem(new GUIContent( TypeName(comp.GetType()) + "/" + field.Name), false, Selected, field);
				}
			}

			if (menu.GetItemCount() == 0){
				Debug.Log("There are no Fields on any of the provided GameObject's Components!");
				return;
			}

			menu.ShowAsContext();
			Event.current.Use();			
		}

		public static void ShowMethodSelectionMenu(GameObject go, List<System.Type> returnTypes, List<System.Type> paramTypes, System.Action<MethodInfo> callback, bool propertiesOnly = false){

			GenericMenu.MenuFunction2 Selected = delegate(object selectedMethod){
				callback((MethodInfo)selectedMethod);
			};

			var menu = new GenericMenu();
			var commonMethods = new List<MethodInfo>();

			foreach (Component comp in go.GetComponents<Component>()){

				if (comp.hideFlags == HideFlags.HideInInspector)
					continue;

				var methodNames = new List<string>();
				foreach (MethodInfo method in comp.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)){

					if (propertiesOnly && !method.IsSpecialName)
						continue;

					if (!propertiesOnly && method.IsSpecialName)
						continue;

					if (method.IsGenericMethod)
						continue;

					var isAssignable = false;
					foreach(System.Type t in returnTypes){
						if (t.IsAssignableFrom(method.ReturnType) ){
							isAssignable = true;
							break;
						}
					}

					if (!isAssignable)
						continue;

					var parameters = method.GetParameters();
					if (parameters.Length > 3)
						continue;

					if (parameters.Length > 0){
						foreach(ParameterInfo param in parameters){
							isAssignable = false;
							foreach (System.Type t in paramTypes){
								if (t.IsAssignableFrom(param.ParameterType) ){
									isAssignable = true;
									break;
								}
							}
						}

					} else {

						isAssignable = true;
					}

					if (!isAssignable)
						continue;

					if (method.DeclaringType == typeof(Component) || !typeof(Component).IsAssignableFrom(method.DeclaringType)){
						commonMethods.Add(method);
						continue;
					}

					var methodName = method.Name;
					if (methodNames.Contains(methodName))
						methodName += "_";

					menu.AddItem(new GUIContent( TypeName(comp.GetType()) + "/" + methodName), false, Selected, method);
					methodNames.Add(methodName);
				}
			}

			foreach (MethodInfo method in commonMethods)
				menu.AddItem(new GUIContent("Common/" + method.Name), false, Selected, method);

			if (menu.GetItemCount() == 0){
				Debug.Log("There are no Methods/Properties on any of the provided GameObject's Components!");
				return;
			}

			menu.ShowAsContext();
			Event.current.Use();
		}

		///Returns event names found on type, that are either of the default EventHandler delegate type or of a parametless delegate handler
		public static List<string> GetAvailableEvents(System.Type type){

			var eventNames = new List<string>();
			foreach(EventInfo e in type.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){

				if (e.EventHandlerType == typeof(System.EventHandler)){
					eventNames.Add(e.Name);
					continue;
				}

				var m = e.EventHandlerType.GetMethod("Invoke");
				if (m.GetParameters().Length == 0 && m.ReturnType == typeof(void))
					eventNames.Add(e.Name);
			}

			return eventNames;
		}

		//Determines if a type can be casted to another
		public static bool CanConvert(Type fromType, Type toType) {
		    try
		    {
		        Expression.Convert(Expression.Parameter(fromType, null), toType);
		        return true;
		    }
		    catch
		    {
		        return false;
		    }
		}

		//all scene names (added in build settings)
		public static List<string> GetSceneNames(){

			List<string> allSceneNames = new List<string>();

			foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes){

				if (scene.enabled){

					string name= scene.path.Substring(scene.path.LastIndexOf("/") + 1);
					name = name.Substring(0,name.Length-6);
					allSceneNames.Add(name);
				}
			}

			return allSceneNames;
		}


	    public static void CreateAsset(Type o){

	        ScriptableObject asset= ScriptableObject.CreateInstance(o);
	        if (!asset){
	        	Debug.LogWarning("NULL");
	        	return;
	        }
	        string path= AssetDatabase.GetAssetPath (Selection.activeObject);
	        if (path == "")
	        {
	            path = "Assets";
	        }
	        else if (Path.GetExtension (path) != "")
	        {
	            path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
	        }

	        string assetPathAndName= AssetDatabase.GenerateUniqueAssetPath (path + "/New " + o.ToString() + ".asset");

	        AssetDatabase.CreateAsset (asset, assetPathAndName);

	        AssetDatabase.SaveAssets ();
	        EditorUtility.FocusProjectWindow ();
	        Selection.activeObject = asset;
	    }


		//for when getting scripts
		public class ScriptInfo{

			public Type type;
			public string name;
			public string category;

			public ScriptInfo(Type type, string name, string category){
				this.type = type;
				this.name = name;
				this.category = category;
			}
		}
	}
}

#endif



namespace NodeCanvas{

	///To exclude a class from when GetAllScriptsOfType. Note: Abstract classes are not listed anyway.
	[AttributeUsage(AttributeTargets.Class)]
	public class DoNotListAttribute : Attribute{

	}

	///Use for friendlier names if class name is weird for any reason
	[AttributeUsage(AttributeTargets.Class)]
	public class NameAttribute : Attribute{

		public string name;
		public NameAttribute(string name){
			this.name = name;
		}
	}

	///Use to categorize scripts
	[AttributeUsage(AttributeTargets.Class)]
	public class CategoryAttribute : Attribute{

		public string category;
		public CategoryAttribute(string category){
			this.category = category;
		}
	}

	///Use to give a description to a node or task
	[AttributeUsage(AttributeTargets.Class)]
	public class DescriptionAttribute : Attribute{

		public string description;
		public DescriptionAttribute(string description){
			this.description = description;
		}
	}

	///When something needs an Icon (now for nodes)
	[AttributeUsage(AttributeTargets.Class)]
	public class IconAttribute : Attribute{

		public string iconName;
		public IconAttribute(string iconName){
			this.iconName = iconName;
		}
	}	


	///Makes the int field show as layerfield
	[AttributeUsage(AttributeTargets.Field)]
	public class LayerFieldAttribute : Attribute{

	}

	///Makes the string field show as tagfield
	[AttributeUsage(AttributeTargets.Field)]
	public class TagFieldAttribute : Attribute{

	}

	///Makes the string field show as text field with specified height
	[AttributeUsage(AttributeTargets.Field)]
	public class TextAreaFieldAttribute : Attribute{

		public float height;
		public TextAreaFieldAttribute(float height){
			this.height = height;
		}
	}


	///Makes the float field show as slider
	[AttributeUsage(AttributeTargets.Field)]
	public class SliderFieldAttribute : Attribute{

		public float left;
		public float right;

		public SliderFieldAttribute(float left, float right){
			this.left = left;
			this.right = right;
		}
	}

	///Helper attribute. Designates that the field is required not to be null or string.empty
	///It can also be used on top of a BB variable field.
	[AttributeUsage(AttributeTargets.Field)]
	public class RequiredFieldAttribute : Attribute{

	}

	///Use on top of a BBGameObject or GameObject field
	[AttributeUsage(AttributeTargets.Field)]
	public class RequiresComponentAttribute : Attribute{

		public System.Type type;

		public RequiresComponentAttribute(System.Type type){
			this.type = type;
		}
	}

	///When auto editor layouting, starts a vertical box group
	[AttributeUsage(AttributeTargets.Field)]
	public class BeginGroupAttribute : Attribute{

	}

	///When auto editor layouting, ends a vertical box group
	[AttributeUsage(AttributeTargets.Field)]
	public class EndGroupAttribute : Attribute{

	}


	[Obsolete("Use [Name] instead")]
	[AttributeUsage(AttributeTargets.Class)]
	public class ScriptNameAttribute : Attribute{
		public string category;
		public ScriptNameAttribute(string category){
			this.category = category;
		}
	}

	[Obsolete("Use [Category] instead")]
	[AttributeUsage(AttributeTargets.Class)]
	public class ScriptCategoryAttribute : Attribute{
		public string category;
		public ScriptCategoryAttribute(string category){
			this.category = category;
		}
	}
}