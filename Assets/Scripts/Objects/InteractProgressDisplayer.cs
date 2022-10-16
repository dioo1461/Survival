using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractProgressDisplayer : MonoBehaviour {
	public ObjectProp objectMGR;
	AttackMGR attackMGR;
	GameObject object_interaction_canvas;
	Image interact_progress_inner;
	TextMeshProUGUI text_interact_progress;
	int _day_remain, _hour_remain, _minute_remain;
	bool _is_color_red = false;

	Transform object_transform;

	void Start() {
		attackMGR = GameObject.Find("AttackMGR").GetComponent<AttackMGR>();
		object_interaction_canvas = GameObject.Find("Object Interaction Canvas");
		transform.SetParent(object_interaction_canvas.transform);
		interact_progress_inner = transform.Find("Interact_Progress_Inner").GetComponent<Image>();
		text_interact_progress = transform.Find("Text_Interact_Progress").GetComponent<TextMeshProUGUI>();
		transform.position = objectMGR.gameObject.transform.position;
		StartCoroutine(Display_Time_Remaining_Coroutine());
	}


	void Update() {
		//transform.position = Camera.main.WorldToScreenPoint(objectMGR.gameObject.transform.position);
		
		interact_progress_inner.fillAmount = 1f - objectMGR.time_required_to_destroy / objectMGR.Data.Max_time_required_to_destroy;
		if (!_is_color_red && attackMGR.battle_ingauged) {
			_is_color_red = true;
			text_interact_progress.color = new Color(255f / 255f, 50f / 255f, 50f / 255f, 180f / 255f);
			interact_progress_inner.color = new Color(255f / 255f, 100f / 255f, 100f / 255f, 150f / 255f);
		} else if (_is_color_red && !attackMGR.battle_ingauged) {
			_is_color_red = false;
			text_interact_progress.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 180f / 255f);
			interact_progress_inner.color = new Color(86f / 255f, 181f / 255f, 231f / 255f, 150f / 255f);
		}
	}

	IEnumerator Display_Time_Remaining_Coroutine() {
		int time_temp;
		while ((time_temp = (int)objectMGR.time_required_to_destroy) >= 0) {
			_day_remain = time_temp / (24 * 3600);
			time_temp -= _day_remain * (24 * 3600);
			_hour_remain = time_temp / 3600;
			time_temp -= _hour_remain * 3600;
			_minute_remain = time_temp / 60;
			if (_day_remain > 0) {
				text_interact_progress.text = _day_remain + "D\n" + _hour_remain + "H\n" + _minute_remain + "M";
			} else if (_hour_remain > 0) {
				text_interact_progress.text = _hour_remain + "H\n" + _minute_remain + "M";
			} else {
				text_interact_progress.text = _minute_remain + "M";
			}
			yield return null;
		}
	}

}

