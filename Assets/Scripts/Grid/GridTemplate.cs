using System.Collections;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GridTemplate : MonoBehaviour
{
    private GridTemplateUnit[,] gridUnitsArray = new GridTemplateUnit[10, 10];

    private bool isRedactMode = false; 

    public bool IsRedactMode => isRedactMode; 

    private void Awake()
    {
        InitializeGridUnits();
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        gameObject.SetActive(false);
    }

    private void InitializeGridUnits()
    {
        for (int x = 0; x < 10; x++)
        {
            var child = transform.GetChild(x);

            for (int y = 0; y < 10; y++)
            {
                gridUnitsArray[x, y] = child.GetChild(y).GetComponent<GridTemplateUnit>();
            }
        }
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    public void ToggleRedactMode()
    {
        isRedactMode = !isRedactMode;
        Debug.Log("Redacting: " + (isRedactMode ? "Enabled" : "Disabled"));
    }

    // Redacting for units playability
    private void OnSceneGUI(SceneView sceneView)
    {
        if (!isRedactMode) return;

        Event e = Event.current;
        if (e.type == EventType.MouseDown && (e.button == 0 || e.button == 1))
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldRay.origin, worldRay.direction);

            if (hit.collider != null)
            {
                GridTemplateUnit unit = hit.collider.GetComponent<GridTemplateUnit>();
                if (unit != null)
                {
                    unit.isPlayable = !unit.isPlayable;
                    unit.GetComponent<SpriteRenderer>().color = unit.isPlayable ? Color.green : Color.red;

                    // Saving changes for editor
                    EditorUtility.SetDirty(unit);

                    e.Use();
                }
            }
        }
    }

    public void SetAllUnitsPlayable()
    {
        foreach (var unit in gridUnitsArray)
        {
            unit.isPlayable = true;
            unit.GetComponent<SpriteRenderer>().color = Color.green;

            // Saving changes for editor
            EditorUtility.SetDirty(unit);
        }
    }

    public bool IsUnitPlayable(int x, int y) { return gridUnitsArray[x, y].isPlayable; }
}
