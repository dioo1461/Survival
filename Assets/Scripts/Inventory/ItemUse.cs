using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUse : MonoBehaviour
{
	public static ItemUse singleton;

	public PlayerProp playerProp;
	public DiseasesMGR diseasesMGR;
	public AttackMGR attackMGR;

	void Start() {
		singleton = this;
	}


	public void Use_Liquid_Item(Liquid_ItemProp _itemProp) {

	}

	public void Use_Food_Item(Food_ItemProp _itemProp) {
		if (_itemProp.Amount <= 0) {
			return;
		}
		// 허기가 90 이상이면 아이템 사용 제한
		if (playerProp.hunger > 90) {
			return;
		// 아이템 사용 시 허기가 max를 넘게 될 경우, 사용 후 허기를 max로 조정
		} else if (_itemProp.Data.Hunger_gain_amount + playerProp.hunger > playerProp.max_hunger) {
			_itemProp.Amount--;
			playerProp.hunger = playerProp.max_hunger;
			playerProp.digesting_calories += (_itemProp).Data.Calories_gain_amount;
			return;
		}
		_itemProp.Amount--;
		playerProp.hunger += (_itemProp).Data.Hunger_gain_amount;
		playerProp.digesting_calories += (_itemProp).Data.Calories_gain_amount;
	}

	public void Use_Med_Item(Med_ItemProp _itemProp) {
		if (_itemProp.Amount < 1) {
			return;
		}

		_itemProp.Amount--;
		switch(_itemProp.Data.Med_type) {
			case "Antibiotics" :
				diseasesMGR.Use_Antibiotics(_itemProp.Data);
				break;
			case "Bandage":
				diseasesMGR.Use_Bandage();
				break;
			
			
		}
	}

	public void Use_Weapon_Item(Tool_ItemProp _itemProp) {
		if (_itemProp.Amount < 1) {
			return;
		}

		
	}

	bool Simulate_Possibility (float possibility) {
		float _rand = Random.Range(0f, 100f);
		if (_rand < possibility) {
			return true;
		} else {
			return false;
		}
	}
}
