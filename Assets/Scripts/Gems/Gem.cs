using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Gem : MonoBehaviour
{
    public GemTypeSO type;

    public void SetType(GemTypeSO type)
    {
        this.type = type;
        GetComponent<SpriteRenderer>().sprite = type.sprite;
    }

    public GemTypeSO GetGemType() => type;

    public void DestroyGem() => Destroy(gameObject);
}
