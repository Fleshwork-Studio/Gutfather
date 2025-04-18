using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSlot : MonoBehaviour
{
    public event Action<int> OnSelected;
    [SerializeField] private Enemy enemy;
    private int index;
    public IEnumerator RunEnemyTurn()
    {
        if (enemy != null && enemy.IsAlive())
            yield return StartCoroutine(enemy.MakeMove());

        yield return null;
    }
    public void SetIndex (int i) => index = i;
    void OnMouseDown()
    {
        OnSelected?.Invoke(index);
    }
}
