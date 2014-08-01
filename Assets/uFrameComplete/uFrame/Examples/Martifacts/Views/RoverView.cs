using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class RoverView
{

    public Vector3 _StartCameraPosition;
    public Vector3 _StartCameraEuler;
    public Transform _BirdView;
    public Transform _BirdView2;

    private NavMeshAgent _agent;
    public Animation _Animation;
    public NavMeshAgent NavAgent
    {
        get
        {
            return _agent ?? (_agent = this.GetComponent<NavMeshAgent>());
        }
    }

    public Transform _ArtifactsContainer;

    public override void Awake()
    {
        _ArtifactsContainer = GameObject.Find("_Artifacts").transform;
    }
    public override void Bind()
    {

        base.Bind();
    }

    public override ViewBase CreateCollectedArtifactsView(ArtifactViewModel artifact)
    {
        return null;
    }

    public override void CollectedArtifactsAdded(ArtifactViewBase artifact)
    {
        //base.CollectedArtifactsAdded(artifact);
    }

    public override void StateChanged(RoverState value)
    {
        base.StateChanged(value);
        if (value == RoverState.Moving)
        {
            NavAgent.Stop(true);
            NavAgent.SetDestination(Rover.RoverTargetPosition);
            this.transform.LookAt(Rover.RoverTargetPosition);
            _Animation.CrossFade("moving", 1f);
            _hasPath = true;
            StartCoroutine(WaitForCompletion());

        }

        else if (value == RoverState.Firing)
        {
            _Animation.CrossFade("flare", 1f);
            StartCoroutine(BirdsEyeView());

        }
        else if (value == RoverState.Drilling)
        {
            _Animation.CrossFade("drilling", 1f);
            //_Animation.Play("drilling");
        }
        else
        {
            NavAgent.Stop(true);
            _Animation.CrossFade("Idle", 1f);
            //_Animation.Play("Idle");

        }
    }

    private IEnumerator BirdsEyeView()
    {
        _StartCameraPosition = Camera.main.transform.position;
        _StartCameraEuler = Camera.main.transform.rotation.eulerAngles;
        yield return new WaitForSeconds(2f);
        Camera.main.transform.position = _BirdView2.transform.position;
        //Camera.main.transform.rotation = _BirdView2.transform.rotation;
//        Camera.main.GetComponent<GlowEffect>().enabled = true;
        _ArtifactsContainer.gameObject.SetActive(true);


        GoTween goTween = Camera.main.transform.positionTo(1f, _BirdView.transform.position);
        goTween.easeType = GoEaseType.SineInOut;
        goTween.setOnCompleteHandler((x) =>
        {
            
            Camera.main.transform.positionTo(5f, _StartCameraPosition).setOnCompleteHandler((t) =>
            {
                _ArtifactsContainer.gameObject.SetActive(false);
//                Camera.main.GetComponent<GlowEffect>().enabled = false;
            });

            Camera.main.transform.rotationTo(2f, _StartCameraEuler).setOnCompleteHandler((tween) =>
            {
                
                
                Camera.main.transform.rotationTo(5f, _StartCameraEuler);
            });
        });
        


    }

    public float _PathEndThreshold = 0.2f;
    private bool _hasPath = false;
    bool AtEndOfPath()
    {
        _hasPath |= NavAgent.hasPath;
        if (_hasPath && NavAgent.remainingDistance <= NavAgent.stoppingDistance + _PathEndThreshold)
        {
            // Arrived
            _hasPath = false;
            return true;
        }

        return false;
    }
    public override void SpeedChanged(float value)
    {
        base.SpeedChanged(value);
        NavAgent.speed = value;
    }

    public override void MovesAdded(RoverMove item)
    {
        base.MovesAdded(item);

    }


    public IEnumerator WaitForCompletion()
    {
        yield return new WaitForSeconds(1f);
        while (!AtEndOfPath())
        {
            yield return new WaitForSeconds(0.1f);
        }
        ExecuteReachedDestination();
        NavAgent.Stop();

    }

    public void Update()
    {



    }
}
