using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPropMGR : MonoBehaviour
{
	public PlayerProp playerProp;
	public PlayerMovement playerMovement;
	public InteractMGR interactMGR;
	public DigestiveMGR digestiveMGR;
	public ExhaustionMGR exhaustionMGR;
	public MentalityMGR mentalityMGR;
	public TemperatureMGR temperatureMGR;

	public float health_recovery_acceleration = 1f;
	public float immunity_recovery_acceleration = 1f;
	public float hunger_diminishment_acceleration = 1f;
	public float calories_diminishment_acceleration = 1f;
	public float thirst_diminishment_acceleration = 1f;

	bool _is_hunger_is_zero = false;
	bool _is_thirst_is_zero = false;


	bool _is__playerProp_adjust_coroutine__running = false;
	bool _is__player_sleeping_coroutine__running = false;

	Coroutine _playerProp_adjust_coroutine;
	Coroutine _player_sleeping_coroutine;

	float _sleep_exhaustion_recovery_per_hour;

	public const float ADJUST_DELAY_WFS = 0.5f;
	
	public const float HEALTH_DIMINISHMENT_BY_LACK_OF_CALORIES_PER_HOUR = 100f / (10);
	public const float HEALTH_DIMINISHMENT_BY_LACK_OF_THIRST_PER_HOUR = 100f / (3 * 24);
	public const float HEALTH_DIMINISHMENT_BY_LACK_OF_TEMPERATURE_PER_HOUR = 100f / (1 * 24);

	void Start()
    {
		_is__playerProp_adjust_coroutine__running = true;
		_playerProp_adjust_coroutine = StartCoroutine(PlayerProp_Adjust_Coroutine());
    }
    
    void Update()
    {
        
    }
	IEnumerator PlayerProp_Adjust_Coroutine() {
		float _currentTime;
		float _timeGap;
		WaitForSeconds _wfs = new WaitForSeconds(ADJUST_DELAY_WFS);
		_currentTime = GameManager.singleton.TotalPlayTime;
		while (_is__playerProp_adjust_coroutine__running) {
			_timeGap = GameManager.singleton.TotalPlayTime - _currentTime;

			digestiveMGR.Digest_Procedure(_timeGap);
			digestiveMGR.Diminish_Calories(_timeGap);
			digestiveMGR.Adjust_Body_Weight(_timeGap);
			exhaustionMGR.Adjust_Exhaustion(_timeGap);
			

			_currentTime = GameManager.singleton.TotalPlayTime;
			yield return _wfs;
		}
	}


}
