using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerstats : MonoBehaviour
{

    public float maxHP;
    public float currentHP;
    public float maxHunger;
    public float currentHunger;
    public float maxWater;
    public float currentWater;

    public float currentTemperature;
    
    
    public Slider hpSlider;
    public Slider foodSlider;
    public Slider waterSlider;
    
    public Slider temperatureSlider;


    public bool isHungry;
    public bool isThirsty;

    

    public void Start()
    {
        currentHP = maxHP;
        currentHunger = maxHunger;
        currentWater = maxWater;
        currentTemperature = 50f;

        hpSlider.maxValue = maxHP;
        foodSlider.maxValue = maxHunger;
        waterSlider.maxValue = maxWater;
        
        hpSlider.value = currentHP;
        foodSlider.value = currentHunger;
        waterSlider.value = currentWater;

        temperatureSlider.maxValue = 100;
        temperatureSlider.value = 50;
        
    }

    public void Update()
    {
        //Sort Clamping issue
        Mathf.Clamp(currentHunger, 0, maxHunger);
        Mathf.Clamp(currentHP, 0, maxHP);
        Mathf.Clamp(currentWater, 0, maxWater);


        currentHunger -= .3f * Time.deltaTime;
        currentWater -= .3f * Time.deltaTime;

        if(currentHunger <= 0)
        {
            isHungry = true;
        }
        if(currentWater <= 0)
        {
            isThirsty = true;
        }

        if(isHungry || isThirsty)
        {

            currentHP -= .5f * Time.deltaTime;
        }

        if (currentTemperature >= 100) {
            currentTemperature -= 1f * Time.deltaTime;
        } else {
            currentTemperature -= .3f * Time.deltaTime;
        }

        //Lose HP if Cold or Hot
        if (currentTemperature <= 10 || currentTemperature >= 100) {
            currentHP -= .5F * Time.deltaTime;
        }

        hpSlider.value = currentHP;
        foodSlider.value = currentHunger;
        waterSlider.value = currentWater;

        temperatureSlider.value = currentTemperature; 

    } 
    
}
