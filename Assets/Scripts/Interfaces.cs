using UnityEngine;
using System.Collections;

public interface IEnemy
{
	void dealDamage();
	void takeDamage(float damage);
}

public interface IEnemyAI
{
	void setPlayer(Transform playerTransform);
}
