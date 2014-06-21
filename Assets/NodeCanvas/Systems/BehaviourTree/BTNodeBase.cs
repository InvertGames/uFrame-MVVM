using UnityEngine;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	///The base class for all Behaviour Tree system nodes
	abstract public class BTNodeBase : Node{

		public override System.Type outConnectionType{
			get{return typeof(ConditionalConnection);}
		}

		public override int maxInConnections{
			get{return 1;}
		}

		public override int maxOutConnections{
			get{return 0;}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeReleased(){
			SortConnectionsByPositionX();
		}

		#endif
	}
}