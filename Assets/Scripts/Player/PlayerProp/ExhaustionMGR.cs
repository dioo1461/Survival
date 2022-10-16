using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustionMGR : MonoBehaviour
{
	public PlayerProp playerProp;

	float _sleep_exhaustion_recovery_per_hour;
	Coroutine _player_sleeping_coroutine;
	Coroutine _calculate_faint_possibility_coroutine;
	bool _is__player_sleeping_coroutine__running = false;
	bool _is__calculate_faint_possibility_coroutine__running = false;

	const float EXHAUSTION_DIMINISHMENT_PER_HOUR = 100f / 24;
	const float FAINT_MAX_EXHAUSTION = 10f; // 실신할 확률이 존재하는 최대 피로도
	const float FAINT_POSSIBILITY_PER_MIN = 7.5f; // 분당 실신 확률(조건을 만족할 때)

	const float TIMESPEED_ACCELERATION_WHEN_SLEEPING = 30f;
	const float TIMESPEED_ACCELERATION_WHEN_DEEP_SLEEPING = 60f;

	void Start() {
		_is__calculate_faint_possibility_coroutine__running = true;
		_calculate_faint_possibility_coroutine = StartCoroutine(Calculate_Faint_Possibility_Coroutine());
	}

	public void Adjust_Exhaustion(float timeGap) {
		if (playerProp.exhaustion > 0) {
			playerProp.exhaustion -= timeGap * EXHAUSTION_DIMINISHMENT_PER_HOUR / 3600f;
		} else {
			playerProp.exhaustion = 0f;
		}
	}

	public void Start_Sleeping(float exhaustion_recovery_per_hour, bool is_faint) {
		_sleep_exhaustion_recovery_per_hour = exhaustion_recovery_per_hour;
		GameManager.singleton.Set_TimeSpeed_Acceleration(TIMESPEED_ACCELERATION_WHEN_SLEEPING);
		_is__player_sleeping_coroutine__running = true;
		_player_sleeping_coroutine = StartCoroutine(Player_Sleeping_Coroutine());
		_is__calculate_faint_possibility_coroutine__running = false;
		StopCoroutine(_calculate_faint_possibility_coroutine);
	}

	public void Stop_Sleeping() {
		GameManager.singleton.Set_TimeSpeed_Default();
		_is__player_sleeping_coroutine__running = false;
		StopCoroutine(_player_sleeping_coroutine);
		_is__calculate_faint_possibility_coroutine__running = true;
		_calculate_faint_possibility_coroutine = StartCoroutine(Calculate_Faint_Possibility_Coroutine());
	}
	IEnumerator Player_Sleeping_Coroutine() {
		float _timeGap;
		float _currentTime = GameManager.singleton.TotalPlayTime;
		WaitForSeconds _wfs = new WaitForSeconds(PlayerPropMGR.ADJUST_DELAY_WFS);
		while (_is__player_sleeping_coroutine__running) {
			_timeGap = GameManager.singleton.TotalPlayTime - _currentTime;


			
			_currentTime = GameManager.singleton.TotalPlayTime;
			yield return _wfs;
		}
	}
	
	void Start_Deep_Sleep() {

	}
	
	void Calculate_Attack_Possibility_While_Sleeping() {

	}

	IEnumerator Calculate_Faint_Possibility_Coroutine() {
		float _accumulated_time = 0f;
		float _currentTime = GameManager.singleton.TotalPlayTime;
		WaitForSeconds _wfs = new WaitForSeconds(PlayerPropMGR.ADJUST_DELAY_WFS);
		while (_is__calculate_faint_possibility_coroutine__running) {
			_accumulated_time += GameManager.singleton.TotalPlayTime - _currentTime;
			if (Calculate_Faint_Possibilty(ref _accumulated_time)) {
				Start_Deep_Sleep();
				yield break;
			}
			_currentTime = GameManager.singleton.TotalPlayTime;
			yield return _wfs;
		}

	}

	bool Calculate_Faint_Possibilty(ref float time) {
		for (float i = time; i >= 60f; i -= 60f) {
			time -= 60f;
			if (Random.Range(0, 100f) < FAINT_POSSIBILITY_PER_MIN) {
				Start_Deep_Sleep();
				return true;
			}
		}
		return false;
	}
	
}
