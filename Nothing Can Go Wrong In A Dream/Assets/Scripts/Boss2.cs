using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss2 : MonoBehaviour
{
    public int bossHealth;
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = bossHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
