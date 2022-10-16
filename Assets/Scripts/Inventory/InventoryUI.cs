using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemTypeEnumData;

public class InventoryUI : MonoBehaviour {
	public Inventory inventory;
	public ItemDivisionPopupUI itemDivisionPopupUI;
	public PlayerProp playerProp;
	public PlayerMovement playerMovement;
	public InteractMGR interactMGR;
	public AttackMGR attackMGR;
	public DroppedItemAccess droppedItemAccess;
	public DroppedItemAccessUI droppedItemAccessUI;

	public Canvas canvas_inventoryUI;
	public Image image_selected_item;
	public Image image_highlighter;
	public Image image_picked_up_item;
	public Text text_item_name;
	public Text text_item_amount;
	public Text text_item_tooltip;
	public GameObject prefab_itemSlot;
	public Transform parent_itemSlots;
	public GameObject player_obj;

	public bool can_access_inventory = true;
	public bool is_inventory_opened = false;

	public ItemProp[] slots;
	public GameObject[] slots_obj;
	int _num_slots;


	public int _current_selected_index { get; private set; }

	int _prev_num_slots;
	bool _is__check_getKeyUp_coroutine__running = false;

	bool _if_picked_up_divided = false;
	int _picked_up_index = -1; // 아이템을 집지 않은 상태 : -1, 집은 상태 : index

	Coroutine _modify_current_selected_slot_coroutine;
	Coroutine _item_division_coroutine;
	Coroutine _item_use_coroutine;
	Coroutine _item_discardure_coroutine;
	bool _is__modify_current_selected_slot_coroutine__running = false;
	bool _is__item_division_coroutine__running = false;
	bool _is__item_use_coroutine__running = false;
	bool _is__item_discardure_coroutine__running = false;

	bool _is_itemDivisionPopupUI_running = false;

	public const int NUM_COLUMN = 6;
	const float SLOT_DEFAULT_XPOS = -100f; // 슬롯의 기본 Xpos (SLOT_XPOS_GAP * (Index % NUM_COLUMN))
	const float SLOT_DEFAULT_YPOS = 80f; // 슬롯의 기본 Ypos (SLOT_YPOS_GAP * (Index / NUM_COLUMN))
	const float SLOT_XPOS_GAP = 40f; // 슬롯들 사이의 Xpos 간격 (index 증가 방향)
	const float SLOT_YPOS_GAP = -40f; // 슬롯들 사이의 Ypos 간격 (index 증가 방향)
	const float TIMEGAP_MOVESLOT_WHILE_PRESSING = 0.15f; // 방향키를 꾹 누를 때 슬롯 이동 속도
	/*const float FOREDELAY_MOVESLOT_WHILE_PRESSING = 0.50f; // 방향키 꾹 누를 시 선딜레이*/
	const float DELAY_LONG_PRESS = 0.5f;

	void Start() {
		slots_obj = new GameObject[1000];
		_prev_num_slots = inventory.num_slots;
		Create_Inventory_Slots(inventory.num_slots);
		canvas_inventoryUI.enabled = false;
	}
	void Update() {
		if (can_access_inventory) {
			if (Input.GetKeyDown(UserKeyInfo.inventoryAccess)) {
				if (is_inventory_opened) {
					Close_Inventory();
				} else {
					Open_Inventory();
				}
			}
			if (Input.GetKeyDown(UserKeyInfo.escape)) {
				if (is_inventory_opened) {
					Close_Inventory();
				}
			}
		}
	}

