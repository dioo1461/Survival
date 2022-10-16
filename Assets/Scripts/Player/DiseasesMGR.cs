using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiseasesMGR : MonoBehaviour {
	public PlayerProp playerProp;
	public HealthMGR healthMGR;
	public StaminaMGR staminaMGR;

	bool _is_foodPoisoning = false;
	bool _is_foodPoisoning_duplicated = false;
	bool _is_foodPoisoning_curing = false;
	float _foodPoisoning_cure_acceleration = 0f;
	float _foodPoisoning_med_duration = 0f;
	float _foodPoisoning_timeRemaining = 0f;
	

	bool _is_hypothermia = false;
	bool _is_hypothermia_duplicated = false;
	bool _is_hypothermia_curing = false;
	float _hypothermia_cure_acceleration = 0f;
	float _hypothermia_med_duration = 0f;
	float _hypothermia_timeRemaining = 0f;

	int bleeding_state = 0;
	bool _is_bleeding_curing = false;
	bool _is_dressing_dirty = false;
	float _bleeding_cure_acceleration = 0f;
	float _bleeding_med_duration = 0f;
	float _bleeding_timeRemaining = 0f;

	bool _is_bloodPoisoning = false;
	bool _is_bloodPoisoning_duplicated = false;
	bool _is_bloodPoisoning_curing = false;
	float _bloodPoisoning_cure_acceleration = 0f;
	float _bloodPoisoning_med_duration = 0f;
	float _bloodPoisoning_timeRemaining = 0f;

	/* HARM = 시간당 건강감소량
	* TIME_REQUIRED = 해당 질병의 치유 소요시간
	*/
	public const float HARM_FOODPOISONING_PER_HOUR = 2f;
	public const float CURETIME_REQUIRED_FOODPOISONING = 3f * 24f * 3600f;
	public const float FOODPOISONING_STAMINA_USAGE_RATE_INCREASE = 1f;

	public const float HARM_HYPOTHERMIA_PER_HOUR = 5f;
	public const float CURETIME_REQUIRED_HYPOTHERMIA = 12f * 3600f;
	public const float HYPOTHERMIA_RATE_STAMINA_USAGE_INCREASE = 1.2f;
	
	public const float HARM_BLEEDING_1_PER_HOUR = 60f;
	public const float CURETIME_REQUIRED_BLEEDING_1 = 2f * 24f * 3600f;
	public const float HARM_BLEEDING_2_PER_HOUR = 120f;
	public const float CURETIME_REQUIRED_BLEEDING_2 = 4f * 24f * 3600f;
	public const float HARM_BLEEDING_3_PER_HOUR = 300f;
	public const float CURETIME_REQUIRED_BLEEDING_3 = 8f * 24f * 3600f;

	public const float HARM_BLOODPOISONING = 20f;
	public const float CURETIME_REQUIRED_BLOODPOISONING = 1f * 24f * 3600f;

	void Start() {

	}
	void Update() {

	}

	public void Incur_FoodPoisoning() {
		if (_is_foodPoisoning) {
			_is_foodPoisoning_duplicated = true;
		} else {
			_is_foodPoisoning = true;
		}
		_foodPoisoning_timeRemaining = CURETIME_REQUIRED_FOODPOISONING;
		StartCoroutine(FoodPoisoning_Coroutine());
		
	}

	IEnumerator FoodPoisoning_Coroutine() {
		float current_time = GameManager.singleton.TotalPlayTime;
		float timeGap;
		staminaMGR.rate_stamina_usage *= FOODPOISONING_STAMINA_USAGE_RATE_INCREASE;
		while (_foodPoisoning_timeRemaining > 0f) {
			if(_is_foodPoisoning_duplicated) {
				_is_foodPoisoning_duplicated = false;
				staminaMGR.rate_stamina_usage /= FOODPOISONING_STAMINA_USAGE_RATE_INCREASE;
				yield break;
			}
			timeGap = GameManager.singleton.TotalPlayTime - current_time;
			playerProp.health -= HARM_FOODPOISONING_PER_HOUR * timeGap / 3600f;
			healthMGR.Player_Health_Changed();

			// 항생제 투여 시 회복속도 3배
			if (_is_foodPoisoning_curing) {
				_foodPoisoning_timeRemaining -= timeGap * 3f;
			} else {
				_foodPoisoning_timeRemaining -= timeGap;
			}
			current_time = GameManager.singleton.TotalPlayTime;
			yield return new WaitForSeconds(0.1f);
		}
		_is_foodPoisoning = false;
		_is_foodPoisoning_curing = false;
		_foodPoisoning_timeRemaining = 0f;
		staminaMGR.rate_stamina_usage /= FOODPOISONING_STAMINA_USAGE_RATE_INCREASE;
	}

	public void Incur_Hypothermia() {
		if (_is_hypothermia) {
			_is_hypothermia_duplicated = true;
		} else {
			_is_hypothermia = true;
		}
		_hypothermia_timeRemaining = CURETIME_REQUIRED_HYPOTHERMIA;
		StartCoroutine(Hypothermia_Coroutine());
		
	}
	
	IEnumerator Hypothermia_Coroutine() {
		float current_time = GameManager.singleton.TotalPlayTime;
		float timeGap;
		staminaMGR.rate_stamina_usage *= HYPOTHERMIA_RATE_STAMINA_USAGE_INCREASE;
		while (_hypothermia_timeRemaining > 0f) {
			if (_is_hypothermia_duplicated) {
				_is_hypothermia_duplicated = false;
				staminaMGR.rate_stamina_usage /= HYPOTHERMIA_RATE_STAMINA_USAGE_INCREASE;
				yield break;
			}

			timeGap = GameManager.singleton.TotalPlayTime - current_time;
			playerProp.health -= HARM_HYPOTHERMIA_PER_HOUR * timeGap / 3600f;
			healthMGR.Player_Health_Changed();
			
			if(playerProp.temperature >= 36f) {
				_is_hypothermia_curing = true;
			}

			if (_is_hypothermia_curing) {
				_hypothermia_timeRemaining -= timeGap;
			} else if (_hypothermia_timeRemaining < CURETIME_REQUIRED_HYPOTHERMIA) {
				_hypothermia_timeRemaining += timeGap * 3f;
			}

			current_time = GameManager.singleton.TotalPlayTime;
			yield return new WaitForSeconds(0.1f);
		}
		_is_hypothermia = false;
		_is_hypothermia_curing = false;
		_hypothermia_timeRemaining = 0f;
		staminaMGR.rate_stamina_usage /= HYPOTHERMIA_RATE_STAMINA_USAGE_INCREASE; 
		
	}

	public void Incur_Bleeding(int occuredBleedingState) {
		if (occuredBleedingState >= bleeding_state) {
			switch (occuredBleedingState) {
				case 1: _bleeding_timeRemaining = CURETIME_REQUIRED_BLEEDING_1; break;
				case 2: _bleeding_timeRemaining = CURETIME_REQUIRED_BLEEDING_2; break;
				case 3: _bleeding_timeRemaining = CURETIME_REQUIRED_BLEEDING_3; break;
			}
			StartCoroutine(Bleeding_Coroutine());
		}
	}
	IEnumerator Bleeding_Coroutine() {
		float current_time = GameManager.singleton.TotalPlayTime;
		float timeGap;
		float harm = HARM_BLEEDING_3_PER_HOUR;
		while (_bleeding_timeRemaining > 0) {
			if (bleeding_state != 1 && _bleeding_timeRemaining < CURETIME_REQUIRED_BLEEDING_1) {
				bleeding_state = 1;
				harm = HARM_BLEEDING_1_PER_HOUR;
			} else if (bleeding_state != 2 && _bleeding_timeRemaining < CURETIME_REQUIRED_BLEEDING_2) {
				bleeding_state = 2;
				harm = HARM_BLEEDING_2_PER_HOUR;
			}
			
			timeGap = GameManager.singleton.TotalPlayTime - current_time;

			// 지혈 시 건강감소 중지, 회복 시작
			if (_is_bleeding_curing) {
				/* if(더러운 붕대) 패혈증 확률 적용
				 */
				/* if(격한 행동) 재출혈 확률 적용
				 */
				_bleeding_timeRemaining -= timeGap;
			} else {
				playerProp.health -= harm * timeGap / 3600f;
				healthMGR.Player_Health_Changed();
			}
			current_time = GameManager.singleton.TotalPlayTime;
			yield return new WaitForSeconds(10f * Time.deltaTime);
		}
		bleeding_state = 0;
		_is_bleeding_curing = false;
		_bleeding_timeRemaining = 0f;
	}
	

	public void Incur_BloodPoisoning() {
		if (_is_bloodPoisoning) {
			_is_bloodPoisoning_duplicated = true;
		} else {
			_is_bloodPoisoning = true;
		}
		_bloodPoisoning_timeRemaining = CURETIME_REQUIRED_BLOODPOISONING;
		StartCoroutine(BloodPoisoning_Coroutine());
	}

	IEnumerator BloodPoisoning_Coroutine() {
		float current_time = GameManager.singleton.TotalPlayTime;
		float timeGap;
		while (_bloodPoisoning_timeRemaining > 0f) {
			if (_is_bloodPoisoning_duplicated) {
				_is_bloodPoisoning_duplicated = false;
				yield break;
			}
			timeGap = GameManager.singleton.TotalPlayTime - current_time;
			playerProp.health -= HARM_BLOODPOISONING * timeGap / 3600f;
			healthMGR.Player_Health_Changed();

			// 항생제 투여가 중단되면 감염 재확산
			if (_is_bloodPoisoning_curing) {
				_bloodPoisoning_timeRemaining -= timeGap;
			} else {
				_bloodPoisoning_timeRemaining += 0.5f * timeGap;
			}
			current_time = GameManager.singleton.TotalPlayTime;
			yield return new WaitForSeconds(10f * Time.deltaTime);
		}
		_is_bloodPoisoning = false;
		_bloodPoisoning_timeRemaining = 0f;
	}

	public void Use_Antibiotics(Med_ItemData _itemData) {
		
		if(_is_foodPoisoning && _itemData.Cure_acceleration > _foodPoisoning_cure_acceleration) {
			_is_foodPoisoning_curing = true;
			_foodPoisoning_cure_acceleration = _itemData.Cure_acceleration;
			_foodPoisoning_med_duration = _itemData.Med_duration;
		}

		if (_is_bloodPoisoning && _itemData.Cure_acceleration > _bloodPoisoning_cure_acceleration) {
			_is_bloodPoisoning_curing = true;
			_bloodPoisoning_cure_acceleration = _itemData.Cure_acceleration;
			_bloodPoisoning_med_duration = _itemData.Med_duration;
		}
		
	}

	public void Use_Bandage() {
		switch(bleeding_state) {
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
		}
	}
	
}
