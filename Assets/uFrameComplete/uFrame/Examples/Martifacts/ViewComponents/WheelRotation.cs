using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class WheelRotation {
    public void Update()
    {
        if (Rover.State == RoverState.Moving)
        {
            this.transform.RotateAround(this.transform.position, this.transform.right, 180f * Time.deltaTime);    
        }
        
    }
}
