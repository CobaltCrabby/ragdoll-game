using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float hp, currentHp;
    private Slider slider;

    void Start() {
        hp = 5f;
        currentHp = 5f;
        slider = GetComponentInChildren<Slider>();    
    }

    void Update() {
        currentHp = Mathf.Lerp(currentHp, hp, Time.deltaTime * 10);
        if (currentHp - hp <= 0.005) {
            currentHp = hp;
        }
        slider.value = currentHp;   
    }

    public void takeDamage(int damage) {
        hp -= damage;
    }
}
