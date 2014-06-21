using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using NodeCanvas;

namespace NodeCanvasEditor{

	// An instance is needed only for when debugging EvenHandler.
	[CustomEditor(typeof(EventDebugger))]
	public class EventDebuggerInspector : Editor{

		private EventDebugger debbuger{
			get {return target as EventDebugger;}
		}

		public int totalMembers;

		override public void OnInspectorGUI(){

			debbuger.logEvents = EditorGUILayout.Toggle("Log Events?", debbuger.logEvents);

			GUI.color = Color.yellow;
			EditorGUILayout.LabelField("Total Events: " + debbuger.subscribedMembers.Count);
			EditorGUILayout.LabelField("Total Members: " + totalMembers);
			GUI.color = Color.white;

			totalMembers = 0;

			foreach (KeyValuePair<System.Enum, List<EventHandler.SubscribedMember>> subscribedMember in debbuger.subscribedMembers){

				if (subscribedMember.Value.Count == 0)
					continue;

				totalMembers += subscribedMember.Value.Count;

				GUILayout.BeginVertical("box");

				GUI.color = Color.yellow;
				EditorGUILayout.LabelField(subscribedMember.Key.ToString());
				GUI.color = Color.white;

				foreach (EventHandler.SubscribedMember member in subscribedMember.Value){

					GUILayout.BeginVertical("textfield");
					
					if (member.subscribedMono != null)
						EditorGUILayout.LabelField("Member", member.subscribedMono.ToString());
					
					if (member.subscribedFunction != null)
						EditorGUILayout.LabelField("Function", member.subscribedFunction.ToString());
					
					EditorGUILayout.LabelField("Invoke Priority", member.invokePriority.ToString());
					EditorGUILayout.LabelField("Unsubscribe After Receive", member.unsubscribeWhenReceive.ToString());
					GUILayout.EndVertical();
				}

				GUILayout.EndVertical();
			}

			if (GUI.changed)
				EditorUtility.SetDirty(debbuger);
		}
	}
}
