using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMGR : MonoBehaviour {

	public bool approve_attack = true;
	public bool battle_ingauged = false;

	public PlayerProp playerProp;
	public StaminaMGR staminaMGR;
	public PlayerMovement playerMovement;
	public InteractMGR interactMGR;
	public GameObject player_obj;

	bool _cooldown = true;

	public float weapon_damage; // 무기 대미지
	public float weapon_stamina_usage; // statminaMGR와 연결해서 스태미나감소속도 차이 적용
	public float weapon_use_delay; // 무기의 공격 간 시간차
	public float weapon_range; // 무기의 공격 범위
	public float weapon_knockback; // 밀쳐내는 거리
	public int weapon_num_hits; // 1회 공격 당 타수
	public float weapon_timeGap_hits; // 타수 적용 시 대미지 출력 간격
	public int weapon_num_motionType; // 무기의 공격 모션 가짓수

	public GameObject prefab_hatchetMotion;
	public GameObject hatchetMotion_instance = null;

	float _attack_delay;
	float _attack_spectrum;

	float _ingauge_battle_timer = 0f;
	bool _is_Ingauge_Battle_Coroutine_running = false;
	
	/* ATTACK_SPECTRUM : 공격모션의 잔상 시간
	 * INGAUGED_BATTLE_DURATION : 전투 상태로부터 비전투중 상태가 될 때까지 필요한 시간
	 */
	public const float ATTACK_SPECTRUM = 0.3f;
	public const float INGAUGED_BATTLE_DURATION = 180f;

	void Start() {

	}

	void Update() {
		if (Input.GetKey(UserKeyInfo.attack) && approve_attack) {
			if (_cooldown && playerProp.current_stamina >= weapon_stamina_usage) {
				StartCoroutine(Using_Weapon());
			}
		}
	}

	public void Restrict_Attack() {
		approve_attack = false;
	}
	public void Approve_Attack() {
		approve_attack = true;
	}

	public int Return_Num_MotionType() {
		return Random.Range(1, weapon_num_motionType + 1);
	}

	// interactMGR의 Approve_Interact 와 연동하기!!
	public void Ingauge_Battle() {
		_ingauge_battle_timer = INGAUGED_BATTLE_DURATION;
		if (!_is_Ingauge_Battle_Coroutine_running) {
			StartCoroutine(Ingauge_Battle_Coroutine());
		}
	}

	IEnumerator Ingauge_Battle_Coroutine() {
		_is_Ingauge_Battle_Coroutine_running = true;
		interactMGR.Restrict_Interact(); // 전투중일 시 상호작용 불가
		battle_ingauged = true;
		while(_ingauge_battle_timer > 0) {
			_ingauge_battle_timer -= GameManager.singleton.timeSpeed_magnification * Time.deltaTime;
			yield return null;
		}
		_ingauge_battle_timer = 0f;
		battle_ingauged = false;
		interactMGR.Approve_Interact(); // 전투종료 시 다시 상호작용 가능
		_is_Ingauge_Battle_Coroutine_running = false;
	}

	IEnumerator Using_Weapon() {
		_cooldown = false;
		Ingauge_Battle();
		Vector3 _rangeMultAngle = weapon_range * playerMovement.playerAngleToVector3_normalized;
		Vector3 _appearPosition = player_obj.transform.position + _rangeMultAngle;
		hatchetMotion_instance = Instantiate(prefab_hatchetMotion, _appearPosition, player_obj.transform.rotation);
		yield return null;
		hatchetMotion_instance.GetComponent<PlayerAnimation>().AttackMotion_Started();
		playerProp.current_stamina -= weapon_stamina_usage;

		if(staminaMGR.is_in_groggy_state) {
			_attack_spectrum = ATTACK_SPECTRUM * 1.43f; // 애니메이션의 그로기속도인 0.7의 역수
			_attack_delay = weapon_use_delay * 2f; // 그로기상태에서 행동간격은 2배가 됨
		} else {
			_attack_spectrum = ATTACK_SPECTRUM;
			_attack_delay = weapon_use_delay;
		}

		float timer = _attack_spectrum;
		while (timer > 0f) {
			hatchetMotion_instance.transform.position = player_obj.transform.position + _rangeMultAngle;
			timer -= Time.deltaTime;
			yield return null;
		}
		Destroy(hatchetMotion_instance);
		yield return new WaitForSeconds(_attack_delay - _attack_spectrum);
		_cooldown = true;
	}


	GameObject WhichWeapon() {
		if (true) {
			return hatchetMotion_instance;
		}
	}



	
}