	/// <summary> 기본적으로 ItemDivisionCoroutine과 동일한 방식으로 동작. 아이템을 분할하여 집은 상태,<br/>
	/// 즉 slots[0] 메모리 공간에 저장된 상태로 만든 후 DroppedItems 객체를 생성,<br/>
	/// slots[0]에 저장되어 있던 데이터를 해당 객체로 이동시킴.
	/// </summary>
	IEnumerator Item_Discardure_Coroutine() {
		while (is_inventory_opened) {
			if (_picked_up_index != -1) {
				yield return null;
				continue;
			}
			int _pressingState = MyInputMethods.singleton.Check_Long_KeyPress(UserKeyInfo.droppedItemsAccess, DELAY_LONG_PRESS);
			/*if (MyInputMethods.singleton.GetKey_Exception(UserKeyInfo.interact, UserKeyInfo.droppedItemsAccess, UserKeyInfo.moveLeft_1, UserKeyInfo.moveRight_1, UserKeyInfo.moveDown_1, UserKeyInfo.moveUp_1)) {
				_pressingState = 0;
			}*/

			if (_pressingState == 0) {
				// Do nothing.
			} else if (_pressingState == 1 && !inventory.Check_If_Slot_Empty(_current_selected_index)) {
				/*StopCoroutine(_modify_current_selected_slot_coroutine);
				_is__modify_current_selected_slot_coroutine__running = false;
				// 꾹 누르는중에 슬롯이동 제한 */
				// Do nothing.
			} else if (_pressingState == 2) {
				if (!inventory.Check_If_Slot_Empty(_current_selected_index)) {
					Discard_Item(_current_selected_index);
				}
				/*if (!inventoryMGR.Check_If_Slot_Empty(_current_selected_index)) {
					Debug.Log("코루틴 시작");
					if (!_is__modify_current_selected_slot_coroutine__running) {
						_modify_current_selected_slot_coroutine = StartCoroutine(Modify_Current_Selected_Slot_Coroutine());
					}
				}		// 꾹 누르는중에 슬롯이동 제한 */
			} else if (_pressingState == 3 && _picked_up_index == -1 && !inventory.Check_If_Slot_Empty(_current_selected_index)) {
				//StartCoroutine(Enable_ItemDivisionPopupUI_Coroutine());

			}
			yield return null;
		}
		_is__item_division_coroutine__running = false;
	}


	/// <summary> 아이템을 집어든 상태일 경우, 인벤토리를 집어들기 전 상태로 되돌림
	/// </summary>
	void Cancel_Item_Pick_Up() {
		if (_picked_up_index == -1) {
			return;
		}
		inventory.Add_Item(inventory.slots[0], -1, _picked_up_index);
		inventory.Remove_Item_And_Return_Origin_Amount(0);
		Update_Slot_Info(_picked_up_index);
		Update_Highlighted_Item_Info();
		_picked_up_index = -1;
		Update_Picked_Up_Item_Image();
	}

	
	/// <summary> ItemDivisionPopupUI를 실행시키고, 실행되는 중에 InventoryUI를 비활성화함.
	/// ItemDivisionPopupUI의 실행의 결과를 인벤토리 데이터에 적용시키고
	/// 나뉘어진 만큼의 아이템을 집어들고 있는 상태로 만듦. (집어든 아이템은 index 0의 메모리 공간에 할당하였음)
	/// </summary>
	IEnumerator Enable_ItemDivisionPopupUI_Coroutine() {
		itemDivisionPopupUI.Enable_ItemDivisionPopupUI(inventory.slots[_current_selected_index].Amount);
		Disable_InventoryUI_Coroutines();
		can_access_inventory = false;
		while (itemDivisionPopupUI.is_itemDivisionPopupUI_enabled) {
			yield return null;
		}
		
		if (itemDivisionPopupUI.Get_Divided_Item_Amount() == 0) { // 집은 개수가 0일 경우
			//_picked_up_index = -1;
		} else if (itemDivisionPopupUI.Get_Remain_Item_Amount() == 0) { // 모든 수량을 집어든 경우
			Pick_Up_All_Item(_current_selected_index);
		} else { // 1 ~ max_amount - 1 만큼 집어든 경우
			_picked_up_index = _current_selected_index;
			inventory.slots[_picked_up_index].Amount = itemDivisionPopupUI.Get_Remain_Item_Amount();
			Debug.Log(itemDivisionPopupUI.Get_Remain_Item_Amount());
			inventory.Copy_ItemProp(inventory.slots[_picked_up_index], 0, itemDivisionPopupUI.Get_Divided_Item_Amount());
			Update_Slot_Info(_picked_up_index);
			Update_Highlighted_Item_Info();
			_if_picked_up_divided = true;
		}
		can_access_inventory = true;
		Update_Slot_Info(_current_selected_index);
		yield return new WaitForSeconds(0.1f); // 팝업을 종료시킨 입력이 UI에도 적용되는 현상 방지
		_modify_current_selected_slot_coroutine = StartCoroutine(Modify_Current_Selected_Slot_Coroutine());
		_item_use_coroutine = StartCoroutine(Item_Use_Coroutine());
		_item_discardure_coroutine = StartCoroutine(Item_Discardure_Coroutine());
		if (_picked_up_index == -1) {
			_item_use_coroutine = StartCoroutine(Item_Use_Coroutine());
			_item_discardure_coroutine = StartCoroutine(Item_Discardure_Coroutine());
		}
		while (Input.GetKey(UserKeyInfo.attack)) { // 키를 떼 줘야 다시 아이템 분할, 이동 가능
			yield return null;
		}
		_item_division_coroutine = StartCoroutine(Item_Division_Coroutine());

		//Enable_InventoryUI();
	}
	
