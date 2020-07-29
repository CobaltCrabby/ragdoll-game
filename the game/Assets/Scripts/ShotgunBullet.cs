using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public float spreadX, spreadZ, spreadY, speed, spread, bulletLife;
    private float bulletTimer = 0f;
    private bool isMoving;
    public Rigidbody RB;
    public Camera cam;

    void Start(){
        bulletTimer = 0f;
        cam = FindObjectOfType<Camera>();
        RB = GetComponent<Rigidbody>();
        spreadX = UnityEngine.Random.Range(-spread, spread);
        spreadZ = UnityEngine.Random.Range(-spread, spread);
        spreadY = UnityEngine.Random.Range(-spread, spread);
        transform.forward = cam.transform.forward;
        transform.Rotate(spreadX, spreadY, spreadZ);
    }

    void FixedUpdate() {
        RB.AddForce(transform.forward * speed);
        if (bulletTimer >= bulletLife) {
            Destroy(gameObject);
        }
        bulletTimer++;
    }
}
