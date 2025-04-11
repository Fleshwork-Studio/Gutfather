using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsHolder : MonoBehaviour
{
    private List<SkillUnit> skillsList = new List<SkillUnit>();

    public void AddSkill(SkillUnit skillUnit)
    {
        skillsList.Add(skillUnit);
    } 
    public IEnumerator UseSkills()
    {
        while(skillsList.Count > 0)
        {
            yield return StartCoroutine(skillsList[0].UseSkill());
            skillsList.RemoveAt(0);
        }

        yield return null;
    }
    public void ClearAllSkills()
    {
        skillsList.Clear();
    }
}
