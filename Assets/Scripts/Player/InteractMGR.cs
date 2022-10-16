using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMGR : MonoBehaviour
{
	public GameObject player_obj;
	public PlayerMovement playerMovement;
	public PlayerProp playerProp;
	public AttackMGR attackMGR;
	public float ray_maxDistance;

	public Transform parent_droppedItemAccessPoint;
	public GameObject prefab_droppedItemAccessPoint;

	public GameObject current_interacting_instance;

	ObjectProp current_objectMGR;
	RaycastHit2D raycastHit;
	Ray ray;
	int layerMask_object;

	bool _approve_interact = true;
	bool _is_interacting = false;

	public const float INTERACT_PROGRESS_OBSERVATION_MIN_DISTANCE_POW = 25f;

	void Start()
    {
		layerMask_object = 1 << LayerMask.NameToLayer("Object");
    }

	void Update() {
		if (_approve_interact) {
			if (Input.GetKey(UserKeyInfo.interact)) {
				if (raycastHit = Physics2D.Raycast(player_obj.transform.position, playerMovement.playerAngleToVector3_normalized, ray_maxDistance, layerMask_object)) {
					if (raycastHit.collider.gameObject.GetComponent<ObjectProp>() != null) {
						if (!_is_interacting) {
							_is_interacting = true;
							playerMovement.Restrict_Movement();
							attackMGR.Restrict_Attack();
							current_interacting_instance = raycastHit.collider.gameObject;
							current_objectMGR = raycastHit.collider.gameObject.GetComponent<ObjectProp>();
							GameManager.singleton.Set_TimeSpeed_Acceleration(current_objectMGR.Data.TimeSpeed_acceleration);
						}
						if (current_objectMGR.time_required_to_destroy < 0) {
							Destroy(raycastHit.collider.gameObject);
							Destroy(current_objectMGR.interact_progress_instance);
							GameManager.singleton.Set_TimeSpeed_Default();
						}
						current_objectMGR.time_required_to_destroy -= playerProp.behaveSpeed * GameManager.singleton.timeSpeed_magnification * Time.deltaTime;
					}
				}
			} else {
				if (Input.GetKeyUp(UserKeyInfo.interact) && _is_interacting) {
					_is_interacting = false;
					playerMovement.Approve_Movement();
					attackMGR.Approve_Attack();
					current_interacting_instance = null;
					GameManager.singleton.Set_TimeSpeed_Default();
				}
				
			}
		}
	}


	// approve_interact 접근 시 스크립트 간 간섭을 피하기 위해 변수를 lock으로 관리
	int _lock = 0;
	void LateUpdate() {
		_approve_interact = (_lock == 0) ? true : false;
	}

	public void Approve_Interact() {
		if (_lock != 0) {
			_lock--;
		}
	}
	public void Restrict_Interact() {
		_lock++;
	}

	public void Make_Access_Point(Vector3 position) {
		Instantiate(prefab_droppedItemAccessPoint, position, Quaternion.identity, parent_droppedItemAccessPoint.transform);
	}


}
