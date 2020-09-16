using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollider : MonoBehaviour
{
    private Rigidbody RB;
    private Rigidbody hips;
    private ThirdPersonMovement TPM;
    private HammerWeapon hammerWeapon;
    private Enemy1Script enemy1Script;
    private bool isDead;
    private float force = 5f;

    void Start() {
        RB = GetComponent<Rigidbody>();
        hammerWeapon = FindObjectOfType<HammerWeapon>();
        enemy1Script = GetComponentInParent<Enemy1Script>();
        TPM = FindObjectOfType<ThirdPersonMovement>();
        hips = TPM.gameObject.GetComponent<Rigidbody>();
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            foreach (Rigidbody rb in enemy1Script.rigidbodys) {
                rb.isKinematic = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.name == "hammer3" && hammerWeapon.isEnemyGrappling) {
            KillEnemy();
            hammerWeapon.StopGrapple();
            foreach (Rigidbody rb in enemy1Script.rigidbodys) {
                rb.velocity = Vector3.zero;
            }
            hammerWeapon.ResetVelocity();
            hips.velocity = Vector3.up * 30;
        }
        
        else if (collision.collider.name == "hammer3" && hammerWeapon.particles.isPlaying) {
            KillEnemy();
        }
    }

    void Update() {
        if (Vector3.Distance(transform.position, hammerWeapon.transform.position) <= 0.2 && !isDead && hammerWeapon.isThrowing) {
            KillEnemy();
            hammerWeapon.hitThrow = true;
            isDead = true;
        }
    }

    void KillEnemy() {
        enemy1Script.isDead = true;
        enemy1Script.animator.enabled = false;

        foreach (Rigidbody rb in enemy1Script.rigidbodys) {
            rb.isKinematic = false;
        }
        RB.velocity = hammerWeapon.armRB.velocity * force;
    }
}
