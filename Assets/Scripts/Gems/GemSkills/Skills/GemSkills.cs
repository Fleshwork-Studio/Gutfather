using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSkills : MonoBehaviour
{
    public virtual IEnumerator UseSkillOfType(MatchType matchType)
    {
        switch (matchType)
        {
            case MatchType.Three3:
                yield return StartCoroutine(UseBasicSkill());
                break;
            case MatchType.Four4:
                yield return StartCoroutine(UseMediumSkill());
                break;
            case MatchType.Five5:
                yield return StartCoroutine(UseStrongSkill());
                break;
            case MatchType.Six6:
                yield return StartCoroutine(UseHyperSkill());
                break;
            case MatchType.Cross:
                yield return StartCoroutine(UseCrossSkill());
                break;
            default:
                Debug.LogWarning("Unknown or Null match type passed to UseSkillOfType.");
                break;
        }
    }

    public virtual IEnumerator UseBasicSkill() // 3 in a row
    {
        //Debug.Log("Basic Atack");
        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator UseMediumSkill() // 4 in a row
    {
        //Debug.Log("Medium Atack");
        yield return new WaitForSeconds(1f);

    }
    public virtual IEnumerator UseCrossSkill() // Cross-X combo
    {
        //Debug.Log("Cross Atack");
        yield return new WaitForSeconds(1f);

    }
    public virtual IEnumerator UseStrongSkill() // 5 in a row
    {
        //Debug.Log("Strong Atack");
        yield return new WaitForSeconds(1f);

    }
    public virtual IEnumerator UseHyperSkill() // 6 in a row
    {
        //Debug.Log("Hyper Atack");
        yield return new WaitForSeconds(1f);
    }
}
