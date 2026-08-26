using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDiff : EditorWindow
{
    string pathA = "Assets/Scenes/Main.unity";
    string pathB = "Assets/Scenes/Main_HEAD.unity";
    Vector2 scroll;
    string result = "";

    [MenuItem("Tools/Scene Diff")]
    static void Open() => GetWindow<SceneDiff>("Scene Diff");

    void OnGUI()
    {
        pathA = EditorGUILayout.TextField("Current", pathA);
        pathB = EditorGUILayout.TextField("Baseline", pathB);

        if (GUILayout.Button("Compare")) result = Compare(pathA, pathB);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUILayout.TextArea(result, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }

    static string Compare(string a, string b)
    {
        var mapA = Collect(a);
        var mapB = Collect(b);
        var sb = new System.Text.StringBuilder();

        foreach (var k in mapA.Keys.Except(mapB.Keys).OrderBy(x => x))
            sb.AppendLine($"+ {k}");

        foreach (var k in mapB.Keys.Except(mapA.Keys).OrderBy(x => x))
            sb.AppendLine($"- {k}");

        foreach (var k in mapA.Keys.Intersect(mapB.Keys).OrderBy(x => x))
            if (mapA[k] != mapB[k])
                sb.AppendLine($"~ {k}\n    was: {mapB[k]}\n    now: {mapA[k]}");

        return sb.Length == 0 ? "Идентичны" : sb.ToString();
    }

    static Dictionary<string, string> Collect(string path)
    {
        var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        var map = new Dictionary<string, string>();

        foreach (var root in scene.GetRootGameObjects())
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                string key = GetPath(t);
                string val = $"active={t.gameObject.activeSelf} " +
                             $"pos={t.localPosition} rot={t.localEulerAngles} scale={t.localScale} " +
                             $"[{string.Join(",", t.GetComponents<Component>().Where(c => c).Select(c => c.GetType().Name).OrderBy(x => x))}]";
                map[key] = val;
            }

        EditorSceneManager.CloseScene(scene, true);
        return map;
    }

    static string GetPath(Transform t)
    {
        string p = t.name;
        while (t.parent) { t = t.parent; p = t.name + "/" + p; }
        return p;
    }
}