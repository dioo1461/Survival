using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobMGR : MonoBehaviour {
	ProficiencyMGR proficiencyMGR;
	Rigidbody2D this_Rigid2D;
	Text_DamagePopUp text_damagePopUp;
	HealthMGR healthMGR;
	AttackMGR attackMGR;
	PlayerMovement playerMovement;
	string mob_name;
	float mob_hp;
	float mob_moveSpeed;
	float mob_damage;
	float mob_exp;

	bool can_damaged = true;
	bool can_damaged_tic = true;
	int _num_hits;

	bool is_coroutine_running = false;
	

	void Start() {
		this_Rigid2D = gameObject.GetComponent<Rigidbody2D>();
		healthMGR = GameObject.Find("HealthBar").GetComponent<HealthMGR>();
		attackMGR = GameObject.Find("AttackMGR").GetComponent<AttackMGR>();
		text_damagePopUp = GameObject.Find("Text_DamagePopUp").GetComponent<Text_DamagePopUp>();
		playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
	}

	public void Init_Mob(string name, float hp, float moveSpeed, float damage, int exp) {
		mob_name = name;
		mob_hp = hp;
		mob_moveSpeed = moveSpeed;
		mob_damage = damage;
		mob_exp = exp;
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.CompareTag("Player")) {
			healthMGR.Player_Damaged(mob_damage);
		}
	}
	void OnTriggerEnter2D(Collider2D coll) {
		if (can_damaged && can_damaged_tic && coll.gameObject.CompareTag("PlayerAttack")) {
			if (!is_coroutine_running) {
				//StopCoroutine(Mob_Pushoff_Coroutine());
				//StartCoroutine(Mob_Pushoff_Coroutine());
				//this_Rigid2D.velocity += attackMGR.weapon_knockback * (Vector2)playerMovement.playerAngleToVector3_normalized; ;
				StartCoroutine(Prevent_Duplicated_Hits_Coroutine());
				StartCoroutine(Mob_Damaged_Coroutine());

			}
		}
	}

	IEnumerator Prevent_Duplicated_Hits_Coroutine() {
		is_coroutine_running = true;
		_num_hits = attackMGR.weapon_num_hits;
		float timer = attackMGR.weapon_use_delay;
		while (timer > 0) {
			if (_num_hits <= 0) {
				can_damaged = false;
			}
			timer -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		can_damaged = true;
		is_coroutine_running = false;
	}

	IEnumerator Mob_Damaged_Coroutine() {
		for (int i = _num_hits; i > 0; i--) {
			_num_hits--;
			float _dmg_taken = Random.Range(attackMGR.weapon_damage - 3f, attackMGR.weapon_damage + 3f);
			mob_hp -= _dmg_taken;
			Vector3 text_damage_popUpPosition = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1f, 2f), 0);
			text_damagePopUp.PopUp_Text_Damage(text_damage_popUpPosition, _dmg_taken);
			if (mob_hp <= 0f) {
				Destroy(gameObject);
			}
			yield return new WaitForSeconds(attackMGR.weapon_timeGap_hits);
		}
	}

	/* IEnumerator Mob_Knockback_Coroutine() {
		Vector3 prior_velocity = transform.position + attackMGR.weapon_thrust * playerMovement.playerAngleToVector3_normalized;
		float position_gap_x, position_gap_y;
		Vector3 
		this_Rigid2D.velocity += attackMGR.weapon_thrust * (Vector2)playerMovement.playerAngleToVector3_normalized;
		do {
			this_Rigid2D.velocity -= Mathf.Lerp(attackMGR.weapon_thrust * (Vector2)playerMovement.playerAngleToVector3_normalized;
			if (target_position.x - transform.position.x < 0) {
				position_gap_x = -(target_position.x - transform.position.x);
			} else {
				position_gap_x = target_position.x - transform.position.x;
			}
			yield return null;
		} while (position_gap_x > 0.01f || position_gap_y > 0.01f);
		
		transform.position = target_position;
	} */

	



}

