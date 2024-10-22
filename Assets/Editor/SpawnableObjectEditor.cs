using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnableObjects))]
public class SpawnableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get a reference to the target object
        SpawnableObjects spawnableObject = (SpawnableObjects)target;

        // Draw the default ObjectType field
        spawnableObject.type = (SpawnableObjects.ObjectType)EditorGUILayout.EnumPopup("Object Type", spawnableObject.type);

        // Draw the spawnPrefab and spawnWeight fields
        //spawnableObject.spawnPrefab = (GameObject)EditorGUILayout.ObjectField("Spawn Prefab", spawnableObject.spawnPrefab, typeof(GameObject), false);        //spawnableObject.spawnPrefab = (GameObject)EditorGUILayout.ObjectField("Spawn Prefab", spawnableObject.spawnPrefab, typeof(GameObject), false);

        // Conditionally show the 'spawnWeight' field only if the ObjectType is not FinishLine
        if (spawnableObject.type != SpawnableObjects.ObjectType.FinishLine)
        {
            //spawnableObject.spawnWeight = EditorGUILayout.FloatField("Spawn Weight", spawnableObject.spawnWeight);
        }

        // Conditionally show the 'collectableValue' field if the ObjectType is Collectable
        if (spawnableObject.type == SpawnableObjects.ObjectType.Collectable)
        {
            spawnableObject.collectableValue = EditorGUILayout.IntField("Collectable Value", spawnableObject.collectableValue);
        }

        // Save changes back to the ScriptableObject
        EditorUtility.SetDirty(spawnableObject);
    }
}
