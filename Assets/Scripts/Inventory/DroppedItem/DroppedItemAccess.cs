using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemAccess : MonoBehaviour
{
	public Inventory inventoryMGR;
	public DroppedItemAccessUI droppedItemAccessUI;
	public PlayerMovement playerMovement;
	public InteractMGR interactMGR;
	public AttackMGR attackMGR;

	public GameObject player_obj;
	public GameObject prefab_droppedItems;
	public GameObject droppedItemAccessPoint_obj;
	public Transform parent_droppedItems;

	public GameObject[] _nearby_droppedItems;
	public GameObject _nearest_droppedItems_obj;
	int _nearest_droppedItems_index = -1;
	int _nearby_droppedItems_count = 0;
	bool _is_position_change_applied = false;

	Coroutine _show_accessPoint_coroutine;
	bool _is__droppedItems_interaction_coroutine__running = false;

	const int SIZE_NEARBY_DROPPEDITEMS_BUFFER = 10;

	void Start() 
	{
		_nearby_droppedItems = new GameObject[SIZE_NEARBY_DROPPEDITEMS_BUFFER];
		droppedItemAccessPoint_obj.SetActive(false);
	}

    void Update()
    {
		if (!_is__droppedItems_interaction_coroutine__running && _nearby_droppedItems_count > 0) {
			_is__droppedItems_interaction_coroutine__running = true;
			droppedItemAccessPoint_obj.SetActive(true);
			_show_accessPoint_coroutine = StartCoroutine(DroppedItems_Interaction_Coroutine());
		} else if (_is__droppedItems_interaction_coroutine__running && _nearby_droppedItems_count == 0) {
			_is__droppedItems_interaction_coroutine__running = false;
			droppedItemAccessPoint_obj.SetActive(false);
			StopCoroutine(_show_accessPoint_coroutine);
		}

    }

	IEnumerator DroppedItems_Interaction_Coroutine() {
		while (true) {
			GameObject _new_nearest_obj = Find_Nearest_DroppedItems();
			if (_nearest_droppedItems_obj != _new_nearest_obj && _new_nearest_obj != null) {
				droppedItemAccessPoint_obj.transform.position = _new_nearest_obj.transform.position;
			}
			_nearest_droppedItems_obj = _new_nearest_obj;

			if (Input.GetKeyDown(UserKeyInfo.droppedItemsAccess) && droppedItemAccessUI.can_access_droppedItemAccessUI) {
				if (!droppedItemAccessUI.is_droppedItemAccessUI_opened) {
					droppedItemAccessUI.Open_DropppedItemAccessUI(_new_nearest_obj.GetComponent<DroppedItems>());
				} else {
					droppedItemAccessUI.Close_DroppedItemAccessUI();
				}
			} else if (Input.GetKeyDown(UserKeyInfo.escape) && droppedItemAccessUI.can_access_droppedItemAccessUI) {
				if (droppedItemAccessUI.is_droppedItemAccessUI_opened) {
					droppedItemAccessUI.Close_DroppedItemAccessUI();
				}
			}
			yield return null;
		}
	}


	/// <summary> DroppedItems 객체를 배열에 참조 연결하고, 저장된 index 정보를 해당 객체에 전달함.
	/// </summary>
	public int Add_Nearby_DroppedItems(GameObject obj) {
		int _idx;
		for (_idx = 0; _idx < SIZE_NEARBY_DROPPEDITEMS_BUFFER; _idx++) {
			if (_nearby_droppedItems[_idx] == null) {
				break;
			}
		}
		_nearby_droppedItems[_idx] = obj;
		_nearby_droppedItems_count++;
		_is_position_change_applied = false; // 배열의 정보에 변동이 생기면, 이를 명시
		return _idx;
	}

	/// <summary> index에 해당하는 DroppedItems 객체를 배열에서 제거하고,<br/>
	/// 해당 객체에 저장되어 있던 index 정보를 -1로 초기화함.
	/// </summary>
	public int Remove_Nearby_DroppedItems(int index) {
		_nearby_droppedItems[index] = null;
		_nearby_droppedItems_count--;
		_is_position_change_applied = false; // 배열의 정보에 변동이 생기면, 이를 명시
		return -1;
	}

	/// <summary> 지정한 위치에 DroppedItems 객체를 생성하고 그 객체를 반환.
	/// </summary>
	public GameObject Create_DroppedItems(Vector3 position) {
		return Instantiate(prefab_droppedItems, position, Quaternion.identity, parent_droppedItems);
	}

	/// <summary> _nearby_droppedItems[] 배열에서 플레이어와 가장 가까운 액세스포인트를 찾아 그 GameObject를 반환.<br/>
	/// 가까운 액세스포인트가 없으면 (배열에 저장된 액세스포인트 객체가 없으면) null 반환.
	/// </summary>
	public GameObject Find_Nearest_DroppedItems() {
		int _nearest_idx = -1;
		float _sqr_nearest_dist = Mathf.Infinity;
		for (int i = 0; i < SIZE_NEARBY_DROPPEDITEMS_BUFFER; i++) {
			if (_nearby_droppedItems[i] != null) {
				float _sqr_new_dist = (_nearby_droppedItems[i].transform.position - player_obj.transform.position).sqrMagnitude;
				if (_sqr_new_dist < _sqr_nearest_dist) {
					_nearest_idx = i;
					_sqr_nearest_dist = _sqr_new_dist;
				}
			}
		}
		if (_nearest_idx != -1) {
			_nearest_droppedItems_index = _nearest_idx;
			return _nearby_droppedItems[_nearest_idx];
		} else {
			return null;
		}
	}

	
}
