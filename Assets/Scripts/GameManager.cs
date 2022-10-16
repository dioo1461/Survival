using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum Difficulty {
	casual, usual, real
};

public class GameManager : MonoBehaviour {
	public static GameManager singleton;
	public PlayerProp playerProp;
	public TestJson testJson;

	public Text text_playTime;
	public Difficulty difficulty = Difficulty.usual;

	public float timeSpeed_magnification { get; private set; } // Critical Section
	bool _is_timeSpeed_magnification_controlling = false; // Semaphore

	public float TotalPlayTime { get; private set; }
	public float TimeElapsed { get; set; }
	int _minute = 0, _hour = 0, _day = 1, _month = 0, _year = 0;

	public const float DEFAULT_TIMESPEED_MAGNIFICATION = 60f;



	void Awake() {
		singleton = this;
		singleton.TimeElapsed = 0f;
		singleton.timeSpeed_magnification = DEFAULT_TIMESPEED_MAGNIFICATION;
	}

	void Update() {
		TimeElapsed += timeSpeed_magnification * Time.deltaTime;
		if (TimeElapsed > 60f) {
			TotalPlayTime += TimeElapsed;
			_minute += (int)TimeElapsed / 60;
			TimeElapsed -= 60f * (int)TimeElapsed / 60;
			if (_minute > 59) {
				_hour += _minute / 60;
				_minute -= _minute / 60 * 60;
				if (_hour > 23) {
					_day += _hour / 24;
					_hour -= _hour / 24 * 24;
					if (_day > 30) {
						_month += _day / 30;
						_day -= _day / 30 * 30;
						if (_month > 12) {
							_year += _month / 12;
							_month -= _month / 12 * 12;
						}
					}
				}
			}
		}
		// Debug
		text_playTime.text = _year + "년 " + _month + "개월 " + _day + "일 " + _hour + "시간 " + _minute + "분";
		
	}

	/// <summary> 게임 내 시간 속도를 변경. 어떤 객체가 시간 속도를 제어하고 있으면,
	/// Set_TimeSpeed_Default() 함수로 제어권을 반환하기 전까지 다른 객체가 속도를 제어할 수 없음.
	/// </summary>
	public bool Set_TimeSpeed_Acceleration(float magnification) {
		if (_is_timeSpeed_magnification_controlling) {
			return false;
		}
		_is_timeSpeed_magnification_controlling = true;
		timeSpeed_magnification *= magnification;
		return true;
	}

	public void Set_TimeSpeed_Default() {
		_is_timeSpeed_magnification_controlling = false;
		timeSpeed_magnification = DEFAULT_TIMESPEED_MAGNIFICATION;
	}

	public void SaveGame(int num_game) {
		playerProp.Save_PlayerProp(num_game, "Save");
	}

	public void LoadGame(int num_game) {
		
	}

}
