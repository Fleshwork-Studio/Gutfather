using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneSkills : GemSkills
{
    public override IEnumerator UseBasicSkill()
    {
        Enemy enemy = FightField.Instance.GetSelectedEnemy();

        // TODO: Make scriptable object with skill stats
        if (enemy != null && enemy.IsAlive())
        {
            yield return StartCoroutine(enemy.InflictDamage(110));

            Debug.Log("Bone skill damaged enemy");
        }

        yield return new WaitForSeconds(1f);
    }
}
