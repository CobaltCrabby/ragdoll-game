using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ProBuilder;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    private Rigidbody RB;
    private Rigidbody hips;
    private ThirdPersonMovement TPC;
    private HammerWeapon hammerWeapon;
    private Enemy1Script enemy1Script;
    private float force = 5f;

    void Start() {
        RB = GetComponent<Rigidbody>();
        hammerWeapon = FindObjectOfType<HammerWeapon>();
        enemy1Script = GetComponentInParent<Enemy1Script>();
        TPC = FindObjectOfType<ThirdPersonMovement>();
        hips = TPC.gameObject.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.name == "hammer3" && hammerWeapon.isEnemyGrappling) {
            KillEnemy();
            hammerWeapon.StopGrapple();
            foreach (Rigidbody rb in enemy1Script.rigidbodys) {
                rb.velocity = Vector3.up - Vector3.up;
            }
            hips.velocity = Vector3.up * 40;
        }
        
        else if (collision.collider.name == "hammer3" && hammerWeapon.particles.isPlaying) {
            KillEnemy();
        }
    }

    void KillEnemy() {
        enemy1Script.animator.enabled = false;

        foreach (Rigidbody rb in enemy1Script.rigidbodys) {
            rb.isKinematic = false;
        }

        if ((hammerWeapon.armRB.velocity * force).magnitude < 25) {
            RB.velocity = (hammerWeapon.armRB.velocity * force * 2f);
        }

        else if ((hammerWeapon.armRB.velocity * force).magnitude > 40) {
            RB.velocity = (hammerWeapon.armRB.velocity * force * 0.75f);
        }

        else {
            RB.velocity = (hammerWeapon.armRB.velocity * force);
        }
    }
}
