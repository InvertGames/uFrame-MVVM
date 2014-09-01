public enum CollisionEventType
{
    OnCollisionEnter,	//OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
    OnCollisionExit,	//OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
    OnCollisionStay,	//OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
    OnTriggerEnter,	//OnTriggerEnter is called when the Collider other enters the trigger.
    OnTriggerExit,	//OnTriggerExit is called when the Collider other has stopped touching the trigger.
    OnTriggerStay, //OnTriggerStay is called once per frame for every Collider other that is touching the trigger.
}