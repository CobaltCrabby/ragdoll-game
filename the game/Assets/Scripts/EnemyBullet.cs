using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    private float bulletTimer = 0;
    public float bulletLife = 100;
    public GameObject bullet;
    private Transform player;
    private ParticleSystem particles;
    private bool didCollide;

    void Start () {
        particles = GetComponentInChildren<ParticleSystem>();
        player = FindObjectOfType<ThirdPersonMovement>().transform;
        particles.Stop();
        transform.LookAt(player);
    }

    void FixedUpdate(){
        if (!didCollide) {
            transform.position += transform.forward * speed;
        }
        if (bulletTimer >= bulletLife) {
            Destroy(gameObject);
        }
        bulletTimer++;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Shootable") {
            particles.Play();
            Destroy(bullet);
            didCollide = true;
            if (other.gameObject.layer == 9) {
                print("player hit");
            }
        }
    }
}
