using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndHover : MonoBehaviour
{
    [SerializeField]
    private float m_RotationsPerMinute = 2f;
    [SerializeField]
    private bool m_ShouldRotateX = false, m_ShouldRotateY = false, m_ShouldRotateZ = true;
    [SerializeField]
    private float m_FloatRate = 2f;
    [SerializeField]
    private float m_FloatHeight = .5f;

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // rotate
        float rotationTarget = 6.0f * m_RotationsPerMinute * Time.deltaTime;
        float rotationX = m_ShouldRotateX ? rotationTarget : 0;
        float rotationY = m_ShouldRotateY ? rotationTarget : 0;
        float rotationZ = m_ShouldRotateZ ? rotationTarget : 0;
        transform.Rotate(rotationX, rotationY, rotationZ);

        // float
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * m_FloatRate) * m_FloatHeight + pos.y;
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
