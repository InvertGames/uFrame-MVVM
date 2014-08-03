using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class GroundView {
    public void Update()
    {
        this.transform.position -= Vector3.right * AngryFlappersGame.ScrollSpeed * Time.deltaTime;
    }
}
