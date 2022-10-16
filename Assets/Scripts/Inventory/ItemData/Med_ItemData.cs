using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item/Med_Item", order = 1)]
public class Med_ItemData : Usable_ItemData
{
	public string Med_type => med_type;
	public float Cure_acceleration => cure_acceleration;
	public float Med_duration => med_duration;

	[Header("Med Item Info")]
	[SerializeField] string med_type;
	[SerializeField] float cure_acceleration;
	[SerializeField] float med_duration;

}
