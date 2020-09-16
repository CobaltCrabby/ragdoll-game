using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VignetteAnimate : MonoBehaviour
{
    private Volume volume;
    private float maxWeight = 0f, velocity;
    private bool vignetteDown = false;

    void Start() {
        volume = GetComponent<Volume>();
    }

    void FixedUpdate() {
        if (vignetteDown) {
            volume.weight = Mathf.SmoothDamp(volume.weight, 0f, ref velocity, .2f);
            if (volume.weight <= 0) {
                vignetteDown = false;
            }
        }
    }

    public IEnumerator VignetteCoroutine() {
        maxWeight += 1;
        volume.weight = maxWeight / 5;
        vignetteDown = false;
        yield return new WaitForSeconds(1.5f);
        vignetteDown = true;
    }
}