	IEnumerator Item_Division_Coroutine() {
		_is__item_division_coroutine__running = true;
		while (is_inventory_opened) {
			int _pressingState = MyInputMethods.singleton.Check_Long_KeyPress(UserKeyInfo.attack, DELAY_LONG_PRESS);
			/*if (MyInputMethods.singleton.GetKey_Exception(UserKeyInfo.attack)) {
				_pressingState = 0;
			}*/

			if (_pressingState == 0) { 
				// Do nothing.
			} else if (_pressingState == 1 && !inventory.Check_If_Slot_Empty(_current_selected_index)) {
				/*StopCoroutine(_modify_current_selected_slot_coroutine);
				_is__modify_current_selected_slot_coroutine__running = false;
				// 꾹 누르는중에 슬롯이동 제한 */
			} else if (_pressingState == 2) {
				if (_picked_up_index == -1) { // 아이템을 집은 상태가 아닐 경우
					if (!inventory.Check_If_Slot_Empty(_current_selected_index)) {
						// 아이템을 집지 않았고, 현재 선택된 슬롯이 비어있지 않은 경우
						Pick_Up_All_Item(_current_selected_index);
					}
				} else { // 아이템을 집어든 상태일 경우
					Put_Down_Item(_current_selected_index);
				}
				/*if (!inventoryMGR.Check_If_Slot_Empty(_current_selected_index)) {
					Debug.Log("코루틴 시작");
					if (!_is__modify_current_selected_slot_coroutine__running) {
						_modify_current_selected_slot_coroutine = StartCoroutine(Modify_Current_Selected_Slot_Coroutine());
					}
				}		// 꾹 누르는중에 슬롯이동 제한 */
			} else if (_pressingState == 3 && _picked_up_index == -1 && !inventory.Check_If_Slot_Empty(_current_selected_index)) {
				StartCoroutine(Enable_ItemDivisionPopupUI_Coroutine());
			}
			yield return null;
		}
		_is__item_division_coroutine__running = false;
	}


