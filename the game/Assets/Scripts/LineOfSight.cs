using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineOfSight : MonoBehaviour {

    public Transform headTransform, gunTip;
    public float bulletSpeed, bulletLife, gunRange;
    public LayerMask groundLayer = 1 << 8, playerLayer = 1 << 9;
    public ParticleSystem particles;
    public GameObject bullet;
    private float playerDist, shootCooldown;
    private Transform hips;
    private bool didSee;
    private GameObject instBullet;
    private Enemy1Script enemy1Script;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Start() {
        enemy1Script = GetComponentInParent<Enemy1Script>();
        hips = FindObjectOfType<ThirdPersonMovement>().transform;
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponentInParent<Animator>();
        particles.Stop();
    }


    void OnTriggerStay(Collider other) {
        bool player = Physics.Raycast(headTransform.position, hips.position - headTransform.position, playerDist, playerLayer);
        bool ground = Physics.Raycast(headTransform.position, hips.position - headTransform.position, playerDist, groundLayer);
        if (other.gameObject.layer == 9 && player && !ground) {
            didSee = true;
        }
    }

    void Update() {
        playerDist = Vector3.Distance(transform.parent.transform.position, hips.position);
        if (!enemy1Script.isDead && didSee) {
            enemy1Script.gameObject.transform.LookAt(hips.transform.position);
            if (playerDist <= gunRange) {
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Isn't Walking");
            }
            else if (playerDist > gunRange) {
                navMeshAgent.destination = hips.position;
                navMeshAgent.isStopped = false;
                animator.SetTrigger("Is Walking");
            }
        }
    }

    void FixedUpdate() {
        if (playerDist <= gunRange && !enemy1Script.isDead && didSee) {
            if (shootCooldown >= 90) {
                instBullet = Instantiate(bullet, gunTip.position, transform.rotation);
                instBullet.SetActive(true);

                EnemyBullet currentBullet = instBullet.GetComponent<EnemyBullet>();
                currentBullet.speed = bulletSpeed;
                currentBullet.bulletLife = bulletLife;

                shootCooldown = 0;

                animator.SetTrigger("Shoot");
                particles.Play();
            }
            shootCooldown += 1;
        }
    }
}
