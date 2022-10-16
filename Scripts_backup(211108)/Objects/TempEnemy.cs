using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour
{
	public Rigidbody2D tempEnemy_rigid2D;
	Text_Damage text_damage;
	HealthMNG healthMNG;
	AttackMotions attackMotions;
	public float hp = 50f;
	public float moveSpeed;
	public float damage = 15f;
	

    void Start()
    {
		healthMNG = GameObject.Find("HealthBar").GetComponent<HealthMNG>();
		attackMotions = GameObject.Find("AttackMotions").GetComponent<AttackMotions>();
		text_damage = GameObject.Find("Text_Damage").GetComponent<Text_Damage>();
    }

    void Update()
    {
        
    }

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.name == "Player") {
			healthMNG.Player_Damaged(damage);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.name == "HatchetMotion") {
			float dmg_taken = Random.Range(attackMotions.hatchet_damage - 3f, attackMotions.hatchet_damage + 3f);
			hp -= dmg_taken;
			text_damage.PopUp_Text_Damage(transform.position, dmg_taken);
		}
		if(hp <= 0f) {
			Destroy(gameObject);
		}
	}

}
