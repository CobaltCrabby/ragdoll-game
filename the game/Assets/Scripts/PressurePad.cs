using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PressurePad : MonoBehaviour {

    public Animator anim;
    public Animator gateAnim;

    void OnTriggerEnter(Collider other) {
        anim.SetTrigger("Down");
        gateAnim.SetTrigger("Open");
        print("collide");    
    }
}
