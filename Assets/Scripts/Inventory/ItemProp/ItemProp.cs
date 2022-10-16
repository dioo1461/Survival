using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemTypeEnumData;

public class ItemProp {
	public ItemData Data { get; private set; }
	public int Amount { get; set; } // 아이템 개수
	public ItemProp(ItemData data, int amount) {
		Data = data;
		Set_Amount(amount);
	}

	/// <summary> 아이템 개수를 설정, 만약 최대 적재량을 초과하면 수량을 최대로 설정
	/// </summary>
	public void Set_Amount(int amount) => Amount = Mathf.Clamp(amount, 0, Data.Max_amount);

	/// <summary> 수량을 더했을 때 수용량을 초과하면, 남은 아이템 개수를 최대로 만들고
	/// 초과한 개수를 반환 </summary>
	public int Add_Amount_And_Get_Excess(int add_amount) {
		int _totalAmount = Amount + add_amount;
		int _excessAmount = _totalAmount - Data.Max_amount;
		Set_Amount(add_amount);
		return (_excessAmount > 0) ? _excessAmount : 0;
	}

	/// <summary> 아이템 개수를 매개변수만큼 감소시키고 true 반환.
	/// 남은 아이템 개수가 감소시킬 수량보다 적으면, 개수를 감소시키지
	/// 않고 false를 반환 </summary> ///
	public bool Subtract_Amount_If_Possible(int sub_amount) {
		if (Amount - sub_amount < 0) {
			return false;
		} else {
			Amount -= sub_amount;
			return true;
		}

	}

	public int Get_Remain_Space_Size() {
		return Data.Max_amount - Amount;
	}
	public ItemTypeEnum Get_Item_Type() {
		if (Data.ID == 0) {
			return ItemTypeEnum.Empty;
		} else if (1 <= Data.ID && Data.ID <= 999) {
			return ItemTypeEnum.Weapon;
		} else if (1000 <= Data.ID && Data.ID <= 1999) {
			return ItemTypeEnum.Tool;
		} else if (2000 <= Data.ID && Data.ID <= 2999) {
			return ItemTypeEnum.Material;
		} else if (3000 <= Data.ID && Data.ID <= 3999) {
			return ItemTypeEnum.Food;
		}
		return ItemTypeEnum.Error;
	}
	public T Get_Copy<T>() {
		return (T)this.MemberwiseClone();
	}

	public void Apply_Spoilage_Chance(float _temp) { }
	
}

