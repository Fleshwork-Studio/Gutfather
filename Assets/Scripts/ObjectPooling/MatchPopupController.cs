using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPopupController : MonoBehaviour
{
    public static MatchPopupController Instance;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 5;

    private Queue<VFXMatchPopup> pool = new Queue<VFXMatchPopup>();
    private MatchComboController matchController;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Bus.Subscribe<ComboMatchEvent>(SpawnObject);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj.GetComponent<VFXMatchPopup>());
        }
    }

    public void ReturnObject(VFXMatchPopup popup)
    {
        popup.Disable();
        pool.Enqueue(popup);
    }

    public void SpawnObject(ComboMatchEvent e)
    {
        if (pool.Count > 0)
        {
            VFXMatchPopup popup = pool.Dequeue();
            popup.Enable(e);
        }
        else
        {
            GameObject obj = Instantiate(prefab);
            obj.GetComponent<VFXMatchPopup>().Enable(e);
        }
    }
}
