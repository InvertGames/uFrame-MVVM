using UnityEngine;
using System.Collections;

public class AlwaysLookAt : MonoBehaviour
{
    public Transform _LookAt;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    this.transform.LookAt(_LookAt);
	}
}
