using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_DamagePopUp : MonoBehaviour
{
	public GameObject prefab_text_damagePopUp;
	PrintDamage printDamage;

	public void PopUp_Text_Damage(Vector3 PopUpPosition, float hitDamage) {
		printDamage = Instantiate(prefab_text_damagePopUp, PopUpPosition, Quaternion.identity).GetComponent<PrintDamage>();
		printDamage.Print_Damage(hitDamage.ToString("F0"));
	}

	void Awake()
    {
	
    }

    void Update()
    {
        
    }
}
