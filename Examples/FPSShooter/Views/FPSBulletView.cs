using UnityEngine;

public class FPSBulletView : MonoBehaviour
{
    public float _BulletSpeed = 10f;

    public void Awake()
    {
        hideFlags = HideFlags.HideInHierarchy;
    }
    public void Start()
    {
        Destroy(this.gameObject,2f);
        
    }
    public void Update()
    {
        this.transform.position += (this.transform.forward * _BulletSpeed * Time.deltaTime);
    }
}