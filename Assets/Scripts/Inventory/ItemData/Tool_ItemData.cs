using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item/Tool_Item", order = 2)]
public class Tool_ItemData : Usable_ItemData
{
	public float Durability_diminishment => _durability_diminishment;
	public float Break_chance_under50 => _break_chance_under50;
	public float Break_chance_under20 => _break_chance_under20;
	[Header("Tool Item Property")]
	[SerializeField] float _durability_diminishment;
	[SerializeField] float _break_chance_under50;
	[SerializeField] float _break_chance_under20;

	/*public override Item CreateItem() {
		return new Tool_Item(this);
	}*/
}
