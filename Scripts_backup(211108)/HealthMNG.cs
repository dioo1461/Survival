using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMNG : MonoBehaviour
{
	public PlayerProp playerProp;
	public Image healthBar_inner;
	public Image healthBar_damaged;
	public Text text_health;

	float current_player_health;
	bool is_player_invincible = false;

	bool is_smoothDiminishCoroutine_running = false;

	public const float MAX_HP = 100f;
	public const float INVINCIBLE_TIME = 1f;

	/// <summary>
	/// Decreases player health when attacked
	/// </summary>
	public void Player_Damaged(float dmg) {
		if (!is_player_invincible) {
			playerProp.player_health -= dmg;
			StartCoroutine(Make_Player_Invincible());
			Player_Health_Changed();
		}
	}

	/// <summary>
	/// Makes player invincible for a moment
	/// </summary>
	/// <returns></returns>
	IEnumerator Make_Player_Invincible() {
		is_player_invincible = true;
		yield return new WaitForSeconds(INVINCIBLE_TIME);
		is_player_invincible = false;
	}

	/// <summary>
	/// reflect change of player health to HealthBar and max_stamina
	/// </summary>
	public void Player_Health_Changed() {
		current_player_health = playerProp.player_health;

		if (current_player_health <= 0f) {
			playerProp.player_health = 0f;
			text_health.text = "GAME OVER";
		}
		
		healthBar_inner.fillAmount = current_player_health / 100f;
		text_health.text = "Health : " + current_player_health.ToString("F0");
		StartCoroutine(HealthBar_Smooth_diminish());
		
		playerProp.player_max_stamina = 20f + playerProp.player_health * 4f / 5f;
		StartCoroutine(GameObject.Find("StaminaBar").GetComponent<StaminaMNG>().StaminaBar_Smooth_diminish());
	}

	/// <summary>
	/// Applies smooth decreasing effect on HealthBar
	/// </summary>
	/// <returns></returns>
	IEnumerator HealthBar_Smooth_diminish() {
		if (!is_smoothDiminishCoroutine_running) {
			is_smoothDiminishCoroutine_running = true;
			yield return new WaitForSeconds(0.3f);
			while (healthBar_damaged.fillAmount - healthBar_inner.fillAmount > 0.01f) {
				healthBar_damaged.fillAmount = Mathf.Lerp(healthBar_damaged.fillAmount, current_player_health / MAX_HP, 3 * Time.deltaTime);
				yield return null;
			}
			healthBar_damaged.fillAmount = current_player_health / 100f;
			is_smoothDiminishCoroutine_running = false;
		}
	}
}
