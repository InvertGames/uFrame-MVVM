using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	///The base class for Behaviour Tree decorators
	abstract public class BTDecorator : BTNodeBase{

		public override int maxOutConnections{
			get{return 1;}
		}

		public override int maxInConnections{
			get{return 1;}
		}

		public override System.Type outConnectionType{
			get{return typeof(Connection);}
		}

		///The decorated connection object
		protected Connection decoratedConnection{
			get
			{
				if (outConnections.Count != 0)
					return outConnections[0];
				return null;			
			}
		}

		///The decorated node object
		protected Node decoratedNode{
			get
			{
				if (outConnections.Count != 0)
					return outConnections[0].targetNode;
				return null;			
			}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (decoratedConnection)
				return decoratedConnection.Execute(agent, blackboard);

			return NodeStates.Failure;
		}
	}
}