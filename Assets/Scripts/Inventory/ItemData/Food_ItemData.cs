using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item/Food_Item", order = 2)]
public class Food_ItemData : Usable_ItemData {
	public float Hunger_gain_amount => _hunger_gain_amount;
	public float Calories_gain_amount => _calories_gain_amount;
	public float Thirst_gain_amount => _thirst_gain_amount;
	public float Health_gain_amount => _health_gain_amount;
	public float Mentality_gain_amount => _mentality_gain_amount;
	public float Spoil_chance_per_hour => _spoil_possibility_per_hour; // 단위 : %
	public Food_ItemData ItemData_spoiled_item => _itemData_spoiled_item;

	[Header("Food Item Info")]
	[SerializeField] float _hunger_gain_amount; // 허기 회복량
	[SerializeField] float _calories_gain_amount; // 허기 회복량
	[SerializeField] float _thirst_gain_amount; // 갈증 회복량
	[SerializeField] float _health_gain_amount; // 건강 회복량
	[SerializeField] float _mentality_gain_amount; // 정신력 회복량
	[SerializeField] float _spoil_possibility_per_hour; // 시간당 부패 확률
	[SerializeField] Food_ItemData _itemData_spoiled_item; // 부패 시 생성되는 아이템

	/*public override Item CreateItem() {
		return new Tool_Item(this);
	}*/
}
