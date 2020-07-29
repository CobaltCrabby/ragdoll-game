using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour {

    public GameObject bullet;
    public Transform shotgunPoint;
    public Camera cam;
    public Rigidbody RB;

    void Start() {
        RB = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Camera>();
    }
    void Update(){
        if (Input.GetMouseButtonDown(0)) {
            for (int i = 0; i < 5; i++) {
                Instantiate(bullet, shotgunPoint.position, shotgunPoint.rotation);
            }
            RB.AddForce(-cam.transform.forward * 700);
        }
    }
}
