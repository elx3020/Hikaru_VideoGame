using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health; 

    }

    
}
