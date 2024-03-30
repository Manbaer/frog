using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    private Transform firePoint;

    void Start()
    {
        firePoint = GameObject.FindGameObjectWithTag("firePoint").transform;
    }

    void Update()
    {
        transform.position = firePoint.position;
    }
}