using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class BirdView
{
    private Vector3 _Velocity = Vector3.zero;
    private bool _didFlap = false;

    public override void StateChanged(BirdState value)
    {
        if (value == BirdState.Idle)
            this.transform.position = Vector3.zero;
    }

    public void FixedUpdate()
    {
        if (Bird.State != BirdState.Alive) return;
        _Velocity += new Vector3(0f,Bird.Gravity,0f) * Time.deltaTime;
        if (_didFlap)
        {
            _didFlap = false;
            _Velocity += new Vector3(0f, Bird.FlapVelocity, 0f);
        }
        _Velocity = Vector3.ClampMagnitude(_Velocity, Bird.MaxSpeed);
        transform.position += _Velocity*Time.deltaTime;

        float angle = _Velocity.y < 0 ?Mathf.Lerp(0f, -90, -_Velocity.y / Bird.MaxSpeed) : 0;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
        ExecuteHit();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            _didFlap = true;
            ExecuteFlapped();
        }
    }
}
