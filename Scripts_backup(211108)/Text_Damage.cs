using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Text_Damage : MonoBehaviour
{
	public Canvas canvas;
	public GameObject text_damage;
	

	public void PopUp_Text_Damage(Vector3 hitPoint, float hitDamage) {
		text_damage = Instantiate(text_damage, hitPoint, Quaternion.identity, canvas.transform);
		Destroy_text_damage();
	}

	IEnumerator Destroy_text_damage() {
		yield return new WaitForSeconds(1.5f);
		Destroy(gameObject);
	}
	void Start()
    {
		canvas = GameObject.FindObjectOfType<Canvas>();
    }

    void Update()
    {
        
    }
}
