using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaMNG : MonoBehaviour
{
	public PlayerProp playerProp;
	public Image staminaBar_inner;
	public Image staminaBar_max;
	public Text text_stamina;

	public float rate_stamina_usage = 1f;
	float current_stamina;
	float max_stamina;
	float groggy_stamina;

	Color red = new Color(255f / 255f, 0f / 255f, 0f / 255f);
	Color yellow = new Color(255f / 255f, 202f / 255f, 0f / 255f);

    void Start()
    {
		groggy_stamina = PlayerMovement.GROGGY_MIN_STAMINA;
    }
	void Update()
	{
		current_stamina = playerProp.player_stamina;
		staminaBar_inner.fillAmount = current_stamina / 100f;
		text_stamina.text = "Stamina : " + current_stamina.ToString("F0");
		
		// 그로기상태 되면 스태미나 바 빨간색으로 변경
		if (current_stamina < groggy_stamina) {
			staminaBar_inner.color = red;
		} else {
			staminaBar_inner.color = yellow;
		}
	}

	/// <summary>
	/// Applies smooth decreasing effect on StaminaBar
	/// </summary>
	/// <returns></returns>
	public IEnumerator StaminaBar_Smooth_diminish() {
		max_stamina = playerProp.player_max_stamina;
		yield return new WaitForSeconds(0.5f);
		while (staminaBar_max.fillAmount - max_stamina / 100f > 0.01f) {
			staminaBar_max.fillAmount = Mathf.Lerp(staminaBar_max.fillAmount, max_stamina / 100f, 0.5f * Time.deltaTime);
			yield return null;
		}
		staminaBar_max.fillAmount = max_stamina / 100f;
	}
}
