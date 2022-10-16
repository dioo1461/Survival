using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealButton : MonoBehaviour
{
	public PlayerProp playerProp;
	public HealthMGR healthMNG;

	public void OnMouseUpAsButton() {
		playerProp.health = 100f;
		healthMNG.Player_Health_Changed();
	}
	
	void Start()
    {
        
    }


    void Update()
    {
        
    }
}
