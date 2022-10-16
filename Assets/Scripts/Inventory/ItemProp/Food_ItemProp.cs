using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_ItemProp : Usable_ItemProp {
	public new Food_ItemData Data { get; private set; }
	public ItemData itemData_spoiled_item => Data.ItemData_spoiled_item; // 상했을 때 분리될 아이템 프리팹

	/*int _edible_item_count; // 멀쩡한 아이템 개수*/
	/*int _spoiled_item_count; // 상한 아이템 개수*/


	public Food_ItemProp(Food_ItemData data, int amount) : base(data, amount) {
		Data = data;
		Amount = amount;

	}
	
	/// <summary> 아이템 수량이 1 이상이면 수량을 1만큼 감소시키고 true 반환, 아니라면 false 반환
	/// </summary>
	public bool Use_Item() {
		if (Subtract_Amount_If_Possible(1)) {
			return true;
		} else {
			return false;
		}
	}
	
	/// <summary> 부패 확률을 적용하여 부패된 아이템 개수를 확정
	/// </summary>
	/*new public void Apply_Spoilage_Chance(float time_elapsed) {
		int _recentSpoiledItemCount = 0;
		for (int i = 0; i <_edible_item_count; i++) {
			float _RandValue = Random.Range(0f, 100f);
			if (_RandValue < Data.Spoil_chance_per_hour) {
				_recentSpoiledItemCount++;
			}
		}
		*//*_edible_item_count -= _recentSpoiledItemCount;
		_spoiled_item_count += _recentSpoiledItemCount;*//*
	}*/
	
	/// <summary> 부패한 아이템 개수를 0으로 만들고, 원래 남아있던 수량을 반환
	/// </summary>
	/*public int Remove_Spoiled_Item() {
		int _temp = _spoiled_item_count;
		_spoiled_item_count = 0;
		return _temp;
	}*/

}
