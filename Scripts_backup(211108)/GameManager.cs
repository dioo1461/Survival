using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public Text text_playTime;

	public static float speed_timePass = 60f;
	public static float timeElapsed = 0f;
	public static float totalPlayTime = 0f;
	public static int minute = 0, hour = 0, day = 1, month = 0, year = 0;

	void Start() {

	}

	void Update() {
		timeElapsed += speed_timePass * Time.deltaTime;
		if (timeElapsed > 60f) {
			totalPlayTime += timeElapsed;
			minute += (int)timeElapsed / 60;
			timeElapsed -= 60f * (int)timeElapsed / 60;
			if (minute > 59) {
				hour += minute / 60;
				minute -= minute / 60 * 60;
				if (hour > 23) {
					day += hour / 24;
					hour -= hour / 24 * 24;
					if (day > 30) {
						month += day / 30;
						day -= day / 30 * 30;
						if (month > 12) {
							year += month / 12;
							month -= month / 12 * 12;
						}
					}
				}
			}
		}
		text_playTime.text = year + "년 " + month + "개월 " + day + "일 " + hour + "시간 " + minute + "분";
		
	
	}

	public void SaveGame() {

	}

	public void LoadGame() {

	}


}
