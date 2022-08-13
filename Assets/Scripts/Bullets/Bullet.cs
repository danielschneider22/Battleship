using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private Vector3 initTargetPos;
    private Vector3 dir;

    public float speed = 20f;
    public bool isTracking = false;

    public void Seek (Transform _target)
    {
        target = _target;
        initTargetPos = _target.position;
        dir = target.position - transform.position;
    }

    private void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }
        float distanceThisFrame = speed * Time.deltaTime;
        if (isTracking)
        {
            Vector3 dir = target.position - transform.position;
            transform.LookAt(target.transform);

            if (dir.magnitude <= distanceThisFrame && isTracking)
            {
                Destroy(gameObject);
                return;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
        else
        {
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
        
    }
}
