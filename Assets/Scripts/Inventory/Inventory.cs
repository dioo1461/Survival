using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemTypeEnumData;

public class Inventory : MonoBehaviour
{

	/* 슬롯 동적 생성과 관련된 코드
	 * 
	public GameObject prefab_inventorySlot; // 슬롯 프리팹
	public RectTransform _contentAreaRect; // 슬롯들이 위치할 영역

	[Header("A")] [Range(0, 10)] int _horizontalSlotCount = 10; // 슬롯 가로개수
	[Range(0, 10)] int _verticalSlotCount = 10; // 슬롯 세로개수
	float _slotMargin = 8f; // 슬롯 상하좌우 여백
	float _contentAreaPadding = 20f; // 인벤토리 영역 내부여백
	[Range(32,64)] float _slotSize = 64f; // 각 슬롯의 가로세로 길이
	*/
	/*void Init_Slots() {
	prefab_inventorySlot.TryGetComponent(out RectTransform _slotRect);
	_slotRect.sizeDelta = new Vector2(_slotSize, _slotSize);
	prefab_inventorySlot.TryGetComponent(out )
}*/

	public int num_slots = 28;

	public ItemProp[] slots; // 슬롯 관리 배열, 사용 가능 index: 1 ~ num_slots

	public float total_weight;

	float _last_accessed_time;

	void Start() {
		slots = new ItemProp[num_slots + 1];
		for (int i = 1; i <= num_slots; i++) {
			Init_Slot(ItemDataStruct.singleton.empty, i, 0);
		}
		ItemProp _temp;
		_temp = new ItemProp(ItemDataStruct.singleton.branch, 3);
		Init_Slot(_temp, 1, 3);
		_temp = new ItemProp(ItemDataStruct.singleton.stone, 3);
		Add_Item_To_Empty_Slot(_temp, 5);
		_temp = new Food_ItemProp(ItemDataStruct.singleton.raw_fish, 6);
		Add_Item(_temp, 16);
		_temp = new ItemProp(ItemDataStruct.singleton.branch, 22);
		Add_Item(_temp, 4, 10);
	}

	/// <summary> 마지막 인벤토리 접근 시간과 현재 시간을 비교하여 경과된 시간을
	/// 각 아이템들에 적용 </summary>
	public void Apply_Time_Lapse() {
		float _gap_lastAccessedTime = GameManager.singleton.TotalPlayTime - _last_accessed_time;
		_last_accessed_time = GameManager.singleton.TotalPlayTime;
		for (int i = 1; i <= num_slots; i++) {
			if (3000 <= slots[i].Data.ID  && slots[i].Data.ID < 4000) {
				// ###### 아이템(음식)에 시간 경과를 적용하는 코드 삽입 ######
				slots[i].Apply_Spoilage_Chance(_gap_lastAccessedTime);
			}
		}
	}
	
	
	public void Copy_ItemProp(ItemProp itemProp, int target_index, int amount = -1) {
		if (amount == -1) {
			amount = itemProp.Amount;
		}
		switch (itemProp.Get_Item_Type()) {
			case ItemTypeEnum.Weapon:
				slots[target_index] = itemProp.Get_Copy<Weapon_ItemProp>();
				slots[target_index].Amount = amount;
				Debug.Log("Copied Weapon");
				break;
			case ItemTypeEnum.Tool:
				slots[target_index] = itemProp.Get_Copy<Tool_ItemProp>();
				slots[target_index].Amount = amount;
				Debug.Log("Copied Tool");
				break;
			case ItemTypeEnum.Material:
				slots[target_index] = itemProp.Get_Copy<ItemProp>();
				slots[target_index].Amount = amount;
				Debug.Log("Copied Material");
				break;
			case ItemTypeEnum.Food:
				slots[target_index] = itemProp.Get_Copy<Food_ItemProp>();
				slots[target_index].Amount = amount;
				Debug.Log("Copied Food");
				break;
			case ItemTypeEnum.Med:
				slots[target_index] = itemProp.Get_Copy<Med_ItemProp>();
				slots[target_index].Amount = amount;
				Debug.Log("Copied Med");
				break;
		}
		return;
	}

	public void Sort_Slots_By_ID() {
		QuickSort_Method(1, num_slots);
	}

