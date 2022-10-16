using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMGR : MonoBehaviour
{
	public Transform parent_mobs;

	bool _can_spawn_tempEnemy = true;
	GameObject mob_instance;

	public MobData tempEnemy_data;

	public void Spawn_Mob(MobData mobData) {
		if (true) {
			float _randX = Random.Range(-8f, 8f);
			float _randY = Random.Range(-4f, 4f);
			mob_instance = Instantiate(mobData.Prefab_mob, new Vector2(_randX, _randY), Quaternion.identity, parent_mobs);
			mob_instance.GetComponent<MobMGR>().Init_Mob(mobData.Name, mobData.Hp, mobData.MoveSpeed, mobData.Damage, mobData.Exp);
		}
	}



	

    void Start()
    {
		for (int i = 0; i < 3; i++)
			Spawn_Mob(tempEnemy_data);
    }

    void Update()
    {
        
    }
}
