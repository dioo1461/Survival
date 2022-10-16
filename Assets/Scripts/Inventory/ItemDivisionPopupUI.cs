using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDivisionPopupUI : MonoBehaviour
{
	public Canvas divisionPopupUI_canvas;
	public Text text_division_item_amount;
	public Image image_inner_scrollbar;
	public Image image_upperArrow;
	public Image image_downerArrow;

	public bool is_itemDivisionPopupUI_enabled;

	int _amount;
	int _divided_amount;

	const float YPOS_MIN = -40f;
	const float YPOS_MAX = 40f;
	const float TIMEGAP_MOVESLOT_WHILE_PRESSING = 0.15f; // 방향키를 꾹 누를 때 수량 증감 간격(속도)

	void Start()
    {
		divisionPopupUI_canvas.enabled = false;
		is_itemDivisionPopupUI_enabled = false;
	}

	Coroutine _itemDivisionPopupUI_coroutine;
	IEnumerator ItemDivisionPopupUI_Coroutine() {
		while (is_itemDivisionPopupUI_enabled) {
			if (MyInputMethods.singleton.Periodical_Check_If_Key_Pressed(UserKeyInfo.moveUp_1, TIMEGAP_MOVESLOT_WHILE_PRESSING) && _divided_amount < _amount) {
				_divided_amount++;
				text_division_item_amount.text = _divided_amount.ToString();
				Update_ScrollBar_Position();
			}
			if (MyInputMethods.singleton.Periodical_Check_If_Key_Pressed(UserKeyInfo.moveDown_1, TIMEGAP_MOVESLOT_WHILE_PRESSING) && _divided_amount > 0) {
				_divided_amount--;
				text_division_item_amount.text = _divided_amount.ToString();
				Update_ScrollBar_Position();
			}
			/*if (Input.GetKeyDown(UserKeyInfo.escape) || Input.GetKeyDown(UserKeyInfo.inventoryAccess)) {
				_divided_amount = 0;
				Disable_ItemDivisionPopupUI();
			}*/

			if (Input.GetKeyUp(UserKeyInfo.attack)) {
				Disable_ItemDivisionPopupUI();
			}

			if (MyInputMethods.singleton.GetKeyDown_Exception(UserKeyInfo.attack, UserKeyInfo.moveUp_1,  UserKeyInfo.moveDown_1)) {
				_divided_amount = 0;
				Disable_ItemDivisionPopupUI();
			}
			yield return null;
		}
	}

	/// <summary> ItemDivisionPopUI에서 분할할 아이템의 수량을 전달받고 분할 초깃값을 설정.<br/>
	/// 초깃값을 지정하지 않으면, (최대 수량 / 2)의 올림으로 자동 설정.
	/// </summary>
	public void Enable_ItemDivisionPopupUI(int amount, int init_amount = -1) {
		_amount = amount;
		if (init_amount == -1) {
			_divided_amount = (int)Mathf.Ceil(((float)amount / 2f));
		} else {
			_divided_amount = init_amount;
		}

		text_division_item_amount.text = _divided_amount.ToString();
		Update_ScrollBar_Position();
		divisionPopupUI_canvas.enabled = true;
		is_itemDivisionPopupUI_enabled = true;
		StartCoroutine(ItemDivisionPopupUI_Coroutine());
	}
	public void Disable_ItemDivisionPopupUI() {
		divisionPopupUI_canvas.enabled = false;
		is_itemDivisionPopupUI_enabled = false;
	}

	public int Get_Remain_Item_Amount() {
		return _amount - _divided_amount;
	}

	public int Get_Divided_Item_Amount() {
		return _divided_amount;
	}


	void Update_ScrollBar_Position() {
		float _ratio = (float)_divided_amount / (float)_amount;
		image_inner_scrollbar.transform.localPosition = new Vector3(0f, Mathf.Lerp(YPOS_MIN, YPOS_MAX, _ratio), 0f);
	}
	
	

}
