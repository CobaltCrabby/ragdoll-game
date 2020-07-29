using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour{

    public ThirdPersonJump TPJ;
    public new Camera camera;
    public LayerMask playerMask;
    public LayerMask allLayers;
    public GameObject player;
    public bool playerDisappear;
    public float distance;

    void Update() {
        distance = player.transform.position.y - camera.transform.position.y;
    }

    void LateUpdate() {
        if (Mathf.Abs(distance) <= .3 && TPJ.isGrounded) {
            camera.cullingMask = playerMask;
        }
        else {
            camera.cullingMask = allLayers;
        }
    }
}
