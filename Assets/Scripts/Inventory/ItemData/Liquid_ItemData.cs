using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item/Liquid_Item", order = 1)]
public class Liquid_ItemData : Food_ItemData
{
	public ItemData Vessel_data => _vessel_data;

	[Header("Vessel Item Info")]
	[SerializeField] ItemData _vessel_data;


}
