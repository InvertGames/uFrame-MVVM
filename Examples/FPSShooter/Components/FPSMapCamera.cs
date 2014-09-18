using UnityEngine;
using System.Collections;

public class FPSMapCamera : MonoBehaviour
{
    public Transform _Follow;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(_Follow.position.x, transform.position.y, _Follow.position.z);

        this.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
    }
}
