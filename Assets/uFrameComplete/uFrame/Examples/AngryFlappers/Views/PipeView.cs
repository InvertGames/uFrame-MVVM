using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class PipeView {

    public override void Awake()
    {
        base.Awake();
        this.transform.position = new Vector3(17.96289f, UnityEngine.Random.Range(-2.3f, 4.11f));
    }

    public void Update()
    {
        if (Pipe.ParentAngryFlappersGame.State == AngryFlappersGameState.Playing)
        this.transform.position -= Vector3.right*Pipe.ParentAngryFlappersGame.ScrollSpeed*Time.deltaTime;

    }
}
