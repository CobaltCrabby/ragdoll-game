using Cinemachine;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    public float speed;
    private float bulletTimer = 0;
    public float bulletLife = 100;
    public GameObject bullet;
    private ThirdPersonMovement TPM;
    private Transform player;
    private ParticleSystem particles;
    private PlayerHealth playerHealth;
    private bool didCollide, collideBool;

    void Start() {
        particles = GetComponentInChildren<ParticleSystem>();
        player = FindObjectOfType<ThirdPersonMovement>().transform;
        playerHealth = FindObjectOfType<PlayerHealth>();
        TPM = FindObjectOfType<ThirdPersonMovement>();
        particles.Stop();
        transform.LookAt(player);
    }

    void FixedUpdate() {
        if (!didCollide) {
            transform.position += transform.forward * speed;
        }
        if (bulletTimer >= bulletLife) {
            Destroy(gameObject);
        }
        bulletTimer++;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Shootable") {

            particles.Play();
            Destroy(bullet);
            didCollide = true;

            if (collision.gameObject.layer == 9 && !collideBool) {
                StartCoroutine(collideBoolTimer());
                playerHealth.takeDamage(1);
                StartCoroutine(TPM.cameraShake(0.25f, 0.75f));
            }

            else if (Vector3.Distance(collision.GetContact(0).point, TPM.transform.position) < 0.4f && !collideBool) {
                StartCoroutine(collideBoolTimer());
                playerHealth.takeDamage(1);
                StartCoroutine(TPM.cameraShake(1f, 1f));
            }
        }
    }

    IEnumerator collideBoolTimer() {
        collideBool = true;
        yield return new WaitForSeconds(0.5f);
        collideBool = false;
    }
}
