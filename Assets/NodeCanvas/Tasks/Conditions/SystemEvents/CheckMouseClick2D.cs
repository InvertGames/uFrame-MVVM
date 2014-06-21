using UnityEngine;
using System.Collections;

namespace NodeCanvas.Conditions{

	[Category("System Events")]
	[Name("Check Mouse Click 2D")]
	[AgentType(typeof(Collider2D))]
	[EventListener("OnMouseDown", "OnMouseUp")]
	public class CheckMouseClick2D : ConditionTask {

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