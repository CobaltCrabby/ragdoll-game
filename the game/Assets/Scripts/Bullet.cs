using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float spreadX, spreadZ, spreadY, bulletLife;
    private float bulletTimer = 0f;
    private bool didCollide = false;
    private Camera cam;
    public float speed, spread;
    public ParticleSystem particles;

    void Awake() {
        particles.Stop();
    }

    void Start(){
        bulletLife = 40f;
        cam = FindObjectOfType<Camera>();
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
        if (other.transform.gameObject.layer == 13) {
            particles.Play();
            didCollide = true;
        }
    }
}
