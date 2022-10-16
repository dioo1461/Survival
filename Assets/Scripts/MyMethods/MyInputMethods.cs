using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInputMethods : MonoBehaviour
{
	public static MyInputMethods singleton;

	bool[] _is__check_getKeyUp_coroutine__running = new bool[600]; // KeyCode : 0 ~ 509
	/*	bool _is_check_getKeyUp_foreDelayed_applied = false;
		bool _is__apply_foreDelay_coroutine__running = false;*/

	bool[] _is__keyPress_apply_foreDelay_coroutine__running = new bool[600]; // KeyCode : 0 ~ 509
	bool[] _is_keyPress_foreDelay_passed = new bool[600];
	

	void Awake()
    {
		singleton = this;
		for (int i = 0; i < 600; i++) {
			_is_keyPress_foreDelay_passed[i] = false;
		}
    }

	int[] _longKeyPressReturnValue = new int[600];
	/// <summary> Update함수 또는 코루틴에서 사용.<br/>
	/// 키 입력 이후, 하단 state에 따라 해당하는 정수를 반환.<br/>
	/// state 0 : 키 입력이 없는 상태<br/>
	/// state 1 : 키를 누르는 중이지만 지정한 시간이 지나지 않은 상태<br/>
	/// state 2 : 키를 누르다가 지정한 시간이 지나기 전에 뗀 상태(GetKeyUp)<br/>
	/// state 3 : 키를 지정한 시간 동안 눌러 조건을 만족시킨 상태
	/// </summary>
	public int Check_Long_KeyPress(KeyCode key, float time_delay) {
		if (Input.GetKey(key)) {
			if (!_is__keyPress_apply_foreDelay_coroutine__running[(int)key] && _longKeyPressReturnValue[(int)key] != 3) {
				_longKeyPressReturnValue[(int)key] = 1;
				StartCoroutine(KeyPress_Apply_ForeDelay_Coroutine(key, time_delay));
			}
		}
		int _temp = _longKeyPressReturnValue[(int)key];
		_longKeyPressReturnValue[(int)key] = 0;
		return _temp;
	}

	
	Coroutine[] _keyPress_apply_foreDelay_coroutine = new Coroutine[600];
	/// <summary> Update함수 또는 코루틴에서 사용.<br/>
	/// Check_Long_KeyPress와 동작은 동일하나, 다른 키를 입력받으면 체킹을 중지.<br/>
	/// 키 입력 이후, 하단 state에 따라 해당하는 정수를 반환.<br/>
	/// state 0 : 키 입력이 없는 상태<br/>
	/// state 1 : 키를 누르는 중이지만 지정한 시간이 지나지 않은 상태<br/>
	/// state 2 : 키를 누르다가 지정한 시간이 지나기 전에 뗀 상태(GetKeyUp)<br/>
	/// state 3 : 키를 지정한 시간 동안 눌러 조건을 만족시킨 상태
	/// </summary>
	public int Check_Long_KeyPress_GetException(KeyCode key, float time_delay) {
		if (Input.GetKey(key)) {
			if (!_is__keyPress_apply_foreDelay_coroutine__running[(int)key] && _longKeyPressReturnValue[(int)key] != 3) {
				_longKeyPressReturnValue[(int)key] = 1;
				_keyPress_apply_foreDelay_coroutine[(int)key] = StartCoroutine(KeyPress_Apply_ForeDelay_Coroutine(key, time_delay));
			}
		}
		if (_is__keyPress_apply_foreDelay_coroutine__running[(int)key] && GetKey_Exception(key)) {
			StopCoroutine(_keyPress_apply_foreDelay_coroutine[(int)key]);
			_is__keyPress_apply_foreDelay_coroutine__running[(int)key] = false;
		}
		int _temp = _longKeyPressReturnValue[(int)key];
		_longKeyPressReturnValue[(int)key] = 0;
		return _temp;
	}


	/// <summary> Update함수 또는 코루틴에서 사용.<br/>해당 함수 호출 시점에 key가 눌려 있는지 검사하여
	/// timeGap의 주기마다 true를 반환.<br/>
	/// </summary>
	public bool Periodical_Check_If_Key_Pressed(KeyCode key, float timeGap, float time_foreDelay = 0f) {
		if (Input.GetKey(key)) {
			if (!_is__check_getKeyUp_coroutine__running[(int)key]) {
				StartCoroutine(Check_GetKeyUp_Coroutine(key, timeGap));
				return true;
			}
		}
		return false;
	}

	/// <summary> 지정한 매개변수 이외의 입력이 들어오면 true 반환
	/// </summary>
	public bool GetKey_Exception(KeyCode key_1, KeyCode key_2 = 0, KeyCode key_3 = 0, KeyCode key_4 = 0, KeyCode key_5 = 0, KeyCode key_6 = 0, KeyCode key_7 = 0, KeyCode key_8 = 0, KeyCode key_9 = 0, KeyCode key_10 = 0) {
		int _count = 1;
		if (key_2 != 0) {
			_count++;
			if (key_3 != 0) {
				_count++;
				if (key_4 != 0) {
					_count++;
					if (key_5 != 0) {
						_count++;
						if (key_6 != 0) {
							_count++;
							if (key_7 != 0) {
								_count++;
								if (key_8 != 0) {
									_count++;
									if (key_9 != 0) {
										_count++;
										if (key_10 != 0) {
											_count++;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		switch (_count) {
			case 1:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKey(_keyCode)) {
						if (_keyCode != key_1) {

							return true;
						}
					}
				};
				break;
			case 2:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKey(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2) {
							return true;
						}
					}
				}
				break;
			case 3:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKey(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3) {
							return true;
						}
					}
				}
				break;
			case 4:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKey(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4) {
							return true;
						}
					}
				}
				break;
			case 5:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKey(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5) {
							return true;
						}
					}
				}
				break;
			case 6:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKey(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6) {
							return true;
						}
					}
				}
				break;
			case 7:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKey(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7) {
							return true;
						}
					}
				}
				break;
			case 8:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKey(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7 && _keyCode != key_8) {
							return true;
						}
					}
				}
				break;
			case 9:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKey(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7 && _keyCode != key_8 && _keyCode != key_9) {
							return true;
						}
					}
				}
				break;
			case 10:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKey(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7 && _keyCode != key_8 && _keyCode != key_9 && _keyCode != key_10) {
							return true;
						}
					}
				}
				break;
		}
		return false;
	}
	/// <summary> 지정한 매개변수 이외의 입력이 들어오면 true 반환
	/// </summary>
	public bool GetKeyDown_Exception(KeyCode key_1, KeyCode key_2 = 0, KeyCode key_3 = 0, KeyCode key_4 = 0, KeyCode key_5 = 0, KeyCode key_6 = 0, KeyCode key_7 = 0, KeyCode key_8 = 0, KeyCode key_9 = 0, KeyCode key_10 = 0) {
		int _count = 1;
		if (key_2 != 0) {
			_count++;
			if (key_3 != 0) {
				_count++;
				if (key_4 != 0) {
					_count++;
					if (key_5 != 0) {
						_count++;
						if (key_6 != 0) {
							_count++;
							if (key_7 != 0) {
								_count++;
								if (key_8 != 0) {
									_count++;
									if (key_9 != 0) {
										_count++;
										if (key_10 != 0) {
											_count++;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		switch (_count) {
			case 1:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown(_keyCode)) {
						if (_keyCode != key_1) {

							return true;
						}
					}
				};
				break;
			case 2:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2) {
							return true;
						}
					}
				}
				break;
			case 3:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3) {
							return true;
						}
					}
				}
				break;
			case 4:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4) {
							return true;
						}
					}
				}
				break;
			case 5:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5) {
							return true;
						}
					}
				}
				break;
			case 6:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6) {
							return true;
						}
					}
				}
				break;
			case 7:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7) {
							return true;
						}
					}
				}
				break;
			case 8:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7 && _keyCode != key_8) {
							return true;
						}
					}
				}
				break;
			case 9:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7 && _keyCode != key_8 && _keyCode != key_9) {
							return true;
						}
					}
				}
				break;
			case 10:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7 && _keyCode != key_8 && _keyCode != key_9 && _keyCode != key_10) {
							return true;
						}
					}
				}
				break;
		}
		return false;
	}
	/// <summary> 전달한 매개변수 이외의 입력이 들어오면 true 반환
	/// </summary>
	public bool GetKeyUp_Exception(KeyCode key_1, KeyCode key_2 = 0, KeyCode key_3 = 0, KeyCode key_4 = 0, KeyCode key_5 = 0, KeyCode key_6 = 0, KeyCode key_7 = 0, KeyCode key_8 = 0, KeyCode key_9 = 0, KeyCode key_10 = 0) {
		int _count = 1;
		if (key_2 != 0) {
			_count++;
			if (key_3 != 0) {
				_count++;
				if (key_4 != 0) {
					_count++;
					if (key_5 != 0) {
						_count++;
						if (key_6 != 0) {
							_count++;
							if (key_7 != 0) {
								_count++;
								if (key_8 != 0) {
									_count++;
									if (key_9 != 0) {
										_count++;
										if (key_10 != 0) {
											_count++;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		switch (_count) {
			case 1:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyUp(_keyCode)) {
						if (_keyCode != key_1) {

							return true;
						}
					}
				};
				break;
			case 2:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyUp(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2) {
							return true;
						}
					}
				}
				break;
			case 3:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyUp(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3) {
							return true;
						}
					}
				}
				break;
			case 4:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyUp(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4) {
							return true;
						}
					}
				}
				break;
			case 5:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyUp(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5) {
							return true;
						}
					}
				}
				break;
			case 6:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyUp(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6) {
							return true;
						}
					}
				}
				break;
			case 7:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyUp(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7) {
							return true;
						}
					}
				}
				break;
			case 8:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyUp(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7 && _keyCode != key_8) {
							return true;
						}
					}
				}
				break;
			case 9:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyUp(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7 && _keyCode != key_8 && _keyCode != key_9) {
							return true;
						}
					}
				}
				break;
			case 10:
				foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyUp(_keyCode)) {
						if (_keyCode != key_1 && _keyCode != key_2 && _keyCode != key_3 && _keyCode != key_4 && _keyCode != key_5 && _keyCode != key_6 && _keyCode != key_7 && _keyCode != key_8 && _keyCode != key_9 && _keyCode != key_10) {
							return true;
						}
					}
				}
				break;
		}
		return false;
	}
	IEnumerator Check_GetKeyUp_Coroutine(KeyCode key, float timeGap) {
		_is__check_getKeyUp_coroutine__running[(int)key] = true;
		float timer = timeGap;
		while (timer > 0) {
			timer -= Time.deltaTime;
			if (Input.GetKeyUp(key)) {
				_is__check_getKeyUp_coroutine__running[(int)key] = false;
				yield break;
			} else {
				yield return null;
			}
		}
		_is__check_getKeyUp_coroutine__running[(int)key] = false;
	}
	IEnumerator KeyPress_Apply_ForeDelay_Coroutine(KeyCode key, float time_foreDelay) {
		_is__keyPress_apply_foreDelay_coroutine__running[(int)key] = true;
		float timer = time_foreDelay;
		while (timer > 0) {
			timer -= Time.deltaTime;
			if (Input.GetKeyUp(key)) {
				_is__keyPress_apply_foreDelay_coroutine__running[(int)key] = false;
				_longKeyPressReturnValue[(int)key] = 2;
				yield break;
			}
			_longKeyPressReturnValue[(int)key] = 1;
			yield return null;
		}
		_longKeyPressReturnValue[(int)key] = 3;
		_is__keyPress_apply_foreDelay_coroutine__running[(int)key] = false;
	}
}

