using System.Collections;
using System.Collections.Generic;
using Gutfather.Assets.Events;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class VFXSelectedGem : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    void Awake()
    {
        Bus.Subscribe<NewGemSelectedEvent>(EnableVFX);

        gameObject.SetActive(false);
    }

    private void EnableVFX(NewGemSelectedEvent e)
    {
        if (e.position == Vector2Int.one * -1)
        {
            particleSystem.Stop();
            particleSystem.Clear();
            gameObject.SetActive(false);
            return;
        }

        var offset = new Vector2(0.5f, 0.5f);
        transform.position = (Vector2)e.position + offset;

        gameObject.SetActive(true);
    }
}
