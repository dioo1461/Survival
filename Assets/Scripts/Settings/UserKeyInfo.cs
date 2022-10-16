using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class UserKeyInfo : MonoBehaviour
{
	/*public UserKeyInfo singleton;*/

	public static KeyCode moveUp_1;
	public static KeyCode moveDown_1;
	public static KeyCode moveLeft_1;
	public static KeyCode moveRight_1;
	public static KeyCode moveUp_2;
	public static KeyCode moveDown_2;
	public static KeyCode moveLeft_2;
	public static KeyCode moveRight_2;

	public static KeyCode run;
	public static KeyCode backStep;
	
	public static KeyCode attack;
	public static KeyCode interact;
	public static KeyCode inventoryAccess;
	public static KeyCode droppedItemsAccess;
	public static KeyCode escape;

/*	int _stack_index = 0;
	bool[] _stack_check_coroutine_running;*/

	void Start()
    {
		/*singleton = this;

		_stack_check_coroutine_running = new bool[10];
		for (int i=0; i<10; i++) {
			_stack_check_coroutine_running[i] = false;
		}*/

		moveUp_1 = KeyCode.UpArrow;
		moveDown_1 = KeyCode.DownArrow;
		moveLeft_1 = KeyCode.LeftArrow;
		moveRight_1 = KeyCode.RightArrow;
		run = KeyCode.LeftShift;
		backStep = KeyCode.LeftControl;

		attack = KeyCode.C;
		interact = KeyCode.X;
		inventoryAccess = KeyCode.Tab;
		droppedItemsAccess = KeyCode.Q;
		escape = KeyCode.Escape;
	}

	/*public bool Is_Press_Holded(KeyCode keyCode) {
		Coroutine _coroutine;
		if (Input.GetKey(keyCode)) {
			if (!_is__Check_GetKeyUp__coroutine_running) {
				_coroutine = StartCoroutine(Check_GetKeyUp(keyCode));
				if (current_selected_index < _num_slots) {
					current_selected_index += 1;
				} else {
					current_selected_index = 1;
				}
				Modify_Highlighter_Position(current_selected_index);
			}
		}

		return true;
	}

	IEnumerator Check_GetKeyUp(KeyCode key) {
		_is__Check_GetKeyUp__coroutine_running = true;
		float timer = 0.5f;
		while (timer > 0) {
			timer -= Time.deltaTime;
			if (Input.GetKeyUp(key)) {
				_is__Check_GetKeyUp__coroutine_running = false;
				yield break;
			} else {
				yield return null;
			}
		}
		_is__Check_GetKeyUp__coroutine_running = false;
	}*/
}

