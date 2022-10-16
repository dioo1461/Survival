using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Object/Enemy", order = 0)]
public class MobData : ScriptableObject
{
	public GameObject Prefab_mob => _prefab_mob;
	public string Name => _name;
	public float Hp => _hp;
	public float MoveSpeed => _moveSpeed;
	public float Damage => _damage;
	public int Exp => _exp;
	[SerializeField] string _name;
	[SerializeField] float _hp;
	[SerializeField] float _moveSpeed;
	[SerializeField] float _damage;
	[SerializeField] int _exp;
	[SerializeField] GameObject _prefab_mob;
	// [SerializeField] drop_item;
}
