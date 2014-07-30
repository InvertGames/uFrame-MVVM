using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class RoverReticle
{
    public Vector3 _StartScale;
    public Vector3 _GrowFactor;
    public int _Iterations = 20;
    public override void Awake()
    {
        _StartScale = this.transform.localScale;
        _GrowFactor = _StartScale / _Iterations;
        StartCoroutine(MoveOut());
    }

    public override void Bind(ViewBase view)
    {
        base.Bind(view);
        Debug.Log("MOVING!!!!!");
        view.BindProperty(() => Rover._StateProperty, (v) =>
        {
            if (v == RoverState.Moving)
            {
                Debug.Log("MOVING!!!!!");
                
            }
            else
            {
                StopAllCoroutines();
            }
        });

    }

    public void Update()
    {
        //this.transform.position = Rover.RoverTargetPosition + new Vector3(0,0.1f,0f);
    }

    public IEnumerator MoveOut()
    {
        while (true)
        {
            for (var i = 0; i < _Iterations; i++)
            {
                yield return new WaitForSeconds(0.1f);
                this.transform.localScale += _GrowFactor;
            }

            this.transform.localScale = _StartScale;
        }
        
        
    }
}
