using UnityEngine;

public class ForgeSmoothCamera : MonoBehaviour
{

    public float smoothTime = 0.3f;
    public GameObject target;
    public GameObject goal;
    private Vector3 delta;

    //public void SetTarget(GameObject t, GameObject g)
    //{

    //    target = t;
    //    goal = g;
    //    delta = Vector3.zero; //(goal.transform.position - target.transform.position ) * 0.5f;

    //    var gotoNow =  target.transform.position + delta;;
    //    transform.position = new Vector3(gotoNow.x, transform.position.y, gotoNow.z);
    //}
    void Update ()
    {
        if (target == null) return;

        var between = target.transform.position + (delta * 0.2f);

	    if (target != null)
	    {
	        var velocityX = target.transform.position.x;//rigidbody.velocity.x;
	        var velocityY = target.transform.position.y;//rigidbody.velocity.z;
            transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, between.x, ref velocityX, smoothTime),
                transform.position.y,
                Mathf.SmoothDamp(transform.position.z, between.z, ref velocityY, smoothTime));    
	    }
	    
	}
}

//#pragma strict

//private var thisTransform : Transform;
//private var velocity : Vector2;

//function Start()
//{
//    thisTransform = transform;
//}

//function Update() 
//{
//    thisTransform.position.x = Mathf.SmoothDamp( thisTransform.position.x, 
//        target.position.x, velocity.x, smoothTime);
//    thisTransform.position.y = Mathf.SmoothDamp( thisTransform.position.y, 
//        target.position.y, velocity.y, smoothTime);
//}