	void QuickSort_Method(int lo, int hi) {
		if (lo >= hi) {
			return;
		}
		ItemProp _temp;
		int l = lo + 1, r = hi;
		int pivot = slots[lo].Data.ID;
		while (l <= r) {
			while (l <= hi && slots[l].Data.ID <= pivot) {
				l++;
			}
			while (r > lo && slots[r].Data.ID >= pivot) {
				r--;
			}
			if (l <= r) {
				_temp = slots[l];
				slots[l] = slots[r];
				slots[r] = _temp;
			}
		}
		_temp = slots[lo];
		slots[lo] = slots[r];
		slots[r] = _temp;
		QuickSort_Method(lo, r - 1);
		QuickSort_Method(r + 1, hi);
	}

	public void Swap_Slot(int idx_1, int idx_2) {
		ItemProp _itemProp_1 = slots[idx_1];
		ItemProp _itemProp_2 = slots[idx_2];
		slots[idx_1] = slots[idx_2] = null;
		slots[idx_1] = _itemProp_2;
		slots[idx_2] = _itemProp_1;
	}

	/// <summary> 기존 ItemProp의 수량을 amount만큼 차감,
	/// ItemProp을 복사하고 수량을 amount로 설정하여 그 객체를 반환 </summary>
	public ItemProp Divide_Item_By_Amount(ref ItemProp itemProp, int amount) {
		ItemProp _new_itemProp = itemProp;
		if (itemProp.Amount > amount) {
			itemProp.Amount -= amount;
			_new_itemProp.Amount = amount;
			return _new_itemProp;
		} else {
			return null;
		}
	}

	/// <summary> amount만큼 다 들어갈 때까지 빈 슬롯에
	/// 아이템을 할당시킨 후 0 반환.<br/>슬롯이 부족할 경우에는 할당하지 못하고 남은 amount를 반환.
	/// </summary>
	public int Add_Item_To_Empty_Slot(ItemProp itemProp, int amount) {
		int _emptyIndex = Find_Index_Of_Empty_Slot(1, num_slots);
		if (_emptyIndex != -1) { // 빈 슬롯이 존재할 경우
			if (amount > itemProp.Data.Max_amount) { // 슬롯 하나를 채우고도 amount가 남을 경우
				amount -= itemProp.Data.Max_amount;
				Copy_ItemProp(itemProp, _emptyIndex, itemProp.Data.Max_amount);
				//slots[_emptyIndex] = new ItemProp(itemProp.Data, itemProp.Data.Max_amount);
				return Add_Item_To_Empty_Slot(itemProp, amount);
			} else { // 슬롯 하나만 더 채우면 끝인 경우
				Copy_ItemProp(itemProp, _emptyIndex, amount);
				//slots[_emptyIndex] = new ItemProp(itemProp.Data, amount);
				return 0;
			}
		} else { // 남아있는 빈 슬롯이 없을 경우
			Debug.Log("There's no space in inventory");
			return amount;
		}
	}



	/// <summary> 인벤토리의 지정한 index에 아이템 추가(이 경우 수량 초과되어도 빈 슬롯 탐색 안함)하고, 
	/// 할당하지 못하고 남은 amount가 있다면 이를 반환.<br/>
	/// amount와 index 매개변수는 비종속적.<br/>
	/// amount 지정하지 않으면 원본 amount만큼 그대로 추가.<br/>
	/// index 지정하지 않으면 아이템 자동 할당 : 수량이 max가 아닌 중복 아이템 
	/// 탐색, 있으면 거기 추가하고 없으면 빈 슬롯에 할당.<br/> 
	/// 인벤토리에 빈 슬롯이 부족해지면 할당하지 못하고 남은 amount를 반환. </summary>
	public int Add_Item(ItemProp itemProp, int push_amount = -1, int index = -1) {
		int _final_leftover_amount = 0;
		if (push_amount == -1) {
			push_amount = itemProp.Amount;
		}

		if (index == -1) { // index 지정하지 않으면, 인벤토리 내에서 같은 종류의 아이템 탐색
			int _iter = 1;
			int _sameItemIndex;
			// 같은 종류의 아이템 존재하면, 해당 슬롯에 여유공간 있는지 조사
			while (push_amount > 0) {
				_sameItemIndex = Find_Index_Of_Same_Item(itemProp.Data, _iter, num_slots);
				if (_sameItemIndex == -1 || _iter > num_slots) {
					break;
				}
				int _slotRemainSpace = slots[_sameItemIndex].Data.Max_amount - slots[_sameItemIndex].Amount;
				if (_slotRemainSpace >= push_amount) { // 여유공간이 옮길 아이템 수량보다 많으면,
					slots[_sameItemIndex].Amount += push_amount; // 전부 그대로 옮김
					return 0;
					// Transmission completed, End Function 
				}
				// 여유공간이 부족하면, 일단 그 슬롯은 다 채우고 새로운 같은 종류 슬롯을 탐색
				slots[_sameItemIndex].Amount = itemProp.Data.Max_amount;
				push_amount -= _slotRemainSpace;
				_iter = _sameItemIndex + 1;
			}
			if (push_amount > 0) { // 같은 종류 슬롯이 남지 않으면, 새로운 빈 슬롯을 탐색
				push_amount = Add_Item_To_Empty_Slot(itemProp, push_amount);
				if (push_amount == 0) {
					return 0;
					// Transmission completed, End Function
				}
			}
			return push_amount;
		} 
		
		else { // index를 지정한 경우 (이 경우, 수량이 초과되어도 빈슬롯 탐색하지 않음)
			if (slots[index].Data.ID == 0) { // 슬롯이 비어있으면, 그대로 할당(최대 max_amount)
				if (push_amount > slots[index].Data.Max_amount) {
					_final_leftover_amount = push_amount - slots[index].Data.Max_amount;
				}
				Copy_ItemProp(itemProp, index, push_amount);
			} else { // 슬롯이 비어있지 않을 경우
				if (Check_Slot_ID(index, itemProp.Data.ID)) {  // 아이템 종류가 같은지 확인
					int _slotRemainSpace = slots[index].Data.Max_amount - slots[index].Amount;
					if (_slotRemainSpace >= push_amount) { // 슬롯의 여유공간이 충분할 경우
						slots[index].Amount += push_amount;
					} else { // 슬롯의 여유공간이 부족한 경우
						slots[index].Amount = itemProp.Data.Max_amount;
						_final_leftover_amount = push_amount - _slotRemainSpace;
					}
				} else { // 아이템 종류가 다를 경우
						 // ###### Item Transmission failure ######
					return push_amount;
				}
			}
		}
		return _final_leftover_amount;
	}

