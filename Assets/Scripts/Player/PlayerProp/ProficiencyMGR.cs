using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProficiencyMGR : MonoBehaviour
{
	public PlayerProp playerProp;

	public int _fireStarting_levelUp_required_exp = 100;

	public void FireStarting_Experienced(int exp) {
		playerProp.fireStartingSkill_exp += exp;
		if (playerProp.fireStartingSkill_exp >= _fireStarting_levelUp_required_exp) {
			playerProp.fireStartingSkill_level++;
		}
	}

	void Start() 
	{
		for (int i = 0; i < playerProp.fireStartingSkill_level; i++) {
			_fireStarting_levelUp_required_exp += _fireStarting_levelUp_required_exp / 2;
			_fireStarting_levelUp_required_exp -= _fireStarting_levelUp_required_exp % 10;
		}

	}

	void Update()
    {
        
    }





}
