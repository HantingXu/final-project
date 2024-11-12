using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro.EditorUtilities;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor planetShapeEditor;
    Editor mapEditor;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawSettingsEditor(planet.planetSettings, planet.OnPlanetSettingsUpdated, ref planetShapeEditor);
        DrawSettingsEditor(planet.mapSettings, planet.OnMapSettingsUpdated, ref mapEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref Editor editor)
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            CreateCachedEditor(settings, null, ref editor);
            editor.OnInspectorGUI();

            if (check.changed)
            {
                onSettingsUpdated();
            }
        }
    }
    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
