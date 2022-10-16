using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertiesDisplayer : MonoBehaviour
{
	public PlayerProp playerProp;
	public PlayerPropMGR playerPropMGR;

	public Text text_health;
	public Text text_hunger;
	public Text text_calories;
	public Text text_thirst;
	public Text text_temperature;
	public Text text_mentality;
	public Text text_weight;
	public Text text_warmth;
	public Text text_digesting;
	public Text text_exhaustion;
	public Text text_sensible_temperature;
	public Text text_power;
	public Text text_defenseBonus;
	public Text text_Immunity;

	void Start()
    {
		
    }

    void Update()
    {
		text_health.text = MyStringMethods.singleton.Assemble_Strings("Health : ", playerProp.health.ToString("F0"));
		text_hunger.text = MyStringMethods.singleton.Assemble_Strings("Hunger : ", playerProp.hunger.ToString("F0"));
		text_calories.text = MyStringMethods.singleton.Assemble_Strings("Calories : ", playerProp.calories.ToString("F0"));
		text_thirst.text = MyStringMethods.singleton.Assemble_Strings("Thirst : ", playerProp.thirst.ToString("F0"));
		text_temperature.text = MyStringMethods.singleton.Assemble_Strings("Temp : ", playerProp.temperature.ToString("F0"));
		text_mentality.text = MyStringMethods.singleton.Assemble_Strings("Mentality : ", playerProp.mentality.ToString("F0"));
		text_weight.text = MyStringMethods.singleton.Assemble_Strings("Weight : ", playerProp.body_weight.ToString("F2"));
		text_warmth.text = MyStringMethods.singleton.Assemble_Strings("Warmth : ", playerProp.warmth.ToString("F0"));
		text_digesting.text = MyStringMethods.singleton.Assemble_Strings("Digesting : ", playerProp.digesting_calories.ToString("F0"));
		text_exhaustion.text = MyStringMethods.singleton.Assemble_Strings("Exhaust : ", playerProp.exhaustion.ToString("F0"));

	}
}
