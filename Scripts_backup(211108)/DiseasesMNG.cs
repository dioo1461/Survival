using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiseasesMNG : MonoBehaviour {
	public PlayerProp playerProp;
	public HealthMNG healthMNG;
	public StaminaMNG staminaMNG;

	bool is_foodPoisoning = false;
	bool is_foodPoisoning_duplicated = false;
	bool is_foodPoisoning_curing = false;
	float foodPoisoning_remaining = 0f;
	

	bool is_hypothermia = false;
	bool is_hypothermia_duplicated = false;
	bool is_hypothermia_curing = false;
	float hypothermia_remaining = 0f;

	int is_bleeding_state = 0;
	bool is_bleeding_curing = false;
	bool is_dressing_dirty = false;
	float bleeding_remaining = 0f;

	bool is_bloodPoisoning = false;
	bool is_bloodPoisoning_duplicated = false;
	bool is_bloodPoisoning_curing = false;
	float bloodPoisoning_remaining = 0f;

	/* HARM = 시간당 건강감소량
	* TIME_REQUIRED = 해당 질병의 치유 소요시간
	*/
	public const float HARM_FOODPOISONING = 2f;
	public const float TIME_REQUIRED_FOODPOISONING = 3f * 24f * 3600f;
	public const float FOODPOISONING_RATE_STAMINA_USAGE_INCREASE = 1f;

	public const float HARM_HYPOTHERMIA = 5f;
	public const float TIME_REQUIRED_HYPOTHERMIA = 12f * 3600f;
	public const float HYPOTHERMIA_RATE_STAMINA_USAGE_INCREASE = 1.2f;

	public const float HARM_BLEEDING_1 = 60f;
	public const float TIME_REQUIRED_BLEEDING_1 = 2f * 24f * 3600f;
	public const float HARM_BLEEDING_2 = 120f;
	public const float TIME_REQUIRED_BLEEDING_2 = 4f * 24f * 3600f;
	public const float HARM_BLEEDING_3 = 300f;
	public const float TIME_REQUIRED_BLEEDING_3 = 8f * 24f * 3600f;

	public const float HARM_BLOODPOISONING = 20f;
	public const float TIME_REQUIRED_BLOODPOISONING = 1f * 24f * 3600f;

	void Start() {

	}
	void Update() {

	}

	public void FoodPoisoning_Occur() {
		if (is_foodPoisoning) {
			is_foodPoisoning_duplicated = true;
		} else {
			is_foodPoisoning = true;
		}
		foodPoisoning_remaining = TIME_REQUIRED_FOODPOISONING;
		StartCoroutine(FoodPoisoning_Coroutine());
		
	}

	IEnumerator FoodPoisoning_Coroutine() {
		float current_time = GameManager.totalPlayTime;
		float timeGap;
		staminaMNG.rate_stamina_usage *= FOODPOISONING_RATE_STAMINA_USAGE_INCREASE;
		while (foodPoisoning_remaining > 0f) {
			if(is_foodPoisoning_duplicated) {
				is_foodPoisoning_duplicated = false;
				staminaMNG.rate_stamina_usage /= FOODPOISONING_RATE_STAMINA_USAGE_INCREASE;
				yield break;
			}
			timeGap = GameManager.totalPlayTime - current_time;
			playerProp.player_health -= HARM_FOODPOISONING * timeGap / 3600f;
			healthMNG.Player_Health_Changed();

			// 항생제 투여 시 회복속도 3배
			if (is_foodPoisoning_curing) {
				foodPoisoning_remaining -= timeGap * 3f;
			} else {
				foodPoisoning_remaining -= timeGap;
			}
			current_time = GameManager.totalPlayTime;
			yield return new WaitForSeconds(10f * Time.deltaTime);
		}
		is_foodPoisoning = false;
		is_foodPoisoning_curing = false;
		foodPoisoning_remaining = 0f;
		staminaMNG.rate_stamina_usage /= FOODPOISONING_RATE_STAMINA_USAGE_INCREASE;
	}

	public void Hypothermia_Occur() {
		if (is_hypothermia) {
			is_hypothermia_duplicated = true;
		} else {
			is_hypothermia = true;
		}
		hypothermia_remaining = TIME_REQUIRED_HYPOTHERMIA;
		StartCoroutine(Hypothermia_Coroutine());
		
	}
	
	IEnumerator Hypothermia_Coroutine() {
		float current_time = GameManager.totalPlayTime;
		float timeGap;
		staminaMNG.rate_stamina_usage *= HYPOTHERMIA_RATE_STAMINA_USAGE_INCREASE;
		while (hypothermia_remaining > 0f) {
			if (is_hypothermia_duplicated) {
				is_hypothermia_duplicated = false;
				staminaMNG.rate_stamina_usage /= HYPOTHERMIA_RATE_STAMINA_USAGE_INCREASE;
				yield break;
			}

			timeGap = GameManager.totalPlayTime - current_time;
			playerProp.player_health -= HARM_HYPOTHERMIA * timeGap / 3600f;
			healthMNG.Player_Health_Changed();
			
			if(playerProp.player_temperature >= 36f) {
				is_hypothermia_curing = true;
			}

			if (is_hypothermia_curing) {
				hypothermia_remaining -= timeGap;
			} else if (hypothermia_remaining < TIME_REQUIRED_HYPOTHERMIA) {
				hypothermia_remaining += timeGap * 3f;
			}

			current_time = GameManager.totalPlayTime;
			yield return new WaitForSeconds(10f * Time.deltaTime);
		}
		is_hypothermia = false;
		is_hypothermia_curing = false;
		hypothermia_remaining = 0f;
		staminaMNG.rate_stamina_usage /= HYPOTHERMIA_RATE_STAMINA_USAGE_INCREASE; 
	
	}

	public void Bleeding_Occur(int state) {
		if (state >= is_bleeding_state) {
			switch (state) {
				case 1: bleeding_remaining = TIME_REQUIRED_BLEEDING_1; break;
				case 2: bleeding_remaining = TIME_REQUIRED_BLEEDING_2; break;
				case 3: bleeding_remaining = TIME_REQUIRED_BLEEDING_3; break;
			}
			StartCoroutine(Bleeding_Coroutine());
		}
	}
	IEnumerator Bleeding_Coroutine() {
		float current_time = GameManager.totalPlayTime;
		float timeGap;
		float harm = HARM_BLEEDING_3;
		while (bleeding_remaining > 0) {
			if (is_bleeding_state != 1 && bleeding_remaining < TIME_REQUIRED_BLEEDING_1) {
				is_bleeding_state = 1;
				harm = HARM_BLEEDING_1;
			} else if (is_bleeding_state != 2 && bleeding_remaining < TIME_REQUIRED_BLEEDING_2) {
				is_bleeding_state = 2;
				harm = HARM_BLEEDING_2;
			}
			
			timeGap = GameManager.totalPlayTime - current_time;

			// 지혈 시 건강감소 중지, 회복 시작
			if (is_bleeding_curing) {
				/* if(더러운 붕대) 패혈증 확률
				 */
				/* if(과격한 행동) 재출혈 확률
				 */
				bleeding_remaining -= timeGap;
			} else {
				playerProp.player_health -= harm * timeGap / 3600f;
				healthMNG.Player_Health_Changed();
			}
			current_time = GameManager.totalPlayTime;
			yield return new WaitForSeconds(10f * Time.deltaTime);
		}
		is_bleeding_state = 0;
		is_bleeding_curing = false;
		bleeding_remaining = 0f;
	}


	public void BloodPoisoning_Occur() {
		if (is_bloodPoisoning) {
			is_bloodPoisoning_duplicated = true;
		} else {
			is_bloodPoisoning = true;
		}
		bloodPoisoning_remaining = TIME_REQUIRED_BLOODPOISONING;
		StartCoroutine(BloodPoisoning_Coroutine());

	}

	IEnumerator BloodPoisoning_Coroutine() {
		float current_time = GameManager.totalPlayTime;
		float timeGap;
		while (bloodPoisoning_remaining > 0f) {
			if (is_bloodPoisoning_duplicated) {
				is_bloodPoisoning_duplicated = false;
				yield break;
			}
			timeGap = GameManager.totalPlayTime - current_time;
			playerProp.player_health -= HARM_BLOODPOISONING * timeGap / 3600f;
			healthMNG.Player_Health_Changed();

			// 항생제 투여가 중단되면 감염 재확산
			if (is_bloodPoisoning_curing) {
				bloodPoisoning_remaining -= timeGap;
			} else {
				bloodPoisoning_remaining += 0.5f * timeGap;
			}
			current_time = GameManager.totalPlayTime;
			yield return new WaitForSeconds(10f * Time.deltaTime);
		}
		is_bloodPoisoning = false;
		bloodPoisoning_remaining = 0f;
	}

}
