using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProp : MonoBehaviour
{
	public ObjectData Data;
	public float time_required_to_destroy;
	//public string drop_items[];

	public GameObject interact_progress_instance;
	GameObject player_obj;
	bool interact_progress_exists;

	/*void Set_Object_Status(string name, float require_time_to_destroy) {
		object_name = name;
	}*/


	void Awake()
    {
		time_required_to_destroy = Data.Max_time_required_to_destroy;
		player_obj = GameObject.Find("Player");
    }

   /* void Update()
    {
		if (!interact_progress_exists && (player_obj.transform.position - this.transform.position).sqrMagnitude < InteractMGR.INTERACT_PROGRESS_OBSERVATION_MIN_DISTANCE_POW) {
			//StartCoroutine(Create_Interact_Progress());
			interact_progress_exists = true;
			interact_progress_instance = Instantiate(prefab_interact_progress);
			interact_progress_instance.GetComponent<InteractProgressDisplayer>().objectMGR = this;
		} else if ((player_obj.transform.position - this.transform.position).sqrMagnitude > InteractMGR.INTERACT_PROGRESS_OBSERVATION_MIN_DISTANCE_POW) {
			Destroy(interact_progress_instance);
			interact_progress_exists = false;
		}
    }*/

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.CompareTag("Player")) {
			interact_progress_instance = Instantiate(Data.Prefab_interact_progress);
			interact_progress_instance.GetComponent<InteractProgressDisplayer>().objectMGR = this;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.CompareTag("Player")) {
			Destroy(interact_progress_instance);
		}
	}

	/* IEnumerator Create_Interact_Progress() {
		interact_progress_exists = true;
		interact_progress_instance = Instantiate(prefab_interact_progress);
		yield return null;
		interact_progress_instance.GetComponent<InteractProgressDisplayer>().objectMGR = this;
	}
	*/

}