	IEnumerator Item_Use_Coroutine() {
		_is__item_use_coroutine__running = true;
		while (is_inventory_opened) {
			if (_picked_up_index != -1) {
				yield return null;
				continue;
			}
			if (Input.GetKeyDown(UserKeyInfo.interact)) {
				if (!inventory.Check_If_Slot_Empty(_current_selected_index)) {
					ItemProp _itemProp = inventory.slots[_current_selected_index];
					switch(_itemProp.Get_Item_Type()) {
						case ItemTypeEnum.Weapon:
							// Weapon_ItemProp _weapon_itemProp = _itemProp as Weapon_ItemProp;
							break;
						case ItemTypeEnum.Tool:
							Tool_ItemProp _tool_itemProp = _itemProp as Tool_ItemProp;
							ItemUse.singleton.Use_Weapon_Item(_tool_itemProp);
							break;
						case ItemTypeEnum.Material:
							// Do nothing.
							break;
						case ItemTypeEnum.Food:
							Food_ItemProp _food_itemProp = _itemProp as Food_ItemProp;
							ItemUse.singleton.Use_Food_Item(_food_itemProp);
							Update_Slot_Info(_current_selected_index);
							Update_Highlighted_Item_Info();
							break;

						case ItemTypeEnum.Med:
							Med_ItemProp _med_itemProp = _itemProp as Med_ItemProp;
							ItemUse.singleton.Use_Med_Item(_med_itemProp);
							break;
						
					}
				}
			}

			yield return null;
		}
		_is__item_use_coroutine__running = false;
	}

	public void Discard_Item(int index, int amount = -1) {
		DroppedItems _droppedItems_inst = droppedItemAccess.Find_Nearest_DroppedItems()?.GetComponent<DroppedItems>();
		if (_droppedItems_inst == null) {
			_droppedItems_inst = droppedItemAccess.Create_DroppedItems(player_obj.transform.position).GetComponent<DroppedItems>();
		}
		
		if (amount == -1) {
			amount = inventory.slots[index].Amount;
		}
		//Debug.Log("[1] num_item : " + _droppedItems_inst.num_items + ", num_slots : " + _droppedItems_inst.num_slots);
		_droppedItems_inst.Adjust_Num_Slots();
		//Debug.Log("[2] num_item : " + _droppedItems_inst.num_items + ", num_slots : " + _droppedItems_inst.num_slots);
		_droppedItems_inst.Add_Item(inventory.slots[index], amount);
		//Debug.Log("[3] num_item : " + _droppedItems_inst.num_items + ", num_slots : " + _droppedItems_inst.num_slots);
		//Debug.Log("[4] slots[1].ID : " + _droppedItems_inst.slots[1].Data.ID);
		//Debug.Log("[5] slots[2].ID : " + _droppedItems_inst.slots[2].Data.ID);
		//Debug.Log("[6] slots[3].ID : " + _droppedItems_inst.slots[3].Data.ID);
		inventory.slots[index].Subtract_Amount_If_Possible(amount);
		inventory.Check_If_Slot_Empty(index);
		Update_Slot_Info(index);
	}
	
	
	void Create_Inventory_Slots(int num_slots) {
		_num_slots = num_slots;
		for (int _idx = 1; _idx <= num_slots; _idx++) {
			float _xpos = parent_itemSlots.position.x + SLOT_DEFAULT_XPOS + SLOT_XPOS_GAP * ((_idx - 1) % NUM_COLUMN);
			float _ypos = parent_itemSlots.position.y + SLOT_DEFAULT_YPOS + SLOT_YPOS_GAP * ((_idx - 1) / NUM_COLUMN);
			Vector3 _slotPos = new Vector3(_xpos, _ypos, 0);
			slots_obj[_idx] = Instantiate(prefab_itemSlot, _slotPos, Quaternion.identity, parent_itemSlots);
			//Update_Slot_Info(_idx);
		}
		//Update_Highlighted_Item_Info();
		//Update_Picked_Up_Item_Image();
	}
	
	void Delete_Inventory_Slots() {
		for (int _idx = 1; _idx <= _num_slots; _idx++) {
			Destroy(slots_obj[_idx]);
		}
	}
	
	// 인게임에서 인벤토리 슬롯 개수 변동 시 사용, 모든 슬롯을 삭제하고 재생성
	void Reset_Inventory_Slots(int num_slots) {
			Delete_Inventory_Slots();
			Create_Inventory_Slots(num_slots);
	}
	
