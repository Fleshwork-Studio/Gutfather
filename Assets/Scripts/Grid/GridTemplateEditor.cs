using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridTemplate))]
public class GridTemplateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); 

        GridTemplate gridTemplate = (GridTemplate)target;

        if (GUILayout.Button(gridTemplate.IsRedactMode ? "Disable Redacting" : "Enable Redacting"))
        {
            gridTemplate.ToggleRedactMode();
        }

        if (GUILayout.Button("Set All Units Playable"))
        {
            gridTemplate.SetAllUnitsPlayable();
        }
    }
}
