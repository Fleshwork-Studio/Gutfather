using UnityEngine;

[CreateAssetMenu(fileName = "GemType", menuName = "Match3/GemType")]
public class GemType : ScriptableObject
{
    public enum GemTypeEnum
    { 
        Eye, // Magic
        Bone, // Atack
        Tooth, // Debuff
        Meat, // Heal
        Skin, // Sheild
        Fat, // Buff
    }

    public GemTypeEnum gemType;
    public Sprite sprite;
}