	public void Open_Inventory() {
		canvas_inventoryUI.enabled = true;
		is_inventory_opened = true;
		droppedItemAccessUI.can_access_droppedItemAccessUI = false;
		playerMovement.Restrict_Movement();
		interactMGR.Restrict_Interact();
		attackMGR.Restrict_Attack();
		_current_selected_index = 1;
		Modify_Highlighter_Position(_current_selected_index);
		inventory.Apply_Time_Lapse();
		Update_Highlighted_Item_Info();
		Update_Picked_Up_Item_Image();
		for (int i = 1; i <= inventory.num_slots; i++) {
			Update_Slot_Info(i);
		}
		Enable_InventoryUI_Coroutines();
	}
	public void Close_Inventory() {
		canvas_inventoryUI.enabled = false;
		is_inventory_opened = false;
		droppedItemAccessUI.can_access_droppedItemAccessUI = true;
		playerMovement.Approve_Movement();
		interactMGR.Approve_Interact();
		attackMGR.Approve_Attack();
		Cancel_Item_Pick_Up();
		Disable_InventoryUI_Coroutines();
	}

	/// <summary> 인벤토리의 모든 코루틴을 활성화함.
	/// </summary>
	public void Enable_InventoryUI_Coroutines() {
		_is__modify_current_selected_slot_coroutine__running = true;
		_is__item_division_coroutine__running = true;
		_is__item_discardure_coroutine__running = true;
		_is__item_use_coroutine__running = true;
		_modify_current_selected_slot_coroutine = StartCoroutine(Modify_Current_Selected_Slot_Coroutine());
		_item_division_coroutine = StartCoroutine(Item_Division_Coroutine());
		_item_discardure_coroutine = StartCoroutine(Item_Discardure_Coroutine());
		_item_use_coroutine = StartCoroutine(Item_Use_Coroutine());
	}

	/// <summary> 인벤토리의 모든 코루틴을 비활성화함 (Open, Close 기능은 제외).
	/// </summary>
	public void Disable_InventoryUI_Coroutines() {
		_is__modify_current_selected_slot_coroutine__running = false;
		_is__item_division_coroutine__running = false;
		_is__item_discardure_coroutine__running = false;
		_is__item_use_coroutine__running = false;
		StopCoroutine(_modify_current_selected_slot_coroutine);
		StopCoroutine(_item_division_coroutine);
		StopCoroutine(_item_discardure_coroutine);
		StopCoroutine(_item_use_coroutine);
	}

	/// <summary> 지정한 index의 아이템을 모두 집어듦.
	/// </summary>
	void Pick_Up_All_Item(int index) {
		if (_picked_up_index == -1) {
			inventory.slots[0] = inventory.slots[index];
			inventory.Remove_Item_And_Return_Origin_Amount(index);
			_picked_up_index = index;
			Update_Slot_Info(index);
			Update_Picked_Up_Item_Image();
			// 아이템을 집어들었을 때 UI의 동작을 지시하는 코드
		}
	}

