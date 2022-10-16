using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigestiveMGR : MonoBehaviour
{
	public PlayerProp playerProp;
	public PlayerMovement playerMovement;
	public InteractMGR interactMGR;

	bool _is_hunger_zero = false;
	bool _is_thirst_zero = false;

	public const float HUNGER_DIMINISHMENT_PER_HOUR = 100f / 10;
	public const float CALORIES_DIMINISHMENT_PER_HOUR = 1700f / 24;
	public const float CALORIES_DIMINISHMENT_PER_HOUR_WHEN_WALKING = 300f;
	public const float CALORIES_DIMINISHMENT_PER_HOUR_WHEN_RUNNING = 900f;
	public const float THIRST_DIMINISHMENT_PER_HOUR = 100f / 18;

	public const float WEIGHT_TO_CALORIES_CONVERSION_PER_HOUR = 30f;
	public const float WEIGHT_TO_CALORIES_CONVERSION_WHEN_STARVING_PER_HOUR = 100f;

	/// <summary> 소화 과정을 표현한 함수. 음식 섭취 시 hunger와 digesting_calories가 증가하고,
	// 소화가 진행됨에 따라 calories가 증가함. hunger가 0이 될 때 digesting_calories도 0이 됨.
	/// </summary>
	public void Digest_Procedure(float timeGap) {
		if (playerProp.hunger > 0) {
			if (_is_hunger_zero) {
				_is_hunger_zero = false;
				//playerProp.power *= 100 / 75;
			}
			float _hunger_diminished = timeGap * HUNGER_DIMINISHMENT_PER_HOUR / 3600f;
			if (_hunger_diminished > playerProp.hunger) {
				_hunger_diminished = playerProp.hunger;
			}
			float _calories_digested = playerProp.digesting_calories * (_hunger_diminished / playerProp.hunger);
			playerProp.hunger -= _hunger_diminished;
			playerProp.digesting_calories -= _calories_digested;
			if (playerProp.calories < 5000) {
				playerProp.calories += _calories_digested;
			}

		} else if (!_is_hunger_zero) {
			_is_hunger_zero = true;
			//playerProp.power *= 75 / 100;
			playerProp.hunger = 0f;
			playerProp.calories += playerProp.digesting_calories;
			playerProp.digesting_calories = 0f;
		}
	}

	public void Diminish_Thirst(float timeGap) {
		if (playerProp.thirst > 0) {
			if (_is_thirst_zero) {
				_is_thirst_zero = false;
				// playerProp.power *= 100 / 75;
			}
			float _thirst_diminished = timeGap * THIRST_DIMINISHMENT_PER_HOUR / 3600f;
			if (_thirst_diminished > playerProp.thirst) {
				_thirst_diminished = playerProp.thirst;
			}
			playerProp.thirst -= _thirst_diminished;

		} else if (!_is_thirst_zero) {
			_is_thirst_zero = true;
			// playerProp.power *= 75 / 100;
			playerProp.thirst = 0f;
		}
	}
	public void Diminish_Calories(float timeGap) {
		float _calories_burned = timeGap * CALORIES_DIMINISHMENT_PER_HOUR / 3600f;
		if (playerMovement.playerMovingState == PlayerMovement.PlayerMovingStateEnum.walking || playerMovement.playerMovingState == PlayerMovement.PlayerMovingStateEnum.backstep) {
			_calories_burned += timeGap * CALORIES_DIMINISHMENT_PER_HOUR_WHEN_WALKING / 3600f;
		} else if (playerMovement.playerMovingState == PlayerMovement.PlayerMovingStateEnum.running) {
			_calories_burned += timeGap * CALORIES_DIMINISHMENT_PER_HOUR_WHEN_RUNNING / 3600f;
		} else if (interactMGR.current_interacting_instance != null) {
			_calories_burned += timeGap * interactMGR.current_interacting_instance.GetComponent<ObjectProp>().Data.Calories_required_per_hour / 3600f;
		}
		playerProp.calories -= _calories_burned;

	}


	public void Adjust_Body_Weight(float timeGap) {
		float _calories_change;
		if (playerProp.calories > 3000 && playerProp.body_weight < 140f) {
			_calories_change = timeGap * WEIGHT_TO_CALORIES_CONVERSION_PER_HOUR / 3600f;
			playerProp.calories -= _calories_change;
			playerProp.body_weight += _calories_change / 7000f;
		} else if (playerProp.calories < 500 && playerProp.body_weight > 40f) {
			_calories_change = timeGap * WEIGHT_TO_CALORIES_CONVERSION_WHEN_STARVING_PER_HOUR / 3600f;
			playerProp.calories += _calories_change;
			playerProp.body_weight -= _calories_change / 7000f;
			// ###### 시간이 한번에 많이 지나면 전환량이 비정상적으로 증가하는 문제.
			// 해결 위해 임의의 공식을 대입하거나 시간이 한번에 많이 지나가지 않게 설계 ######
		} else if (playerProp.calories < 1000 && playerProp.body_weight > 40f) {
			_calories_change = timeGap * WEIGHT_TO_CALORIES_CONVERSION_PER_HOUR / 3600f;
			playerProp.calories += _calories_change;
			playerProp.body_weight -= _calories_change / 7000f;
		}
	}

}
