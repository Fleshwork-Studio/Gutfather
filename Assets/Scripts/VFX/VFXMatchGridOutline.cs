using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VFXMatchGridOutline : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 15;

    private Queue<SpriteRenderer> pool = new Queue<SpriteRenderer>();

    Sequence sequence;

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj.GetComponent<SpriteRenderer>());
        }
    }

    private void Start()
    {
        MatchComboController.Instance.OnMatchPositions += OnMatch;
    }

    private void OnMatch(List<Vector2Int> hashset)
    {
        foreach(Vector2Int position in hashset)
        {
            var sprite = GetObject();

            sprite.transform.position = position + Vector2.one * 0.5f;

            PlayAnimation(sprite);
        }
    }

    public void ReturnObject(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.gameObject.SetActive(false);
        pool.Enqueue(spriteRenderer);
    }

    public SpriteRenderer GetObject()
    {
        SpriteRenderer sprite;

        if (pool.Count > 0)
        {
            sprite = pool.Dequeue();
            sprite.gameObject.SetActive(true);

            return sprite;
        }
        else
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(true);

            sprite = obj.GetComponent<SpriteRenderer>();
        }

        return sprite;
    }

    private void PlayAnimation(SpriteRenderer sprite)
    {
        Sequence sequence = DOTween.Sequence();

        sprite.color = Color.clear;

        sequence.Append(sprite.DOColor(Color.white, 0.3f).SetEase(Ease.InQuad));
        sequence.AppendInterval(0.5f);
        sequence.Append(sprite.DOColor(Color.clear, 0.2f).SetEase(Ease.OutQuad));
        sequence.OnComplete(() => ReturnObject(sprite));

        sequence.Play();
    }
}
