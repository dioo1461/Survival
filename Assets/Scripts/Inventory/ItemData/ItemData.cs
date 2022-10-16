using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemCodeEnumData {
	public enum ItemCodeEnum {
		None = 0,
		Bandage = 1,
		Branch = 2,
		Cloth = 3,
		Herb = 4,
		Lighter = 5,
		Log = 6,
		Rock = 7,
		Tatters = 8,

	};
}

namespace ItemTypeEnumData {
	public enum ItemTypeEnum {
		Empty = 0,
		Weapon = 1,
		Tool = 2,
		Material = 3,
		Food = 4,
		Med = 5,

		Error = 999
	};
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item/Item", order = 1)]
public class ItemData : ScriptableObject {
	public int ID => _id;
	public string Name => _name;
	public int Max_amount => _max_amount;
	public float Weight => _weight;
	public string Tooltip => _tooltip;
	public Sprite IconSprite => _iconSprite;
	public Sprite ImageSprite => _imageSprite;

	[SerializeField] int _id;
	[SerializeField] string _name;
	[SerializeField] int _max_amount;
	[SerializeField] float _weight;
	[TextArea(3,5)]
	[SerializeField] string _tooltip;
	[SerializeField] Sprite _iconSprite;
	[SerializeField] Sprite _imageSprite;
	
}

/*public class Usable_ItemData : ItemData {

}*/

/*[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Tool_Item", order = 2)]
public class Tool_ItemData : Usable_ItemData {
	public float Durability_diminishment => _durability_diminishment;
	public float Break_chance_under50 => _break_chance_under50;
	public float Break_chance_under20 => _break_chance_under20;
	[Header("Tool Item Property")]
	[SerializeField] float _durability_diminishment;
	[SerializeField] float _break_chance_under50;
	[SerializeField] float _break_chance_under20;

	*//*public override Item CreateItem() {
		return new Tool_Item(this);
	}*//*
}*/

/*[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Food_Item", order = 2)]
public class Food_ItemData : Usable_ItemData {
	public float Hunger_recover_amount => _hunger_recover_amount;
	public float Thirst_recover_amount => _thirst_recover_amount;
	public float Spoil_chance_per_hour => _spoil_chance_per_hour; // 단위 : %
	public int ID_spoiled_item => _id_spoiled_item;
	
	[Header("Food Item Property")]
	[SerializeField] float _hunger_recover_amount; // 허기 회복량
	[SerializeField] float _thirst_recover_amount; // 갈증 회복량
	[SerializeField] float _spoil_chance_per_hour; // 시간당 부패 확률
	[SerializeField] int _id_spoiled_item; // 부패 시 생성되는 아이템

	*//*public override Item CreateItem() {
		return new Tool_Item(this);
	}*//*
}*/



