using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class DoorTP : MonoBehaviour
{
    private CircleWipeTransition circleWipe;

    void Start() {
        circleWipe = FindObjectOfType<CircleWipeTransition>();    
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 9) {
            StartCoroutine(circleWipe.SceneTransition());
        }    
    }
}