	/// <summary> 아이템을 집어든 상태이면, 지정한 index에 집어든 아이템을 내려놓음.<br/>
	/// 다른 종류의 아이템이면 둘을 Swap, 같은 종류의 아이템이면 둘을 합침.<br/> 지정한 슬롯이 꽉 차면,
	/// 남은 수량만큼 기존 슬롯에 되돌려 놓음.
	/// </summary>
	void Put_Down_Item(int target_index) {
		ItemProp _target_itemProp = inventory.slots[target_index];
		if (_picked_up_index == -1) { // 아이템을 집어들지 않은 상태일 경우
			return;
		}

		if (_if_picked_up_divided) { // 아이템을 분할한 것을 들고 있을 경우
			if (inventory.Compare_Slots_ID(target_index, _picked_up_index)) { // 같은 종류의 아이템일 경우
				if (inventory.slots[target_index].Get_Remain_Space_Size() >= inventory.slots[0].Amount) {
					inventory.Add_Item(inventory.slots[0], inventory.slots[0].Amount, target_index);
					_picked_up_index = -1;
					inventory.Remove_Item_And_Return_Origin_Amount(0);
					_if_picked_up_divided = false;
					// Item is successfully put down.
				}
			} else if (inventory.Check_If_Slot_Empty(_current_selected_index)) {
				// 빈 슬롯에 할당할 경우, 그냥 내려놓으면 됨
				inventory.Add_Item(inventory.slots[0], inventory.slots[0].Amount, _current_selected_index);
				_picked_up_index = -1;
				inventory.Remove_Item_And_Return_Origin_Amount(0);
				_if_picked_up_divided = false;
				// Item is successfully put down.
			}
			// 아이템 분할 후 들고 있을 때, 내려놓으려는 슬롯의 공간이 부족하면, 입력 무시(자동 예외 처리됨) 
			Update_Slot_Info(target_index);
			Update_Highlighted_Item_Info();
			Update_Picked_Up_Item_Image();
			return;
		}

		if (target_index == _picked_up_index) { // 집은 것과 동일한 슬롯에 내려놓았을 경우
			inventory.Swap_Slot(0, target_index);
			_picked_up_index = -1;
			inventory.Remove_Item_And_Return_Origin_Amount(0);
			Update_Slot_Info(target_index);
			Update_Highlighted_Item_Info();
			Update_Picked_Up_Item_Image();
			return;
		}


		// 두 아이템이 같은 종류일 경우
		if (inventory.Compare_Slots_ID(target_index, 0)) {
			int _remainAmount = inventory.Add_Item(_target_itemProp, inventory.slots[0].Amount, target_index);
			if (_remainAmount == inventory.slots[0].Amount) { // 목표 슬롯이 가득 차 있을 경우
				inventory.Swap_Slot(0, target_index);
				inventory.Swap_Slot(_picked_up_index, 0);
			} else if (_remainAmount == 0) { // 목표 슬롯에 다 넣을 수 있을 경우
				inventory.Remove_Item_And_Return_Origin_Amount(_picked_up_index);
			} else {
				inventory.slots[0].Amount = _remainAmount;
				inventory.Swap_Slot(0, _picked_up_index);
			}
		}
		// 두 아이템이 다른 종류일 경우
		else {
			inventory.Swap_Slot(0, target_index);
			inventory.Swap_Slot(_picked_up_index, 0);
			inventory.Remove_Item_And_Return_Origin_Amount(0);
		}
		Update_Slot_Info(_picked_up_index);
		Update_Slot_Info(target_index);
		Update_Highlighted_Item_Info();
		Update_Picked_Up_Item_Image();
		_picked_up_index = -1;
		
	}

	void Update_Highlighted_Item_Info() {
		// 우측 툴팁 아이템 이미지 갱신
		image_selected_item.sprite = inventory.slots[_current_selected_index].Data.IconSprite;
		// 우측 툴팁 텍스트 갱신 (이름, 수량, 툴팁)
		StringBuilder _sb = new StringBuilder(256);
		if (!inventory.Check_If_Slot_Empty(_current_selected_index)) {
			text_item_amount.text = MyStringMethods.singleton.Assemble_Strings("Amount : ", inventory.slots[_current_selected_index].Amount.ToString());
			text_item_name.text = inventory.slots[_current_selected_index].Data.Name;
			text_item_tooltip.text = inventory.slots[_current_selected_index].Data.Tooltip;
		} else {
			text_item_amount.text = null;
			text_item_name.text = null;
			text_item_tooltip.text = null;
		}
	}

	/// <summary> index에 해당하는 슬롯의 이미지와 수량을 갱신
	/// </summary>
	void Update_Slot_Info(int index) {
		inventory.Check_If_Slot_Empty(index);
		if (index > 0) {
			slots_obj[index].transform.GetChild(0).GetComponent<Image>().sprite = inventory.slots[index].Data.IconSprite;
			if (inventory.slots[index].Amount != 0) {
				slots_obj[index].transform.GetChild(1).GetComponent<Text>().text = inventory.slots[index].Amount.ToString();
			} else {
				slots_obj[index].transform.GetChild(1).GetComponent<Text>().text = null;
			}
		}
	}

