using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    
    public float speed;
    public float bulletLife = 100;
    public float bulletSpread;
    private float bulletTimer = 0;

    private VignetteAnimate vignette;
    private ThirdPersonMovement TPM;
    private Transform bullet;
    private ParticleSystem particles;
    private PlayerHealth playerHealth;
    private bool didCollide;

    void Start() {
        particles = GetComponentInChildren<ParticleSystem>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        TPM = FindObjectOfType<ThirdPersonMovement>();
        bullet = transform.GetChild(1);
        vignette = FindObjectOfType<VignetteAnimate>();

        Vector3 randomOffset = new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread));
        
        particles.Stop();
        
        transform.LookAt(TPM.transform);
        transform.eulerAngles += randomOffset;
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

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Shootable" && !didCollide) {

            didCollide = true;
            if (other.gameObject.layer != 9) {
                particles.Play();

            }
            Destroy(bullet.gameObject);

            if (other.gameObject.layer == 9 || Vector3.Distance(transform.position, TPM.transform.position) < 0.4f) {
                didCollide = true;
                playerHealth.TakeDamage(1);
                StartCoroutine(TPM.CameraShake(5f, 20f));
                StartCoroutine(vignette.VignetteCoroutine());
            }
        }
    }
}
