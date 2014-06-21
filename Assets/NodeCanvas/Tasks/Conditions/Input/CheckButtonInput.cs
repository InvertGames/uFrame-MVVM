using UnityEngine;

namespace NodeCanvas.Conditions{

	[Category("Input")]
	public class CheckButtonInput : ConditionTask{

		public enum PressTypes {ButtonDown, ButtonUp, ButtonPressed}
		public PressTypes pressType = PressTypes.ButtonDown;

		[RequiredField]
		public string buttonName = "Fire1";

		protected override string info{
			get {return pressType.ToString() + " '" + buttonName + "'";}
		}

		protected override bool OnCheck(){

			if (pressType == PressTypes.ButtonDown)
				return Input.GetButtonDown(buttonName);
			
			if (pressType == PressTypes.ButtonUp)
				return Input.GetButtonUp(buttonName);
			
			if (pressType == PressTypes.ButtonPressed)
				return Input.GetButton(buttonName);	

			return false;					
		}
	}
}