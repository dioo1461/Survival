using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCoroutine : MonoBehaviour
{
	bool is_coroutine_running = false;

	public void StartCoroutineSingle(IEnumerator enumerator) {
		if(!is_coroutine_running) {
			is_coroutine_running = true;
			StartCoroutine(enumerator);
		}
	}
    
}
