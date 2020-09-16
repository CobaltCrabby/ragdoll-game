using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineOfSight : MonoBehaviour {

    public Transform headTransform, gunTip;
    public float bulletSpeed, bulletLife, gunRange, losRange, bulletSpread;
    public ParticleSystem particles;
    public GameObject bullet;

    private LayerMask sightLayer = 1 << 8 | 1 << 9;
    private Vector3 velocity;
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

    void Update() {
        //creates to direction vectors for the LOS
        Vector3 leftMax = Quaternion.Euler(0, -45, 0) * enemy1Script.transform.forward;
        Vector3 rightMax = Quaternion.Euler(0, 45, 0) * enemy1Script.transform.forward;

        //finds distance between player and enemy
        playerDist = Vector3.Distance(transform.parent.transform.position, hips.position);

        //create player local position and angle in LOS
        Vector3 player = hips.position - headTransform.position;
        float angleArea = Vector3.Angle(leftMax, rightMax);

        if (Vector3.Angle(leftMax, player) < angleArea && Vector3.Angle(rightMax, player) < angleArea) {
            //raycasts to see if any objects are in the way of player and enemy
            bool hitObject = Physics.Raycast(headTransform.position, hips.position - headTransform.position, out RaycastHit hit, losRange, sightLayer);
            Debug.DrawRay(headTransform.position, hips.position - headTransform.position);
            if (hitObject) {
                if (hit.transform.gameObject.layer == 9) {
                    didSee = true;
                }
            }
        }

        //changes animations and looks at player based on if the player is seen and in range of shooting
        if (!enemy1Script.isDead && didSee) {
            enemy1Script.transform.LookAt(new Vector3(hips.position.x, enemy1Script.transform.position.y, hips.position.z));
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

    //fires bullets at player
    void FixedUpdate() {
        if (playerDist <= gunRange && !enemy1Script.isDead && didSee) {
            if (shootCooldown >= 90) {
                instBullet = Instantiate(bullet, gunTip.position, transform.rotation);
                instBullet.SetActive(true);

                EnemyBullet currentBullet = instBullet.GetComponent<EnemyBullet>();
                currentBullet.speed = bulletSpeed;
                currentBullet.bulletLife = bulletLife;
                currentBullet.bulletSpread = bulletSpread;

                shootCooldown = 0;

                animator.SetTrigger("Shoot");
                particles.Play();
            }
            shootCooldown += 1;
        }
    }
}
