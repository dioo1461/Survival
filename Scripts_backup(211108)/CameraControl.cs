using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public GameObject player;
	Transform AT;
	void Start() {
		AT = player.transform;
	}
	void LateUpdate() {
		transform.position = new Vector3(AT.position.x, AT.position.y, transform.position.z);
	}
}