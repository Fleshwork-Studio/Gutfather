using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSlot : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    public IEnumerator RunEnemyTurn()
    {
        if (enemy != null && enemy.IsAlive())
            yield return StartCoroutine(enemy.MakeMove());

        yield return null;
    }
}
