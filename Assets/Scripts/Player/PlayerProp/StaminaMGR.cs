using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaMGR : MonoBehaviour
{
	public PlayerProp playerProp;
	public Image staminaBar_inner;
	public Image staminaBar_max;
	public Text text_stamina;

	public float rate_stamina_usage = 1f;
	
	public float groggy_min_stamina;
	public bool is_in_groggy_state;

	Color red = new Color(255f / 255f, 0f / 255f, 0f / 255f);
	Color yellow = new Color(255f / 255f, 202f / 255f, 0f / 255f);

    void Start()
    {
		groggy_min_stamina = PlayerMovement.GROGGY_MIN_STAMINA;
    }
	void Update()
	{
		staminaBar_inner.fillAmount = playerProp.current_stamina / 100f;
		text_stamina.text = "Stamina : " + playerProp.current_stamina.ToString("F0");
		
		// 그로기상태 되면 스태미나 바 빨간색으로 변경
		if (playerProp.current_stamina < groggy_min_stamina) {
			is_in_groggy_state = true;
			staminaBar_inner.color = red;
		} else {
			is_in_groggy_state = false;
			staminaBar_inner.color = yellow;
		}
	}

	/// <summary>
	/// Applies smooth decreasing effect on StaminaBar
	/// </summary>
	/// <returns></returns>
	public IEnumerator StaminaBar_Smooth_diminish() {
		yield return new WaitForSeconds(0.5f);
		while (staminaBar_max.fillAmount - playerProp.max_stamina / 100f > 0.01f) {
			staminaBar_max.fillAmount = Mathf.Lerp(staminaBar_max.fillAmount, playerProp.max_stamina / 100f, 0.5f * Time.deltaTime);
			yield return null;
		}
		staminaBar_max.fillAmount = playerProp.max_stamina / 100f;
	}
}
