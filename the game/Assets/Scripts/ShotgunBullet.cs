using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public float spreadX, spreadZ, spreadY, speed, spread, bulletLife;
    private float bulletTimer = 0f;
    public bool didCollide = false;
    public ParticleSystem particles;
    public LayerMask groundLayer;
    private Rigidbody RB;
    private Camera cam;

    void Awake() {
        particles.Stop();
    }

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
        if (!didCollide) {
            transform.position += transform.forward * speed;
        }
        if (bulletTimer >= bulletLife) {
            Destroy(gameObject);
            didCollide = false;
        }
        bulletTimer++;
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.tag == "Ground") {
            particles.Play();
            didCollide = true;
        }
    }
}
