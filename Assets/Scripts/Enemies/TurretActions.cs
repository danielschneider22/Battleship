using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretActions : MonoBehaviour
{
    GameObject playerBody;
    public GameObject rotateHinge;

    public GameObject bulletPrefab;
    public GameObject trackingBulletPrefab;
    public Transform firePoint;

    public float fireRate = 1f;
    private float fireCountdown = 0f;

    void Awake()
    {
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody");
        
    }
    private void Update()
    {
        rotateHinge.transform.LookAt(playerBody.transform);

        if(fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject prefab = Random.Range(1, 10) > 3 ? bulletPrefab : trackingBulletPrefab;
        GameObject bulletGO = Instantiate(prefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.Seek(playerBody.transform);
        }
    }

}
