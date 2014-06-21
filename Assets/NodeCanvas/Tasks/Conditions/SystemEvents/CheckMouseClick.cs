using UnityEngine;
using System.Collections;

namespace NodeCanvas.Conditions{

	[Category("System Events")]
	[AgentType(typeof(Collider))]
	[EventListener("OnMouseDown", "OnMouseUp")]
	public class CheckMouseClick : ConditionTask {

		public enum MouseClickEvent{
			MouseDown = 0,
			MouseUp = 1
		}

		public MouseClickEvent checkType = MouseClickEvent.MouseDown;

		protected override string info{
			get{ return checkType.ToString();}
		}

		protected override bool OnCheck(){
			return false;
		}


		void OnMouseDown(){
			if (checkType == MouseClickEvent.MouseDown)
				YieldReturn(true);
		}

		void OnMouseUp(){
			if (checkType == MouseClickEvent.MouseUp)
				YieldReturn(true);
		}
	}
}