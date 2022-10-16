using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealButton : MonoBehaviour
{
	public PlayerProp playerProp;
	public HealthMNG healthMNG;

	public void OnMouseUpAsButton() {
		playerProp.player_health = 100f;
		healthMNG.Player_Health_Changed();
	}
	
	void Start()
    {
        
    }


    void Update()
    {
        
    }
}
