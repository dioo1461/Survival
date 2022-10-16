using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMGR : MonoBehaviour
{
	public PlayerProp playerProp;
	public StaminaMGR staminaMGR;
	public AttackMGR attackMGR;

	public InventoryUI inventoryUI;
	public Image healthBar_inner;
	public Image healthBar_damaged;
	public Text text_health;

	float current_player_health;
	bool is_player_invincible = false;

	bool is_smoothDiminishCoroutine_running = false;

	public const float MAX_HP = 100f;
	public const float INVINCIBLE_TIME = 0.5f;

	/// <summary> PlayerPropMGR에서 사용되는 함수, 건강의 점진적 회복을 구현
	/// </summary>
	public void Adjust_Health() {

	}


	/// <summary> 플레이어가 공격당하면 체력을 감소시키고 플레이어에게 무적 시간을 적용함.
	/// </summary>
	public void Player_Damaged(float dmg) {
		if (!is_player_invincible) {
			playerProp.health -= dmg;
			attackMGR.Ingauge_Battle();
			StartCoroutine(Make_Player_Invincible());
			Player_Health_Changed();
			if(inventoryUI.is_inventory_opened) {
				inventoryUI.Close_Inventory();
			}

		}
	}

	IEnumerator Make_Player_Invincible() {
		is_player_invincible = true;
		yield return new WaitForSeconds(INVINCIBLE_TIME);
		is_player_invincible = false;
	}

	/// <summary>
	/// Reflect changed values to HealthBar and max_stamina, when player health is changed
	/// </summary>
	public void Player_Health_Changed() {
		current_player_health = playerProp.health;		
		healthBar_inner.fillAmount = current_player_health / 100f;
		text_health.text = "Health : " + current_player_health.ToString("F0");
		if (current_player_health <= 0f) {
			playerProp.health = 0f;
			text_health.text = "GAME OVER";
		}
		StartCoroutine(HealthBar_Smooth_diminish());
		playerProp.max_stamina = 20f + playerProp.health * 4f / 5f;
		StartCoroutine(staminaMGR.StaminaBar_Smooth_diminish());
	}

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
