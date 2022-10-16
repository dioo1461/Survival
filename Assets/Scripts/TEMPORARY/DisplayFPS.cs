using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFPS : MonoBehaviour
{
    Text text;
    float fps;
	float avg = 0;
	int cnt = 0;
    float max = 0;
    float min = Mathf.Infinity;

    void Start() {
        text = gameObject.GetComponent<Text>();
		StartCoroutine(Update_FPS_Coroutine());
        }

    void Update()
    {
        fps = 1f / Time.deltaTime;
		avg += fps;
		cnt++;
		if (fps > max) {
			max = fps;
		}
		if (fps < min) {
			min = fps;
		}
    }

	IEnumerator Update_FPS_Coroutine() {
		WaitForSeconds _wfs = new WaitForSeconds(1f);
		while (true) {
			avg = 0;
			cnt = 0;
			max = 0;
			min = Mathf.Infinity;
			yield return _wfs;
			avg /= (float)cnt;
			text.text = MyStringMethods.singleton.Assemble_Strings("AVG FPS : ", avg.ToString("F0"), "\nMAX FPS : ", max.ToString("F0"), "\nMIN FPS : ", min.ToString("F0"));
		}
	}

}