	/// <summary> 지정한 index에 대응하는 슬롯의 아이템 제거하고 empty 할당, 제거 전
	/// 남아 있던 수량을 int형으로 반환 </summary>
	public int Remove_Item_And_Return_Origin_Amount (int index) {
		int _origin_amount = slots[index].Amount;
		Init_Slot(ItemDataStruct.singleton.empty, index, 0);
		return _origin_amount;
	}

	/// <summary> 지정한 itemProp 객체를 지정한 index에 해당하는 슬롯에 할당
	/// </summary>
	public void Init_Slot(ItemProp itemProp, int index, int amount) {
		itemProp.Amount = amount;
		slots[index] = itemProp;
	}


	/// <summary> 인벤토리의 두 index를 비교하여 아이템 종류가 같으면 true 반환.
	/// </summary>
	public bool Compare_Slots_ID(int index_1, int index_2) {
		if (slots[index_1].Data.ID == slots[index_2].Data.ID) {
			return true;
		}
		return false;
	}

	/// <summary> 지정한 index에 대응하는 슬롯의 ID와 비교할 ID를 대조, 값이 같으면 true 반환.
	/// </summary>
	public bool Check_Slot_ID(int index, int ID) {
		if (slots[index].Data.ID == ID) {
			return true;
		}
		return false;
	}

	/// <summary> 슬롯의 Amount와 ID를 체크하여, 빈 슬롯이면 true 반환.<br/>
	/// 슬롯의 아이템 데이터는 존재하나 amount가 0이었다면, 해당 슬롯의 정보를 empty로 갱신
	/// </summary>
	public bool Check_If_Slot_Empty(int index) {
		if (slots[index].Data.ID == 0) {
			return true;
		}
		if (slots[index].Amount == 0) {
			Init_Slot(ItemDataStruct.singleton.empty, index, 0);
			return true;
		}
		return false;
	}

	/// <summary> Returns index of first found empty slot from index lo to hi (includes both) </summary>
	/// <returns> index of first found empty slot, or -1 if there's no empty slot </returns>
	public int Find_Index_Of_Empty_Slot(int lo, int hi) {
		if (lo < 1) {
			lo = 1;
		}
		if (hi > num_slots) {
			hi = num_slots;
		}
		for (int i = lo; i < hi + 1; i++) {
			if (Check_If_Slot_Empty(i)) {
				return i;
			}
		}
		return -1;
	}

	/// <summary> 같은 종류의 아이템이 할당된 슬롯을 찾아 그 index를 반환, 없으면 -1 반환
	/// </summary>
	public int Find_Index_Of_Same_Item(ItemData itemData, int lo, int hi) {
		for (int i = lo; i <= hi ; i++) {
			if (slots[i].Data.ID == itemData.ID) {
				return i;
			}
		}
		return -1;
	}
}
