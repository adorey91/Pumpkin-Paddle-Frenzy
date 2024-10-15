using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeAsset))]
public class UpgradeAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get a reference to the target script
        UpgradeAsset upgradeAsset = (UpgradeAsset)target;

        // Draw the default inspector except colliderRadius
        DrawDefaultInspectorExcept("colliderRadius");

        // Conditional display for colliderRadius when type is Health
        if (upgradeAsset.type == UpgradeAsset.StateUpgrade.Health)
        {
            // Draw the colliderRadius property only if type is Health
            EditorGUILayout.PropertyField(serializedObject.FindProperty("colliderRadius"));
        }

        // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }

    // Helper method to draw all properties except for the specified one
    private void DrawDefaultInspectorExcept(string propertyName)
    {
        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;

        while (prop.NextVisible(enterChildren))
        {
            if (prop.name != propertyName)
            {
                EditorGUILayout.PropertyField(prop, true);
            }
            enterChildren = false;
        }
    }
}
