using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public PlayerProp playerProp;
	public StaminaMGR staminaMNG;
	public Rigidbody2D player_rigid2D;

	public enum PlayerAngleEnum {
		UP, DOWN, LEFT, RIGHT,
		UPLEFT, UPRIGHT, DOWNLEFT, DOWNRIGHT
	};
	public enum PlayerMovingStateEnum {
		stop,
		walking,
		running,
		backstep
	};

	public PlayerAngleEnum playerAngle_state;
	public PlayerMovingStateEnum playerMovingState;
	public Vector3 playerAngleToVector3_normalized;


	bool _approve_movement = true;

	float _speed = 1f;
	bool _flag_backstep;
	float _runningTimer = 0f;
	bool _can_run = true;
	bool _can_run_more = true;
	bool _is_moving = false;


	/* RUNNINGSPEED : 달리기 속도
	 * WALKINGSPEED : 걷는 속도
	 * GROGGYSPEED : 스태미나 소진시 걷는속도
	 * SPEED_RUNNINGSTART : 달리기로 취급되는 최소 속도
	 * GROGGY_MIN_STAMINA : 스태미나가 이 수치 이하이면 그로기상태 적용
	 * STAMINADECREASE_RUNNING : 달릴 때 초당 스태미나 감소량
	 * STAMINAINCREASE_WALKING : 걸을 때 초당 스태미나 회복량
	 * STAMINAINCREASE_RESTING : 움직이지 않을 때 초당 스태미나 회복량
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
		// 이동이 가능한 상태인지 확인
		if (_approve_movement) {

			// LCtrl 누르고있으면, 백스텝
			if (Input.GetKey(UserKeyInfo.backStep)) {
				_flag_backstep = true;
				playerMovingState = PlayerMovingStateEnum.backstep;
			} else {
				_flag_backstep = false;
			}

			// LShift를 뗀 순간, 달릴 수 있는 상태인지 확인
			if (Input.GetKeyUp(UserKeyInfo.run)) {
				if (_can_run) {
					_can_run_more = true;
				} else {
					_can_run_more = false;
				}
			}
			// LShift를 누르는 순간, 달릴 수 있는 상태인지 확인
			if (Input.GetKeyDown(UserKeyInfo.run)) {
				if (_can_run) {
					_can_run_more = true;
				} else {
					_can_run_more = false;
				}
			}
			// LShift 누르고있고 달릴 수 있는 상태면, 달리기
			if (_can_run_more && Input.GetKey(UserKeyInfo.run)) {
				_runningTimer += Time.deltaTime;
				playerMovingState = PlayerMovingStateEnum.running;
				if (playerProp.current_stamina > GROGGY_MIN_STAMINA) {
					_speed += Time.deltaTime * 3f;
					if (_speed > RUNNINGSPEED) {
						_speed = RUNNINGSPEED;
					}
				} else if (playerProp.current_stamina > 10f) {
					_speed += Time.deltaTime * 2f;
					if (_speed > RUNNINGSPEED * 0.8f) {
						_speed = RUNNINGSPEED * 0.8f;
					}
				} else if (playerProp.current_stamina > 0f) {
					_speed += Time.deltaTime * 1.5f;
					if (_speed > RUNNINGSPEED * 0.5f) {
						_speed = RUNNINGSPEED * 0.5f;
					}
				} else {
					_can_run_more = false;
					_speed = GROGGYSPEED;
				}
			}
			// LShift 안누르고 있는 상태
			else if (!Input.GetKey(UserKeyInfo.run)) {
				_runningTimer = 0f;
				_speed = WALKINGSPEED;
				playerMovingState = PlayerMovingStateEnum.walking;
				if (!_can_run) {
					_speed = GROGGYSPEED;
				}
			}
			/* ↑ 스태미나 < 20이면 is_running=false가 되어서 LShift를 누른
			 * 채로는 다시 이동할 수 없는 버그가 있었다. is_running=false이면
			 * 두 블록 밑의 if문에서 speed = 0f 실행 후에 속도를 재조정할 수 
			 * 없어서 생긴 버그로, 하단의 else문에서 속도를 따로 조정해
			 * 줌으로써 해결하였다. */
			else { // Lshift 누르고 있으나 달릴 수 없는 상태
				playerMovingState = PlayerMovingStateEnum.walking;
				if (_can_run) {
					_speed = WALKINGSPEED;
				} else {
					_speed = GROGGYSPEED;
				}
			}

			// 아무방향키도 안누른상태
			if (!Input.GetKey(UserKeyInfo.moveUp_1) && !Input.GetKey(UserKeyInfo.moveDown_1) && !Input.GetKey(UserKeyInfo.moveLeft_1) && !Input.GetKey(UserKeyInfo.moveRight_1)) {
				_runningTimer = 0f;
				_speed = 0f;
				_is_moving = false;
				playerMovingState = PlayerMovingStateEnum.stop;
			} else {
				// 이동 시 플레이어 바라보는 방향 설정
				if (Input.GetKey(UserKeyInfo.moveRight_1)) Player_HeadRight();
				if (Input.GetKey(UserKeyInfo.moveLeft_1)) Player_HeadLeft();
				if (Input.GetKey(UserKeyInfo.moveDown_1)) {
					if (Input.GetKey(UserKeyInfo.moveRight_1) && Input.GetKey(UserKeyInfo.moveLeft_1)) Player_HeadDown();
					else if (Input.GetKey(UserKeyInfo.moveRight_1)) Player_HeadDownright();
					else if (Input.GetKey(UserKeyInfo.moveLeft_1)) Player_HeadDownleft();
					else Player_HeadDown();
				}
				if (Input.GetKey(UserKeyInfo.moveUp_1)) {
					if (Input.GetKey(UserKeyInfo.moveRight_1) && Input.GetKey(UserKeyInfo.moveLeft_1)) Player_HeadUp();
					else if (Input.GetKey(UserKeyInfo.moveRight_1)) Player_HeadUpright();
					else if (Input.GetKey(UserKeyInfo.moveLeft_1)) Player_HeadUpleft();
					else Player_HeadUp();
				}
				

			}
		} else {
			_speed = 0f;
		}
		// !!! 스태미나의 소모와 회복 구현부 !!!
		if (_speed >= SPEED_RUNNINGSTART) {
			playerProp.current_stamina -= STAMINADECREASE_RUNNING * staminaMNG.rate_stamina_usage * Time.deltaTime;
		} else if (0f < _speed && _speed < SPEED_RUNNINGSTART) {
			playerProp.current_stamina += STAMINAINCREASE_WALKING * Time.deltaTime;
		} else {
			playerProp.current_stamina += STAMINAINCREASE_RESTING * Time.deltaTime;
		}

		// 스태미나가 max보다 커지면 stamina = max로 고정
		if (playerProp.current_stamina > playerProp.max_stamina) {
			playerProp.current_stamina = playerProp.max_stamina;
		}

		// 스태미나가 20 이하면 달리기 시작하지 못하게 함 (그로기 상태)
		// (이미 달리고 있던 중에는 마저 달릴 수 있음)
		if (playerProp.current_stamina >= GROGGY_MIN_STAMINA) {
			_can_run = true;
		} else if (0f < playerProp.current_stamina && playerProp.current_stamina < GROGGY_MIN_STAMINA) {
			_can_run = false;
		} else {
			playerProp.current_stamina = 0f;
		}

		// !!! 실제 플레이어블 캐릭터의 이동 구현부 (velocity를 이용) !!!
		float _h = Input.GetAxisRaw("Horizontal");
		float _v = Input.GetAxisRaw("Vertical");
		Vector3 _direction = new Vector3(_h, _v).normalized;
		if (!_flag_backstep) {
			player_rigid2D.velocity = _direction * playerProp.moveSpeed * _speed;
		} else {
			player_rigid2D.velocity = BACKSTEPSPEED * _direction * playerProp.moveSpeed * _speed;
		}
		
	}

	// approve_movement 접근 시 스크립트 간 간섭을 피하기 위해 lock으로 변수를 관리
	int _lock = 0;
	void LateUpdate() {
		_approve_movement = (_lock == 0) ? true : false;
	}

	public void Approve_Movement() {
		if (_lock != 0) {
			_lock--;
		}
	}
	public void Restrict_Movement() {
		_lock++;
	}
	

	public void Player_HeadUp() {
		if (playerAngle_state != PlayerAngleEnum.UP) {
			transform.rotation = Quaternion.Euler(0, 0, 0);
			playerAngle_state = PlayerAngleEnum.UP;
			playerAngleToVector3_normalized = Vector3.up;
		}
	}
	public void Player_HeadDown() {
		if (playerAngle_state != PlayerAngleEnum.DOWN) {
			transform.rotation = Quaternion.Euler(0, 0, 180);
			playerAngle_state = PlayerAngleEnum.DOWN;
			playerAngleToVector3_normalized = Vector3.down;
		}
	}
	public void Player_HeadLeft() {
		if (playerAngle_state != PlayerAngleEnum.LEFT) {
			transform.rotation = Quaternion.Euler(0, 0, 90);
			playerAngle_state = PlayerAngleEnum.LEFT;
			playerAngleToVector3_normalized = Vector3.left;
		}
	}
	public void Player_HeadRight() {
		if (playerAngle_state != PlayerAngleEnum.RIGHT) {
			transform.rotation = Quaternion.Euler(0, 0, -90);
			playerAngle_state = PlayerAngleEnum.RIGHT;
			playerAngleToVector3_normalized = Vector3.right;
		}
	}
	public void Player_HeadUpleft() {
		if (playerAngle_state != PlayerAngleEnum.UPLEFT) {
			transform.rotation = Quaternion.Euler(0, 0, 45);
			playerAngle_state = PlayerAngleEnum.UPLEFT;
			playerAngleToVector3_normalized = new Vector3(-1, 1, 0).normalized;
		}
	}
	public void Player_HeadUpright() {
		if (playerAngle_state != PlayerAngleEnum.UPRIGHT) {
			transform.rotation = Quaternion.Euler(0, 0, -45);
			playerAngle_state = PlayerAngleEnum.UPRIGHT;
			playerAngleToVector3_normalized = new Vector3(1, 1, 0).normalized;
		}
	}
	public void Player_HeadDownleft() {
		if (playerAngle_state != PlayerAngleEnum.DOWNLEFT) {
			transform.rotation = Quaternion.Euler(0, 0, 135);
			playerAngle_state = PlayerAngleEnum.DOWNLEFT;
			playerAngleToVector3_normalized = new Vector3(-1, -1, 0).normalized;
		}
	}
	public void Player_HeadDownright() {
		if (playerAngle_state != PlayerAngleEnum.DOWNRIGHT) {
			transform.rotation = Quaternion.Euler(0, 0, -135);
			playerAngle_state = PlayerAngleEnum.DOWNRIGHT;
			playerAngleToVector3_normalized = new Vector3(1, -1, 0).normalized;
		}
	}
}
