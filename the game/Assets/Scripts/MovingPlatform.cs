using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 endPoint, startPoint;
    public bool forwards, isCollide;
    public float time = 2f;
    private Vector3 velocity, velocity2;
    private PlayerHealth player;
    private GameObject hips;


    void Start() {
        player = FindObjectOfType<PlayerHealth>();
        hips = FindObjectOfType<ThirdPersonMovement>().gameObject;
        StartCoroutine(movePlatform());
    }

    void Update() {
        if (forwards) {
            transform.position = Vector3.SmoothDamp(transform.position, endPoint, ref velocity, time);
        }

        else {
            transform.position = Vector3.SmoothDamp(transform.position, startPoint, ref velocity2, time);
        }
    }

    public IEnumerator movePlatform() {
        forwards = true;
        yield return new WaitForSeconds(time + 3);
        forwards = false;
        yield return new WaitForSeconds(time + 3);
        StartCoroutine(movePlatform());
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer == 9) {
            player.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.layer == 9) {
            player.gameObject.transform.parent = null;
        }
    }
}
