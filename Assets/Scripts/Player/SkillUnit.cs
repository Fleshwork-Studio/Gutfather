using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SkillUnit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    MatchType matchType;
    GemTypeSO gemType;
    GemSkills gemSkills;
    GemSkillSO gemSkillSO;
    public void SetParametres(MatchType matchType, GemTypeSO gemType, GemSkillSO gemSkillSO)
    {
        this.matchType = matchType;
        this.gemType = gemType;

        if (gemSkillSO != null)
        {
            this.gemSkillSO = gemSkillSO;
            sprite.sprite = this.gemSkillSO.icon;
        }

        SetGemSkill();

        Debug.Log($"Skill Unit of {gemType.gemType}, combo {matchType} created");
    }

    public IEnumerator UseSkill()
    {
        Debug.Log($"Used {matchType} skill of {gemType.gemType}");
        yield return gemSkills.UseSkillOfType(matchType);
    }

    private void SetGemSkill()
    {
        switch (gemType.gemType)
        {
            case GemTypeEnum.Bone:
                gemSkills = gameObject.AddComponent<BoneSkills>();
                break;

            case GemTypeEnum.Eye:
                gemSkills = gameObject.AddComponent<EyeSkills>();
                break;

            case GemTypeEnum.Tooth:
                gemSkills = gameObject.AddComponent<ToothSkills>();
                break;

            case GemTypeEnum.Meat:
                gemSkills = gameObject.AddComponent<MeatSkills>();
                break;

            case GemTypeEnum.Skin:
                gemSkills = gameObject.AddComponent<SkinSkills>();
                break;

            case GemTypeEnum.Fat:
                gemSkills = gameObject.AddComponent<FatSkills>();
                break;

            default:
                gemSkills = gameObject.AddComponent<GemSkills>();
                break;
        }
    }

}
