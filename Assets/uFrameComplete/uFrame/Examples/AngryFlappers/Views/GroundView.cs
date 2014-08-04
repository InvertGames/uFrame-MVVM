using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class GroundView {
    public void Update()
    {
        if (AngryFlappersGame.State == AngryFlappersGameState.Playing)
        {
            if (this.transform.position.x <= -20f)
            {
                this.transform.position = new Vector3(21.1f, -5.3f, -0.1f);
            }
            this.transform.position -= Vector3.right * AngryFlappersGame.ScrollSpeed * Time.deltaTime;
        }
        
    }
}
