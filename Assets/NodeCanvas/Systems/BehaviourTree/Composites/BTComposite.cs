using UnityEngine;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	abstract public class BTComposite : BTNodeBase {

		public override System.Type outConnectionType{
			get{return typeof(ConditionalConnection);}
		}

		public override int maxInConnections{
			get{return 1;}
		}

		public override int maxOutConnections{
			get {return -1;}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnContextMenu(UnityEditor.GenericMenu menu){

			menu.AddItem (new GUIContent ("Make Nested"), false, ContextMakeNested);
		}		
	
		//TODO possibly move making nested into Node so that it's reusable in other graph systems as well
		private void ContextMakeNested(){

			if (!UnityEditor.EditorUtility.DisplayDialog("Make Nested", "This will create a new Nested BT and place this node and all child nodes inside.\nThe Nested BT can NOT be unpacked later on!\nAre you sure?", "Yes", "No!"))
				return;

			BTNestedBTNode newNestedNode = graph.AddNewNode(typeof(BTNestedBTNode)) as BTNestedBTNode;
			BehaviourTree newBT = (BehaviourTree)Graph.CreateNested(newNestedNode, typeof(BehaviourTree), "Nested BT");

			newNestedNode.nodeRect.center = this.nodeRect.center;
			
			if (this.graph.primeNode == this)
				this.graph.primeNode = newNestedNode;

			//Relink connections to the new nested tree node
			foreach (Connection connection in inConnections.ToArray())
				connection.Relink(newNestedNode);

			//Copy the nodes over to the new graph. TODO: Use IReferencable interface instead of check type
			foreach (Node node in FetchAllChildNodes(true)){
				
				if (this.graph.primeNode == node)
					this.graph.primeNode = newNestedNode;

				if (node.GetType() == typeof(BTAction))
					(node as BTAction).BreakReference();

				if (node.GetType() == typeof(BTCondition))
					(node as BTCondition).BreakReference();
				
				node.MoveToGraph(newBT);
			}

			//TODO: Use IReferencable interface instead of check type
			foreach (Node node in newNestedNode.graph.allNodes){
				
				if (node.GetType() == typeof(BTAction)){
					if ( this.graph.allNodes.Contains( (node as BTAction).referencedNode ) )
						(node as BTAction).BreakReference();
				}

				if (node.GetType() == typeof(BTCondition)){
					if ( this.graph.allNodes.Contains( (node as BTCondition).referencedNode ) )
						(node as BTCondition).BreakReference();
				}
			}

			UnityEditor.Undo.RecordObject(this, "Make Nested");
			newBT.primeNode = this;
			this.inConnections.Clear();
		}

		#endif
	}
}