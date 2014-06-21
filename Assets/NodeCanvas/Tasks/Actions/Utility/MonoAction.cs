using UnityEngine;

namespace NodeCanvas.Actions{

	[Category("✫ Utility")]
	[Description("Use to enable / disable an existing MonoBehaviour on a gameObject. To end this action, set that MonoBehaviour.enabled to false.")]
	public class MonoAction : ActionTask {

		[SerializeField] [RequiredField]
		private MonoBehaviour _mono;

		public MonoBehaviour mono{
			get {return _mono;}
			set
			{
				_mono = value;
				if (_mono != null && !Application.isPlaying)
					_mono.enabled = false;
			}
		}

		protected override string info{
			get {return mono != null? ("Mono '" + mono.GetType().Name + "'") : "Select a MonoBehaviour";}
		}

		protected override void OnExecute(){

			mono.enabled = true;
		}

		protected override void OnUpdate(){

			if (mono.enabled == false)
				EndAction();
		}

		protected override void OnStop(){

			mono.enabled = false;
		}
	}
}