using UnityEngine;
using System.Collections;

namespace NodeCanvas.Actions{

	[Category("✫ Utility")]
	[Description("Start, Resume, Pause, Stop a GraphOwner's behaviour")]
	[AgentType(typeof(GraphOwner))]
	public class GraphOwnerControl : ActionTask {

		public enum Control
		{
			StartBehaviour,
			StopBehaviour,
			PauseBehaviour
		}

		public Control control = Control.StartBehaviour;

		protected override string info{
			get {return agentInfo + "." + control.ToString();}
		}

		//This is done OnUpdate as to ensure its one frame later and not immediate
		protected override void OnUpdate(){

			if (control == Control.StartBehaviour)
				(agent as GraphOwner).StartGraph();
			else if (control == Control.StopBehaviour)
				(agent as GraphOwner).StopGraph();
			else if (control == Control.PauseBehaviour)
				(agent as GraphOwner).PauseGraph();

			EndAction();			
		}
	}
}