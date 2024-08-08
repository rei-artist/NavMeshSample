using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;
using UnityEngine.UI;

public class HpGuage : MonoBehaviour
{
    [SerializeField] private UnityChanControlScriptWithRgidBody player;
    [SerializeField] private Slider sliderRed;
    [SerializeField] private Slider sliderGreen;
    private float preValue;
    private float timeDamaged;

    // Start is called before the first frame update
    void Start()
    {
        timeDamaged = 0;
        float newValue = (float)player.hp / player.maxHp;
        sliderGreen.value = newValue;
        sliderRed.value = newValue;
        preValue = newValue;
    }

    // Update is called once per frame
    void Update()
    {
        float newValue = (float)player.hp / player.maxHp;
        sliderGreen.value = newValue;
        if (newValue < preValue)
        {
            timeDamaged = Time.time;
            preValue = newValue;
        }
        if(timeDamaged + 3f < Time.time)
        {
            sliderRed.value = newValue;
        }
    }
}
