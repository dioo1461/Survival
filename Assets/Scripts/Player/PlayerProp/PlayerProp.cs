using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class PlayerProp : MonoBehaviour {
	// 기본 스탯
	public float health;
	public float exhaustion;
	public float hunger;
	public float digesting_calories;
	public float calories;
	public float body_weight; // 체중
	public float thirst;
	public float temperature;
	public float mentality;

	public float max_health = 100f; // 최대 건강
	public float max_exhaustion = 120f; // 최대 피로도
	public float max_hunger = 100f; // 최대 허기
	public float max_calories = 5000f; // 최대 칼로리
	public float min_body_weight = 40f; // 최소 체중
	public float max_thirst = 100f; // 최대 갈증
	public float max_temperature = 45f; // 최대 체온
	public float max_mentality = 100f; // 최대 정신력

	public float current_stamina; // 현재 스태미나
	public float max_stamina; // 최대 스태미나

	public float defenseBonus; // 방어도
	public float warmth; // 보온
	public float sensible_temperature; // 체감온도
	public float power; // 힘
	public float moveSpeed; // 이동속도
	public float behaveSpeed; // 행동속도
	public float Immunity; // 면역력

	public float dark_adaptation; // 암적응 수치
	public float ketone_concentration; // 혈중 케톤 농도

	// 스킬 레벨
	public int fireStartingSkill_level;
	public int fireStartingSkill_exp;
	public int cookingSkill_level;
	public int cookingSkill_exp;
	public int craftingSkill_level;
	public int craftingSkill_exp;
	public int huntingSkill_level;
	public int huntingSkill_exp;
	public int fisiningSkill_level;
	public int fishingSkill_exp;


	void Awake() {
		health = 100f;
		exhaustion = 100f;
		hunger = 0f;
		digesting_calories = 0f;
		calories = 1500f;
		body_weight = 65f;
		thirst = 60f;
		temperature = 36.5f;
		mentality = 100f;

		current_stamina = 100f;
		max_stamina = 100f;

		defenseBonus = 0f;
		warmth = 0f;
		sensible_temperature = 36.5f;
		power = 10f;
		moveSpeed = 2f;
		behaveSpeed = 1f;
		Immunity = 100f;

		dark_adaptation = 0f;
		ketone_concentration = 1f;

		fireStartingSkill_level = 1;
		fireStartingSkill_exp = 0;
		cookingSkill_level = 1;
		cookingSkill_exp = 0;
		craftingSkill_level = 1;
		craftingSkill_exp = 0;
		huntingSkill_level = 1;
		huntingSkill_exp = 0;
		fisiningSkill_level = 1;
		fishingSkill_exp = 0;


		GameObject.Find("HealthBar").GetComponent<HealthMGR>().Player_Health_Changed();
	}

	public void Save_PlayerProp(int num_game, string saveDirectory_name) {
		string _directoryPath = Application.dataPath + "/" + saveDirectory_name + "/";
		string _jsonData = JsonUtility.ToJson(gameObject.GetComponent<PlayerProp>());
		Debug.Log(_jsonData);
		//FileStream fileStream = new FileStream(string.Format("{0}/PlayerProp{1}.json", _dataPath, num_game), FileMode.Create);

		if (!Directory.Exists(_directoryPath)) {
			Directory.CreateDirectory(_directoryPath);
		}
		File.WriteAllText(string.Format("{0}PlayerProp{1}.json", _directoryPath, num_game), _jsonData);
	}                                             

	/*public void Load_PlayerProp(int num_game) {
		string _dataPath = Application.dataPath;
		string _jsonData = File.ReadAllText(string.Format("{0}/save/PlayerProp{1}.json", _dataPath, num_game));
		playerProp = JsonUtility.FromJson<PlayerProp>(_jsonData);
	}*/
}


