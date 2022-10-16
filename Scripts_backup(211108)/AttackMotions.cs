using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMotions : MonoBehaviour
{
	public StaminaMNG staminaMNG;
	public PlayerMovement playerMovement;

	public GameObject hatchetMotion;
	public float hatchet_damage = 18f;
	public float hatchet_stamina_usage = 10f;
	public float hatchet_use_delay = 0.8f;
	public float hatchet_Range = 0.7f;




	void Using_Hatchet() {
		Instantiate(hatchetMotion, )
	}


    void Start()
    {
        
    }

    void Update()
    {
        
    }

	
}
