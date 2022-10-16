using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PlayerPropToJson
{
	float player_health;
	float player_hunger;
	float player_thirst;
	float player_temperature;

	float player_moveSpeed;
	float player_behaveSpeed;

	int fireStartingSkill_level;
	int fireStartingSkill_exp;
	int cookingSkill_level;
	int cookingSkill_exp;
	int craftingSkill_level;
	int craftingSkill_exp;
	int huntingSkill_level;
	int huntingSkill_exp;
	int fisiningSkill_level;
	int fishingSkill_exp;

	public PlayerPropToJson(float health, float hunger, float thirst, float temp, float moveSpeed, float behaveSpeed, int fireLev, int fireExp, int cookLev, int cookExp, int craftLev, int craftExp, int huntLev, int huntExp, int fishLev, int fishExp)
	{
		player_health = health;
		player_hunger = hunger;
		player_thirst = thirst;
		player_temperature = temp;
		player_moveSpeed = moveSpeed;
		player_behaveSpeed = behaveSpeed;
		fireStartingSkill_level = fireLev;
		fireStartingSkill_exp = fireExp;
		cookingSkill_level = cookLev;
		cookingSkill_exp = cookExp;
		craftingSkill_level = craftLev;
		craftingSkill_exp = craftExp;
		huntingSkill_level = huntLev;
		huntingSkill_exp = huntExp;
		fisiningSkill_level = fishLev;
		fishingSkill_exp = fishExp;
	}
}
