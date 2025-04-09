using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public virtual IEnumerator MakeMove()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Enemy made a move");
    }
}
