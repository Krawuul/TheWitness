using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// Window who search all the meshRender without mesh Collider
/// </summary>
public class MeshColliderWindows : EditorWindow
{
    Vector2 scrollPosition = Vector2.zero;
    MeshRenderer[] m_gameObject;

    [MenuItem("Tools/Mesh Finder")]
    public static void OpenWindow()
    {
        GetWindow<MeshColliderWindows>("Mesh Finder");
    }

    private void OnGUI()
    {
        GUI.backgroundColor = Color.blue;
        if (GUILayout.Button("Search", GUILayout.Width(200)))
        {
            m_gameObject = Resources.FindObjectsOfTypeAll<MeshRenderer>();
        };
        GUI.backgroundColor = Color.white;

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        Debug.Log(m_gameObject.Length);
        for (int i = 0; i < m_gameObject.Length; i++)
        {
            if (m_gameObject[i].gameObject.GetComponent<MeshCollider>() == null) 
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(m_gameObject[i].gameObject, typeof(GameObject), true);

                if (GUILayout.Button("Apply Mesh Collider ( convex )", GUILayout.Width(180)))
                {
                   MeshCollider meshCollider = m_gameObject[i].gameObject.AddComponent<MeshCollider>();
                    meshCollider.convex = true;

                    if (PrefabUtility.GetPrefabInstanceStatus(m_gameObject[i].gameObject) == PrefabInstanceStatus.Connected)
                    {
                        string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(m_gameObject[i].gameObject);
                        PrefabUtility.ApplyPrefabInstance(m_gameObject[i].gameObject,InteractionMode.AutomatedAction);                        
                        AssetDatabase.Refresh();
                        m_gameObject = Resources.FindObjectsOfTypeAll<MeshRenderer>();
                    }
                };

                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView();
    }

    private void OnDisable()
    {
    }
}
