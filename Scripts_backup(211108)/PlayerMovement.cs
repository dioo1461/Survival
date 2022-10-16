using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	float speed = 1f;
	bool flag_backstep;
	float runningTimer = 0f;
	bool can_run = true;
	bool can_run_more = false;

	public PlayerProp playerProp;
	public StaminaMNG staminaMNG;
	public Rigidbody2D player_rigid2D;
	public Vector2 playerAngle;
	float h, v;

	/* RUNNINGSPEED : 달리기 속도
	 * WALKINGSPEED : 걷는 속도
	 * GROGGYSPEED : 스태미나 소진시 걷는속도
	 * SPEED_RUNNINGSTART : 달리기로 취급되는 최소 속도
	 * GROGGY_MIN_STAMINA : 스태미나가 이 수치 이하이면 그로기상태가 적용됨
	 * STAMINADECREASE_RUNNING : 달릴 때 초당 스태미나 감소량
	 * STAMINAINCREASE_WALKING : 걸을 때 초당 스태미나 감소량
	 * STAMINAINCREASE_RESTING : 쉴 때 초당 스태미나 감소량
	 */
	public const float RUNNINGSPEED = 2.5f;
	public const float WALKINGSPEED = 1.0f;
	public const float GROGGYSPEED = 0.3f;
	public const float BACKSTEPSPEED = -0.5f;

	public const float SPEED_RUNNINGSTART = 1.2f;
	public const float GROGGY_MIN_STAMINA = 30f;

	public const float STAMINAINCREASE_WALKING = 2f;
	public const float STAMINAINCREASE_RESTING = 4f;
	public const float STAMINADECREASE_RUNNING = 6f;


	void Start() {

	}

	void FixedUpdate() {
		// LCtrl 누르고있으면, 백스텝
		if (Input.GetKey(KeyCode.LeftControl)) {
			flag_backstep = true;
			speed = WALKINGSPEED;
		} else
			flag_backstep = false;

		// LShift를 눌렀을 때, 달릴 수 있는 상태인지 확인
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			if (can_run) {
				can_run_more = true;
			} else {
				can_run_more = false;
			}
		}
		// LShift 누르고있으면, 달리기
		if (can_run_more && Input.GetKey(KeyCode.LeftShift)) {
			runningTimer += Time.deltaTime;
			if (playerProp.player_stamina > GROGGY_MIN_STAMINA) {
				speed += Time.deltaTime * 3f;
				if (speed > RUNNINGSPEED) {
					speed = RUNNINGSPEED;
				}
			} else if (playerProp.player_stamina > 10f) {
				speed += Time.deltaTime * 2f;
				if (speed > RUNNINGSPEED * 0.8f) {
					speed = RUNNINGSPEED * 0.8f;
				}
			} else if (playerProp.player_stamina > 0f) {
				speed += Time.deltaTime * 1.5f;
				if (speed > RUNNINGSPEED * 0.5f) {
					speed = RUNNINGSPEED * 0.5f;
				}
			}

			else{
				can_run_more = false;
				speed = GROGGYSPEED;
			}
		}
		// LShift 안누르고 있는 상태
		else if (!Input.GetKey(KeyCode.LeftShift)) {
			runningTimer = 0f;
			speed = WALKINGSPEED;
			if (!can_run) {
				speed = GROGGYSPEED;
			}
		}
		/* ↑ 스태미나 < 20이면 is_running=false가 되어서 LShift를 누른
		 * 채로는 다시 이동할 수 없는 버그가 있었다. is_running=false이면
		 * 두 블록 밑의 if문에서 speed = 0f 실행 후에 속도를 재조정할 수 
		 * 없어서 생긴 버그였다. 하단의 else문에서 속도를 따로 조정해
		 * 줌으로써 해결하였다. */
		else {
			if (can_run) {
				speed = WALKINGSPEED;
			} else {
				speed = GROGGYSPEED;
			}
		}

		// 아무방향키도 안누른상태
		if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) {
			runningTimer = 0f;
			speed = 0f;
		} else {
			// 이동 시 플레이어 바라보는 방향 설정
			if (Input.GetKey(KeyCode.DownArrow)) Player_HeadDown();
			if (Input.GetKey(KeyCode.UpArrow)) Player_HeadUp();
			if (Input.GetKey(KeyCode.RightArrow)) Player_HeadRight();
			if (Input.GetKey(KeyCode.LeftArrow)) Player_HeadLeft();

		}

		// !!! 스태미나의 소모와 회복 구현부 !!!
		if (speed >= SPEED_RUNNINGSTART) {
			playerProp.player_stamina -= STAMINADECREASE_RUNNING * staminaMNG.rate_stamina_usage * Time.deltaTime;
		} else if (0f < speed && speed < SPEED_RUNNINGSTART) {
			playerProp.player_stamina += STAMINAINCREASE_WALKING * Time.deltaTime;
		} else {
			playerProp.player_stamina += STAMINAINCREASE_RESTING * Time.deltaTime;
		}

		// 스태미나가 max보다 커지면 stamina = max로 고정
		if (playerProp.player_stamina > playerProp.player_max_stamina) {
			playerProp.player_stamina = playerProp.player_max_stamina;
		}

		// 스태미나가 20 이하면 달리기 시작하지 못하게 함 (그로기 상태)
		// (이미 달리고 있던 중에는 마저 달릴 수 있음)
		if (playerProp.player_stamina >= GROGGY_MIN_STAMINA) {
			can_run = true;
		} else if (0f < playerProp.player_stamina && playerProp.player_stamina < GROGGY_MIN_STAMINA) {
			can_run = false;
		} else {
			playerProp.player_stamina = 0f;
		}

		// !!! 걷기, 뛰기, 백스텝 구현부 (velocity를 이용) !!!
		h = Input.GetAxisRaw("Horizontal");
		v = Input.GetAxisRaw("Vertical");
		playerAngle = new Vector2(h, v);
		if (!flag_backstep) {
			player_rigid2D.velocity = playerAngle.normalized
				* playerProp.player_moveSpeed * speed;
		} else {
			player_rigid2D.velocity = BACKSTEPSPEED * playerAngle.normalized
				* playerProp.player_moveSpeed;
		}
	}



	public void Player_HeadUp() {
		transform.rotation = Quaternion.Euler(0, 0, 0);
	}
	public void Player_HeadDown() {
		transform.rotation = Quaternion.Euler(0, 0, 180);
	}
	public void Player_HeadLeft() {
		transform.rotation = Quaternion.Euler(0, 0, 90);
	}
	public void Player_HeadRight() {
		transform.rotation = Quaternion.Euler(0, 0, -90);
	}

}
