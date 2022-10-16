using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedItemAccessUI : MonoBehaviour {
	public DroppedItemAccess droppedItemAccess;
	public InventoryUI inventoryUI;
	public Inventory inventory;
	public ItemDivisionPopupUI itemDivisionPopupUI;
	public PlayerMovement playerMovement;
	public InteractMGR interactMGR;
	public AttackMGR attackMGR;

	public DroppedItems droppedItems; // 객체로부터 전달받을 droppedItems.cs

	public Canvas canvas_DroppedItemAccessUI;
	public Image image_highlighter;
	public GameObject prefab_itemSlot;
	public Transform parent_leftSlots;
	public Transform parent_rightSlots;
	public Transform parent_current_slots;

	public ItemProp[] current_slots;
	public ItemProp[] new_leftSlots;
	public ItemProp[] new_rightSlots;

	int _current_num_slots;
	int _current_num_column;

	public GameObject[] current_slots_obj;
	public GameObject[] leftSlots_obj;
	public GameObject[] rightSlots_obj;

	public int current_selected_index;
	int _left_current_selected_index;
	int _right_current_selected_index;

	int _rightSlot_first_index = 1;
	
	public bool can_access_droppedItemAccessUI = true;
	public bool is_droppedItemAccessUI_opened = false;

	bool _is_leftSlots_enabled = true; // false일 경우, rightSlots가 enabled된 것.

	Coroutine _modify_current_selected_slot_coroutine;
	Coroutine _item_division_coroutine;
	Coroutine _item_use_coroutine;
	Coroutine _switch_slots_coroutine;
	bool _is__modify_current_selected_slot_coroutine__running = false;
	bool _is__item_division_coroutine__running = false;
	bool _is__item_use_coroutine__running = false;
	bool _is__switch_slots_coroutine__runing = false;

	public const int TOTAL_NUM_RIGHTSLOTS = 20;
	public const int RIGHTSLOTS_NUM_COLUMN = 4; // 우측 슬롯의 열 수
	public const int RIGHTSLOTS_NUM_ROW = 5; // 우측 슬롯의 최대 행 수
	const int RIGHTSLOTS_BOUNDARY_NUM = 2; // droppedItems의 아이템이 화면에 표시 가능한 것보다 많을 때, 하이라이터 위치 변경이 제한되는 경계선

	const float LEFTSLOT_DEFAULT_XPOS = -100f; // 좌측 슬롯의 기본 로컬 Xpos (SLOT_XPOS_GAP * (Index % NUM_COLUMN))
	const float LEFTSLOT_DEFAULT_YPOS = 80f; // 좌측 슬롯의 기본 로컬 Ypos (SLOT_YPOS_GAP * (Index / NUM_COLUMN))
	const float RIGHTSLOT_DEFAULT_XPOS = -60f; // 우측 슬롯의 기본 로컬 Xpos (SLOT_XPOS_GAP * (Index % NUM_COLUMN))
	const float RIGHTSLOT_DEFAULT_YPOS = 80f; // 우측 슬롯의 기본 로컬 Ypos (SLOT_YPOS_GAP * (Index / NUM_COLUMN))
	const float SLOT_XPOS_GAP = 40f; // 슬롯들 사이의 Xpos 간격 (index 증가 방향)
	const float SLOT_YPOS_GAP = -40f; // 슬롯들 사이의 Ypos 간격 (index 증가 방향)

	const float TIMEGAP_MOVESLOT_WHILE_PRESSING = 0.15f; // 꾹 누를 때 슬롯 이동 속도
	const float DELAY_LONG_PRESS = 0.5f;

	void Start() {
		/*leftSlots = inventoryMGR.slots;*/
		Create_LeftSlots();
		Create_RightSlots();
		
	}

	private void OnValidate() {
	
	}

	void Update()
    {

	}

	public void Open_DropppedItemAccessUI(DroppedItems instance) {
		droppedItems = instance;
		_rightSlot_first_index = 1;
		_left_current_selected_index = 1;
		_right_current_selected_index = 1;
		current_selected_index = _right_current_selected_index;
		current_slots = droppedItems.slots;
		current_slots_obj = rightSlots_obj;
		parent_current_slots = parent_rightSlots;
		_current_num_slots = droppedItems.num_slots;
		_current_num_column = RIGHTSLOTS_NUM_COLUMN;
		_is_leftSlots_enabled = false;

		droppedItems.Adjust_Num_Slots();
		Init_Num_RightSlots_Obj();
		is_droppedItemAccessUI_opened = true;
		inventoryUI.can_access_inventory = false;
		playerMovement.Restrict_Movement();
		interactMGR.Restrict_Interact();
		attackMGR.Restrict_Attack();
			
		canvas_DroppedItemAccessUI.enabled = true;

		
		
		Modify_Highlighter_Position(current_selected_index);
		for (int i = 1; i <= inventory.num_slots; i++) {
			Update_LeftSlot_Info(i);
		}
		for (int i = 1; i <= droppedItems.num_slots; i++) {
			Update_RightSlot_Info(i);
		}
		Enable_DroppedItemAccessUI_Coroutines();
	}
	public void Close_DroppedItemAccessUI() {
		is_droppedItemAccessUI_opened = false;
		inventoryUI.can_access_inventory = true;
		playerMovement.Approve_Movement();
		interactMGR.Approve_Interact();
		attackMGR.Approve_Attack();
		Destroy_DroppedItems_If_Empty();
		is_droppedItemAccessUI_opened = false;
		canvas_DroppedItemAccessUI.enabled = false;
		Disable_DroppedItemAccessUI_Coroutines();
	}

	/// <summary> DroppedItemAccessUI의 우측 슬롯 개수를 설정
	/// </summary>

	public void Create_LeftSlots() {
		leftSlots_obj = new GameObject[inventory.num_slots + 1];
		for (int _idx = 1; _idx <= inventory.num_slots; _idx++) {
			// float _xpos = parent_leftSlots.position.x + LEFTSLOT_DEFAULT_XPOS + SLOT_XPOS_GAP * ((_idx - 1) % InventoryUI.NUM_COLUMN);
			float _xpos = LEFTSLOT_DEFAULT_XPOS + SLOT_XPOS_GAP * ((_idx - 1) % InventoryUI.NUM_COLUMN);
			float _ypos = LEFTSLOT_DEFAULT_YPOS + SLOT_YPOS_GAP * ((_idx - 1) / InventoryUI.NUM_COLUMN);
			Vector3 _slotPos = new Vector3(_xpos, _ypos, 0);
			leftSlots_obj[_idx] = Instantiate(prefab_itemSlot, _slotPos, Quaternion.identity, parent_leftSlots);
			leftSlots_obj[_idx].transform.localPosition = _slotPos;
		}
	}

	public void Create_RightSlots() {
		rightSlots_obj = new GameObject[TOTAL_NUM_RIGHTSLOTS + 1];
		for (int _idx = 1; _idx <= TOTAL_NUM_RIGHTSLOTS; _idx++) {
			float _xpos = RIGHTSLOT_DEFAULT_XPOS + SLOT_XPOS_GAP * ((_idx - 1) % RIGHTSLOTS_NUM_COLUMN);
			float _ypos = RIGHTSLOT_DEFAULT_YPOS + SLOT_YPOS_GAP * ((_idx - 1) / RIGHTSLOTS_NUM_COLUMN);
			Vector3 _slotPos = new Vector3(_xpos, _ypos, 0);
			rightSlots_obj[_idx] = Instantiate(prefab_itemSlot, _slotPos, Quaternion.identity, parent_rightSlots);
			rightSlots_obj[_idx].transform.localPosition = _slotPos;
		}
	}

	void Delete_LeftSlots() {
		for (int _idx = 1; _idx <= inventory.num_slots; _idx++) {
			Destroy(leftSlots_obj[_idx]);
		}
	}
	void Delete_RightSlots() {
		for (int _idx = 1; _idx <= droppedItems.num_slots; _idx++) {
			Destroy(rightSlots_obj[_idx]);
		}
	}

	void Adjust_Num_LeftSlots() {

	}

	void Expand_RightSlots(int num_column) {

	}

	/// <summary> DroppedItemsUI의 모든 코루틴을 활성화함.
	/// </summary>
	public void Enable_DroppedItemAccessUI_Coroutines() {
		_modify_current_selected_slot_coroutine = StartCoroutine(Modify_Current_Selected_Slot_Coroutine());
		_item_division_coroutine = StartCoroutine(Item_Division_Coroutine());
		_item_use_coroutine = StartCoroutine(Item_Use_Coroutine());
		_switch_slots_coroutine = StartCoroutine(Switch_Slots_Coroutine());
	}

	/// <summary> DroppedItemsUI의 모든 코루틴을 비활성화함.
	/// </summary>
	public void Disable_DroppedItemAccessUI_Coroutines(){
		StopCoroutine(_modify_current_selected_slot_coroutine);
		StopCoroutine(_item_division_coroutine);
		StopCoroutine(_item_use_coroutine);
		StopCoroutine(_switch_slots_coroutine);
		_is__modify_current_selected_slot_coroutine__running = false;
		_is__item_division_coroutine__running = false;
		_is__item_use_coroutine__running = false;
		_is__switch_slots_coroutine__runing = false;
	}

	void Modify_Highlighter_Position(int index) {
		if (!_is_leftSlots_enabled) {
			index -= _rightSlot_first_index - 1;
		}
		image_highlighter.transform.SetParent(parent_current_slots);
		image_highlighter.transform.localPosition = current_slots_obj[index].transform.localPosition;
	}

	/// <summary> 해당 슬롯의 Empty 여부 체크 이후 이미지를 갱신.
	/// </summary>
	void Update_LeftSlot_Info(int index) {
		inventory.Check_If_Slot_Empty(index);
		if (index > 0) {
			leftSlots_obj[index].transform.GetChild(0).GetComponent<Image>().sprite = inventory.slots[index].Data.IconSprite;
			if (inventory.slots[index].Amount != 0) {
				leftSlots_obj[index].transform.GetChild(1).GetComponent<Text>().text = inventory.slots[index].Amount.ToString();
			} else {
				leftSlots_obj[index].transform.GetChild(1).GetComponent<Text>().text = null;
			}
		}
	}
	/// <summary> 해당 슬롯의 Empty 여부 체크 이후 이미지를 갱신.
	/// </summary>
	void Update_RightSlot_Info(int index) {
		if (1 <= index - _rightSlot_first_index + 1 && index - _rightSlot_first_index + 1 <= TOTAL_NUM_RIGHTSLOTS) { 
			if (droppedItems.Check_If_Slot_Empty(index)) {
				rightSlots_obj[index - _rightSlot_first_index + 1].transform.GetChild(0).GetComponent<Image>().sprite = ItemDataStruct.singleton.empty.Data.IconSprite;
				rightSlots_obj[index - _rightSlot_first_index + 1].transform.GetChild(1).GetComponent<Text>().text = null;
				return;
			}
			rightSlots_obj[index - _rightSlot_first_index + 1].transform.GetChild(0).GetComponent<Image>().sprite = droppedItems.slots[index].Data.IconSprite;
			rightSlots_obj[index - _rightSlot_first_index + 1].transform.GetChild(1).GetComponent<Text>().text = droppedItems.slots[index].Amount.ToString();
		}
	}

	/// <summary> UI를 Open할 때 우측 slots_obj 개수를 조정.
	/// </summary>
	void Init_Num_RightSlots_Obj() {
		int _num_slots_obj = (droppedItems.num_slots < TOTAL_NUM_RIGHTSLOTS) ? droppedItems.num_slots : TOTAL_NUM_RIGHTSLOTS;
		for (int i = 1; i <= _num_slots_obj; i++) {
			rightSlots_obj[i].SetActive(true);
			Update_RightSlot_Info(i);
		}
		for (int i = _num_slots_obj + 1; i <= TOTAL_NUM_RIGHTSLOTS; i++) {
			rightSlots_obj[i].SetActive(false);
		}
	}
	
	/// <summary> 한 droppedItems내에서 num_slots의 변동에 따라 화면에 표시되는 우측 slots_obj 개수를 조정.<br/>
	/// droppedItems.num_slots 조정 후 슬롯 개수와 조정 전 슬롯 개수의 차를 반환. (슬롯 증가 : 양수, 슬롯 감소 : 음수)
	/// </summary>
	int Adjust_Num_RightSlots_Obj() {
		int _diff = droppedItems.Adjust_Num_Slots();
		int _num_slots = droppedItems.num_slots;

		if (_diff > 0) {
			if (_num_slots - _diff < TOTAL_NUM_RIGHTSLOTS) { // 원래 슬롯이 최대 개수(20개) 미만이었다면,
				for (int i = _num_slots - _diff; i <= _num_slots; i++) {
					Update_RightSlot_Info(i);
					rightSlots_obj[i].SetActive(true);
				}
			}
		} else if (_diff < 0) {
			for (int i = TOTAL_NUM_RIGHTSLOTS; i > _num_slots; i--) {
				rightSlots_obj[i].SetActive(false);
			}
		}
		return _diff;
	}
	
	
	/// <summary> 현재 상호작용 중인 droppedItems의 slots가 비어있는지 확인.<br/>
	/// 만약 비어있다면, 해당 GameObject를 제거하고 true 반환.
	/// </summary>
	bool Destroy_DroppedItems_If_Empty() {
		if (droppedItems.num_items == 0) {
			Destroy(droppedItems.gameObject);
			return true;
		}
		return false;
	}

	/// <summary> 오른쪽 슬롯에서 왼쪽 슬롯으로 아이템 이동. 슬롯 이미지, num_slots 업데이트 함께 수행
	/// </summary>
	void Transfer_Item_From_Right_To_Left(int origin_index, int amount = -1) {
		ItemProp _itemProp = droppedItems.slots[origin_index];
		ItemProp[] _slots = inventory.slots;
		int _origin_amount;

		if (amount == -1) {
			amount = _itemProp.Amount;
		}
		_origin_amount = amount;

		int _iter = 1;
		int _sameItemIndex;
		// 같은 종류의 아이템 존재하면, 해당 슬롯에 여유공간 있는지 조사
		while (amount > 0) {
			_sameItemIndex = inventory.Find_Index_Of_Same_Item(_itemProp.Data, _iter, inventory.num_slots);
			if (_sameItemIndex == -1 || _iter > inventory.num_slots) {
				break;
			}
			int _slotRemainSpace = _slots[_sameItemIndex].Data.Max_amount - _slots[_sameItemIndex].Amount;
			if (_slotRemainSpace >= amount) { // 여유공간이 옮길 아이템 수량보다 많으면,
				_slots[_sameItemIndex].Amount += amount; // 전부 그대로 옮김
				Update_LeftSlot_Info(_sameItemIndex);
				Debug.Log(_sameItemIndex + " : same - completed, R -> L");
				amount = 0;
				break;
				// Transmission completed.
			}
			// 여유공간이 부족하면, 일단 그 슬롯은 다 채우고 새로운 같은 종류 슬롯을 탐색
			_slots[_sameItemIndex].Amount = _itemProp.Data.Max_amount;
			Update_LeftSlot_Info(_sameItemIndex);
			Debug.Log(_sameItemIndex + " : same - exceeded, R -> L");
			amount -= _slotRemainSpace;
			_iter = _sameItemIndex + 1;
		}

		while (amount > 0) { // 같은 종류 슬롯을 다 썼으면, 새로운 빈 슬롯을 탐색
			int _emptyIndex = inventory.Find_Index_Of_Empty_Slot(1, inventory.num_slots);
			if (_emptyIndex == -1) { // 남아있는 빈 슬롯이 없을 경우
				break;
			}
			// 빈 슬롯이 존재할 경우
			if (amount > _itemProp.Data.Max_amount) { // 슬롯 하나를 채우고도 amount가 남을 경우
				inventory.Copy_ItemProp(_itemProp, _emptyIndex, _itemProp.Data.Max_amount);
				Update_LeftSlot_Info(_emptyIndex);
				Debug.Log(_emptyIndex + " : empty - exceeded, R -> L");
				Debug.Log("ID : " + inventory.slots[_emptyIndex].Data.ID);
				amount -= _itemProp.Data.Max_amount;
			} else { // 슬롯 하나만 더 채우면 끝인 경우
				inventory.Copy_ItemProp(_itemProp, _emptyIndex, amount);
				Update_LeftSlot_Info(_emptyIndex);
				Debug.Log(_emptyIndex + " : empty - completed, R -> L"); // 이부분이 문제
				Debug.Log("ID : " + inventory.slots[_emptyIndex].Data.ID);
				amount = 0;
				break;
				// Transmission completed.
			}
		}
			
		if (amount == 0) {
			droppedItems.slots[origin_index].Amount -= _origin_amount;
			if (droppedItems.Check_If_Slot_Empty(origin_index)) {
				droppedItems.Advance_Slots_In_Ascending_Order(origin_index);
				droppedItems.num_items--;
				for (int i = origin_index; i <= droppedItems.num_items + 1; i++) {
					Update_RightSlot_Info(i);
				}
			}
		} else {
			_itemProp.Subtract_Amount_If_Possible(amount);
		}
		Update_RightSlot_Info(origin_index);

		if (Adjust_Num_RightSlots_Obj() != 0) {
			if (droppedItems.num_slots >= TOTAL_NUM_RIGHTSLOTS && _rightSlot_first_index + TOTAL_NUM_RIGHTSLOTS - 1 - droppedItems.num_slots >= 4) {
				_rightSlot_first_index -= RIGHTSLOTS_NUM_COLUMN;
				Modify_Highlighter_Position(current_selected_index);
				Update_All_RightSlots_Info();
			}
		}
	}

	/// <summary> 왼쪽 슬롯에서 오른쪽 슬롯으로 아이템 이동. 슬롯 이미지, num_slots 업데이트 함께 수행
	/// </summary>
	void Transfer_Item_From_Left_To_Right (int origin_index, int amount = -1) {
		ItemProp _itemProp = inventory.slots[origin_index];
		ItemProp[] _slots = droppedItems.slots;
		int _num_slots = droppedItems.num_slots;
		int _origin_amount;

		if (amount == -1) {
			amount = _itemProp.Amount;
		}
		_origin_amount = amount;

		int _iter = 1;
		int _sameItemIndex;
		// 같은 종류의 아이템 존재하면, 해당 슬롯에 여유공간 있는지 조사
		while (amount > 0) {
			_sameItemIndex = droppedItems.Find_Index_Of_Same_Item(_itemProp.Data, _iter, _num_slots);
			if (_sameItemIndex == -1 || _iter > droppedItems.num_slots) {
				break;
			}
			int _slotRemainSpace = _slots[_sameItemIndex].Data.Max_amount - _slots[_sameItemIndex].Amount;
			if (_slotRemainSpace >= amount) { // 여유공간이 옮길 아이템 수량보다 많으면,
				_slots[_sameItemIndex].Amount += amount; // 전부 그대로 옮김
				Update_RightSlot_Info(_sameItemIndex);
				amount = 0;
				break;
				// Transmission completed.
			}
			// 여유공간이 부족하면, 일단 그 슬롯은 다 채우고 새로운 같은 종류 슬롯을 탐색
			_slots[_sameItemIndex].Amount = _itemProp.Data.Max_amount;
			Update_RightSlot_Info(_sameItemIndex);
			amount -= _slotRemainSpace;
			_iter = _sameItemIndex + 1;
		}

		while (amount > 0) { // 같은 종류 슬롯을 다 썼으면, 새로운 빈 슬롯을 탐색
			int _emptyIndex = droppedItems.Find_Index_Of_Empty_Slot(1, _num_slots);
			if (_emptyIndex == -1) { // 남아있는 빈 슬롯이 없을 경우
				break;
			}
			// 빈 슬롯이 존재할 경우
			droppedItems.num_items++;
			Adjust_Num_RightSlots_Obj();
			if (amount > _itemProp.Data.Max_amount) { // 슬롯 하나를 채우고도 amount가 남을 경우
				droppedItems.Copy_ItemProp(_itemProp, _emptyIndex, _itemProp.Data.Max_amount);
				Update_RightSlot_Info(_emptyIndex);
				amount -= _itemProp.Data.Max_amount;
			} else { // 슬롯 하나만 더 채우면 끝인 경우
				droppedItems.Copy_ItemProp(_itemProp, _emptyIndex, amount);
				Update_RightSlot_Info(_emptyIndex);
				amount = 0;
			}
		}

		if (amount == 0) {
			inventory.slots[origin_index].Amount -= _origin_amount;
		} else {
			_itemProp.Subtract_Amount_If_Possible(amount);
		}
		Update_LeftSlot_Info(origin_index);
		Debug.Log(origin_index + " : L -> R");

		/*if (Adjust_Num_RightSlots_Obj() != 0) {
			if (droppedItems.num_slots >= TOTAL_NUM_RIGHTSLOTS && _rightSlot_first_index + TOTAL_NUM_RIGHTSLOTS - 1 - droppedItems.num_slots >= 4) {
				_rightSlot_first_index += RIGHTSLOTS_NUM_COLUMN;
				Modify_Highlighter_Position(current_selected_index);
				Update_All_RightSlots_Info();
			}
		}*/
	}

	IEnumerator Switch_Slots_Coroutine() {
		if (_is__switch_slots_coroutine__runing) {
			yield break;
		}

		while (is_droppedItemAccessUI_opened) {
			if (Input.GetKeyDown(UserKeyInfo.inventoryAccess)) {
				if (!_is_leftSlots_enabled) { // 왼쪽 슬롯으로 전환
					current_slots = inventory.slots;
					current_slots_obj = leftSlots_obj;
					_current_num_slots = inventory.num_slots;
					_right_current_selected_index = current_selected_index;
					current_selected_index = _left_current_selected_index;
					parent_current_slots = parent_leftSlots;
					_current_num_column = InventoryUI.NUM_COLUMN;
					_is_leftSlots_enabled = true;
				} else { // 오른쪽 슬롯으로 전환
					current_slots = droppedItems.slots;
					current_slots_obj = rightSlots_obj;
					_current_num_slots = droppedItems.num_slots;
					_left_current_selected_index = current_selected_index;
					current_selected_index = _right_current_selected_index;
					parent_current_slots = parent_rightSlots;
					_current_num_column = RIGHTSLOTS_NUM_COLUMN;
					_is_leftSlots_enabled = false;
				}
				Modify_Highlighter_Position(current_selected_index);
			}
			yield return null;
		}
	}


	IEnumerator Enable_ItemDivisionPopupUI_Coroutine() {
		Disable_DroppedItemAccessUI_Coroutines();
		// Debug.Log("current index : " + current_selected_index + ", ID : " + current_slots[current_selected_index].Data.ID + ", amount : " + current_slots[current_selected_index].Amount);
		// Debug.Log("right_ID : " + droppedItems.slots[current_selected_index].Data.ID + ", amount : " + droppedItems.slots[current_selected_index].Amount);
		if (_is_leftSlots_enabled) {
			itemDivisionPopupUI.Enable_ItemDivisionPopupUI(inventory.slots[current_selected_index].Amount);
		} else {
			itemDivisionPopupUI.Enable_ItemDivisionPopupUI(droppedItems.slots[current_selected_index].Amount);
		}
		can_access_droppedItemAccessUI = false;
		while (itemDivisionPopupUI.is_itemDivisionPopupUI_enabled) {
			yield return null;
		}

		if (itemDivisionPopupUI.Get_Divided_Item_Amount() == 0) { // 선택한 개수가 0일 경우
			// do nothing.
		} else {
			if (_is_leftSlots_enabled) {
				Transfer_Item_From_Left_To_Right(current_selected_index, itemDivisionPopupUI.Get_Divided_Item_Amount());
			} else {
				Transfer_Item_From_Right_To_Left(current_selected_index, itemDivisionPopupUI.Get_Divided_Item_Amount());
			}
		}
		can_access_droppedItemAccessUI = true;
		if (_is_leftSlots_enabled) {
			Update_LeftSlot_Info(current_selected_index);
		} else {
			Update_RightSlot_Info(current_selected_index);
		}
		yield return new WaitForSeconds(0.1f); // 팝업을 종료시킨 입력이 UI에도 적용되는 현상 방지
		_modify_current_selected_slot_coroutine = StartCoroutine(Modify_Current_Selected_Slot_Coroutine());
		_item_use_coroutine = StartCoroutine(Item_Use_Coroutine());
		_switch_slots_coroutine = StartCoroutine(Switch_Slots_Coroutine());
		while (Input.GetKey(UserKeyInfo.attack)) { // 키를 떼 줘야 다시 아이템 분할, 이동 가능
			yield return null;
		}
		_item_division_coroutine = StartCoroutine(Item_Division_Coroutine());

		//Enable_InventoryUI();
	}

	IEnumerator Item_Division_Coroutine() {
		if (_is__item_division_coroutine__running) {
			yield break;
		}

		_is__item_division_coroutine__running = true;
		while (is_droppedItemAccessUI_opened) {
			int _pressingState = MyInputMethods.singleton.Check_Long_KeyPress(UserKeyInfo.attack, DELAY_LONG_PRESS);
			/*if (MyInputMethods.singleton.GetKey_Exception(UserKeyInfo.attack, UserKeyInfo.moveLeft_1, UserKeyInfo.moveRight_1, UserKeyInfo.moveDown_1, UserKeyInfo.moveUp_1)) {
				_pressingState = 0;
			}*/

		if (_pressingState == 0) {
				// Do nothing.
			} else if (_pressingState == 1) {
				// Do nothing.
			} else if (_pressingState == 2) {
				if (_is_leftSlots_enabled && inventory.slots[current_selected_index].Data.ID != ItemDataStruct.singleton.empty.Data.ID) {
					Transfer_Item_From_Left_To_Right(current_selected_index);
				} else if (!_is_leftSlots_enabled && droppedItems.slots[current_selected_index].Data.ID != ItemDataStruct.singleton.empty.Data.ID) {
					Transfer_Item_From_Right_To_Left(current_selected_index);
				}

			} else if (_pressingState == 3) {
				if (_is_leftSlots_enabled && inventory.slots[current_selected_index].Data.ID != ItemDataStruct.singleton.empty.Data.ID) {
					StartCoroutine(Enable_ItemDivisionPopupUI_Coroutine());
				} else if (!_is_leftSlots_enabled && droppedItems.slots[current_selected_index].Data.ID != ItemDataStruct.singleton.empty.Data.ID) {
					StartCoroutine(Enable_ItemDivisionPopupUI_Coroutine());
				}
			}
			yield return null;
		}
		_is__item_division_coroutine__running = false;
	}

	IEnumerator Item_Use_Coroutine() {


		yield return null;
	}

	IEnumerator Modify_Current_Selected_Slot_Coroutine() {
		if (_is__modify_current_selected_slot_coroutine__running) {
			yield break;
		}
		_is__modify_current_selected_slot_coroutine__running = true;

		while (is_droppedItemAccessUI_opened) {
			if (_is_leftSlots_enabled) {
				_current_num_slots = inventory.num_slots;
			} else {
				_current_num_slots = droppedItems.num_slots;
			}
			if (MyInputMethods.singleton.Periodical_Check_If_Key_Pressed(UserKeyInfo.moveRight_1, TIMEGAP_MOVESLOT_WHILE_PRESSING)) {
				if (!_is_leftSlots_enabled && current_selected_index % RIGHTSLOTS_NUM_COLUMN == 0) {
					if (0 <= droppedItems.num_slots - current_selected_index && droppedItems.num_slots - current_selected_index <= RIGHTSLOTS_NUM_COLUMN - 1) {
						_rightSlot_first_index = 1;
						Update_All_RightSlots_Info();
					} else if (_rightSlot_first_index + RIGHTSLOTS_NUM_COLUMN * RIGHTSLOTS_NUM_ROW - 1 < droppedItems.num_slots && 12 <= current_selected_index - _rightSlot_first_index && current_selected_index - _rightSlot_first_index <= 19) {
						_rightSlot_first_index += RIGHTSLOTS_NUM_COLUMN;
						Update_All_RightSlots_Info();
					}
				}
				if (current_selected_index < _current_num_slots) {
					current_selected_index += 1;
				} else {
					current_selected_index = 1;
				}
				Modify_Highlighter_Position(current_selected_index);
			}


			if (MyInputMethods.singleton.Periodical_Check_If_Key_Pressed(UserKeyInfo.moveLeft_1, TIMEGAP_MOVESLOT_WHILE_PRESSING)) {
				if (!_is_leftSlots_enabled && current_selected_index % RIGHTSLOTS_NUM_COLUMN == 1) {
					if (droppedItems.num_slots >= 20 && 1 <= current_selected_index && current_selected_index <= RIGHTSLOTS_NUM_COLUMN) {
						_rightSlot_first_index = droppedItems.num_slots - TOTAL_NUM_RIGHTSLOTS + 1;
						Update_All_RightSlots_Info();
					} else if (_rightSlot_first_index > 1 && 0 <= current_selected_index - _rightSlot_first_index && current_selected_index - _rightSlot_first_index <= RIGHTSLOTS_NUM_COLUMN * RIGHTSLOTS_BOUNDARY_NUM - 1) {
						_rightSlot_first_index -= RIGHTSLOTS_NUM_COLUMN;
						Update_All_RightSlots_Info();
					}
				}
				if (current_selected_index > 1) {
					current_selected_index -= 1;
				} else {
					current_selected_index = _current_num_slots;
				}
				Modify_Highlighter_Position(current_selected_index);
			}


			if (MyInputMethods.singleton.Periodical_Check_If_Key_Pressed(UserKeyInfo.moveDown_1, TIMEGAP_MOVESLOT_WHILE_PRESSING)) {
				if (!_is_leftSlots_enabled) {
					if (0 <= droppedItems.num_slots - current_selected_index && droppedItems.num_slots - current_selected_index <= RIGHTSLOTS_NUM_COLUMN - 1) {
						_rightSlot_first_index = 1;
						Update_All_RightSlots_Info();
					}
					else if (_rightSlot_first_index + RIGHTSLOTS_NUM_COLUMN * RIGHTSLOTS_NUM_ROW - 1 < droppedItems.num_slots && (TOTAL_NUM_RIGHTSLOTS - RIGHTSLOTS_NUM_COLUMN * RIGHTSLOTS_BOUNDARY_NUM) <= current_selected_index - _rightSlot_first_index && current_selected_index - _rightSlot_first_index <= TOTAL_NUM_RIGHTSLOTS - 1) {
						_rightSlot_first_index += RIGHTSLOTS_NUM_COLUMN;
						Update_All_RightSlots_Info();
					}
				}
				if (current_selected_index <= _current_num_slots - _current_num_column) {
						current_selected_index += _current_num_column;
				} else {
					current_selected_index %= _current_num_column;
					if (current_selected_index == 0) { // index가 24일때 6으로 나누면 0이 돼버리는 문제 해결
						current_selected_index = _current_num_column;
					}
				}
				Modify_Highlighter_Position(current_selected_index);
			}

			if (MyInputMethods.singleton.Periodical_Check_If_Key_Pressed(UserKeyInfo.moveUp_1, TIMEGAP_MOVESLOT_WHILE_PRESSING)) {
				if (!_is_leftSlots_enabled) {
					if (droppedItems.num_slots >= 20 && 1 <= current_selected_index && current_selected_index <= RIGHTSLOTS_NUM_COLUMN) {
						_rightSlot_first_index = droppedItems.num_slots - TOTAL_NUM_RIGHTSLOTS + 1;
						Update_All_RightSlots_Info();
					}
					else if (_rightSlot_first_index > 1 && 0 <= current_selected_index - _rightSlot_first_index && current_selected_index - _rightSlot_first_index <= RIGHTSLOTS_NUM_COLUMN * RIGHTSLOTS_BOUNDARY_NUM - 1) {
						_rightSlot_first_index -= RIGHTSLOTS_NUM_COLUMN;
						Update_All_RightSlots_Info();
					}
				} 
				if (current_selected_index > _current_num_column) {
					current_selected_index -= _current_num_column;
				} else {
					if (current_selected_index <= _current_num_slots % _current_num_column) {
						current_selected_index += _current_num_slots / _current_num_column * _current_num_column;
					} else {
						current_selected_index += (_current_num_slots / _current_num_column - 1) * _current_num_column;
					}
				}
				Modify_Highlighter_Position(current_selected_index);
			}

			yield return null;
		}
		_is__modify_current_selected_slot_coroutine__running = false;
	}

	void Update_All_RightSlots_Info() {
		for (int i = _rightSlot_first_index; i <= droppedItems.num_slots; i++) {
			Update_RightSlot_Info(i);
		}
	}

}
