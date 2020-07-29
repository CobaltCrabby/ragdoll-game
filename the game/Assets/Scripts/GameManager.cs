using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public enum GunState {grappling, shotgun};
    public GunState whichGun;
    public GameObject[] guns;
    void Update(){
        switch (whichGun) {
            case GunState.grappling:
                guns[0].SetActive(true);
                guns[1].SetActive(false);
                break;
            case GunState.shotgun:
                guns[0].SetActive(false);
                guns[1].SetActive(true);
                break;
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            if (whichGun == GunState.grappling) {
                whichGun = GunState.shotgun;
            }
            else {
                whichGun = GunState.grappling;
            }
        }
    }
}
