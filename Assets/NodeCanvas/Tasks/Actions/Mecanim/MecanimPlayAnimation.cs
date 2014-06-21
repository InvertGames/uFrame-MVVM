using UnityEngine;
 
namespace NodeCanvas.Actions{
 
    [Name("Play Animation")]
    [Category("Mecanim")]
    [AgentType(typeof(Animator))]
    public class MecanimPlayAnimation : ActionTask{
     
        public int layerIndex;
        [RequiredField]
        public string stateName;
        [SliderField(0,1)]
        public float transitTime = 0.25f;
     
        public bool waitUntilFinish;
     
        [GetFromAgent]
        private Animator animator;
        private AnimatorStateInfo stateInfo;
        private bool played;
     
        protected override string info{
            get {return "Mec.PlayAnimation '" + stateName + "'";}
        }
     
        protected override void OnExecute(){
            played = false;
            animator.CrossFade(stateName, transitTime, layerIndex);
        }
     
        protected override void OnUpdate(){
         
            stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
         
            if (waitUntilFinish){
             
                if (stateInfo.IsName(stateName)){
            
					played = true;
                    if(elapsedTime >= (stateInfo.length / animator.speed))
                        EndAction();              

                } else if (played) {

                    EndAction();
                }
             
            } else {

                if (elapsedTime >= transitTime)
                    EndAction();
            }
        }
    }
}