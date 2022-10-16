using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrintDamage : MonoBehaviour
{
	string _damage;
	TextMesh textMesh;

	public void Print_Damage(string damage) {
		textMesh = gameObject.GetComponent<TextMesh>();
		_damage = damage;
		StartCoroutine(Print_Damage_Coroutine());
	}
	IEnumerator Print_Damage_Coroutine() {
		textMesh.text = _damage;
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}
