using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PlayerProp : MonoBehaviour {
	// 직업

	// 기본 스탯
	public float player_health { get; set; }
	public float player_hunger { get; set; }
	public float player_thirst { get; set; }
	public float player_temperature { get; set; }
	public float player_stamina { get; set; }
	public float player_max_stamina { get; set; }

	public float player_moveSpeed { get; set; }
	public float player_behaveSpeed { get; set; }

	// 스킬 레벨
	public int fireStartingSkill_level { get; set; }
	public int fireStartingSkill_exp { get; set; }
	public int cookingSkill_level { get; set; }
	public int cookingSkill_exp { get; set; }
	public int craftingSkill_level { get; set; }
	public int craftingSkill_exp { get; set; }
	public int huntingSkill_level { get; set; }
	public int huntingSkill_exp { get; set; }
	public int fisiningSkill_level { get; set; }
	public int fishingSkill_exp { get; set; }

	
	

	void Start() {
		player_health = 100f;
		player_hunger = 60f;
		player_thirst = 60f;
		player_temperature = 36.5f;
		player_stamina = 100f;
		player_max_stamina = 100f;

		player_moveSpeed = 2f;
		player_behaveSpeed = 100f;

		fireStartingSkill_level = 1;
		fireStartingSkill_exp = 0;
		cookingSkill_level = 1;
		cookingSkill_exp = 0;
		craftingSkill_level = 1;
		craftingSkill_exp = 0;
		huntingSkill_level = 1;
		huntingSkill_exp = 0;
		fisiningSkill_level = 1;
		fishingSkill_exp = 0;

		
		GameObject.Find("HealthBar").GetComponent<HealthMNG>().Player_Health_Changed();
	}
	
}