	/// <summary> _picked_up_index 가 -1이면 아이템 이동 슬롯 팝업 이미지 제거, -1이 아니면
	/// 아이템 이동 슬롯 팝업 이미지 생성
	/// </summary>
	void Update_Picked_Up_Item_Image() {
		if (_picked_up_index == -1) {
			image_picked_up_item.sprite = null;
			image_picked_up_item.enabled = false;
		} else {
			image_picked_up_item.enabled = true;
			image_picked_up_item.sprite = inventory.slots[0].Data.IconSprite;
			image_picked_up_item.transform.localPosition = new Vector3 (slots_obj[_current_selected_index].transform.localPosition.x, slots_obj[_current_selected_index].transform.localPosition.y + 0.5f, 0);
		}
	}

	void Modify_Highlighter_Position(int index) {
		image_highlighter.transform.localPosition = slots_obj[index].transform.localPosition;
	}


	/// <summary> 인벤토리 활성화 시 코루틴 실행, 방향키의 이동에 따라 사용자가 선택한 슬롯을
	/// 반영시킴 </summary>
	IEnumerator Modify_Current_Selected_Slot_Coroutine() {
		_is__modify_current_selected_slot_coroutine__running = true;
		int _num_slots = inventory.num_slots;
		while (is_inventory_opened) {
			if (MyInputMethods.singleton.Periodical_Check_If_Key_Pressed(UserKeyInfo.moveRight_1, TIMEGAP_MOVESLOT_WHILE_PRESSING)) {
				if (_current_selected_index < _num_slots) {
					_current_selected_index += 1;
				} else {
					_current_selected_index = 1;
				}
				Modify_Highlighter_Position(_current_selected_index);
				Update_Highlighted_Item_Info();
				Update_Picked_Up_Item_Image();
			}
			
			
			if (MyInputMethods.singleton.Periodical_Check_If_Key_Pressed(UserKeyInfo.moveLeft_1, TIMEGAP_MOVESLOT_WHILE_PRESSING)) {
				if (_current_selected_index > 1) {
					_current_selected_index -= 1;
				} else {
					_current_selected_index = _num_slots;
				}
				Modify_Highlighter_Position(_current_selected_index);
				Update_Highlighted_Item_Info();
				Update_Picked_Up_Item_Image();
			}
			

			if (MyInputMethods.singleton.Periodical_Check_If_Key_Pressed(UserKeyInfo.moveDown_1, TIMEGAP_MOVESLOT_WHILE_PRESSING)) {
				if (_current_selected_index <= _num_slots - NUM_COLUMN) {
					_current_selected_index += NUM_COLUMN;
				} else {
					_current_selected_index %= NUM_COLUMN;
					if (_current_selected_index == 0) { // index가 24일때 6으로 나누면 0이 돼버리는 문제 해결
						_current_selected_index = NUM_COLUMN;
					}
				}
				Modify_Highlighter_Position(_current_selected_index);
				Update_Highlighted_Item_Info();
				Update_Picked_Up_Item_Image();
			}
		
			if (MyInputMethods.singleton.Periodical_Check_If_Key_Pressed(UserKeyInfo.moveUp_1, TIMEGAP_MOVESLOT_WHILE_PRESSING)) {
				if (_current_selected_index > NUM_COLUMN) {
					_current_selected_index -= NUM_COLUMN;
				} else {
					if (_current_selected_index <= _num_slots % NUM_COLUMN) {
						_current_selected_index += _num_slots / NUM_COLUMN * NUM_COLUMN;
					} else {
						_current_selected_index += (_num_slots / NUM_COLUMN - 1) * NUM_COLUMN;
					}
				}
				Modify_Highlighter_Position(_current_selected_index);
				Update_Highlighted_Item_Info();
				Update_Picked_Up_Item_Image();
			}
			
			yield return null;
		}
		_is__modify_current_selected_slot_coroutine__running = false;
	}
}


