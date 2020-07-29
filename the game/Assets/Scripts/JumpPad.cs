using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {
    public Transform jumpPad;
    public ThirdPersonJump TPJ;
    private float currentVelocity;
    public float smoothTime;

    void Update() {
        jumpPad = GetComponent<Transform>();
        if (TPJ.isJumpPad) {
            StartCoroutine(JumpPadCoroutine());
        }
    }
    private IEnumerator JumpPadCoroutine() {
        jumpPad.position -= new Vector3(0f, 0.15f, 0f);
        yield return new WaitForSeconds(0.2f);
        jumpPad.position += new Vector3(0f, 0.15f, 0f);
    }
}
