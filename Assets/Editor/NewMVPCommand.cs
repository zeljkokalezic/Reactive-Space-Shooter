using UnityEngine;
using System.Collections;
using UnityEditor;

public class NewMVPCommand {

    [MenuItem("Project/Create/New MVP")]
    [MenuItem("Assets/Create/New MVP")]
    public static void Create()
    {

        // Show the selection window.
        var window = EditorWindow.GetWindow<NewMVPEditor>(true, "Create a new MVP", true);
        window.ShowPopup();

        window.path = AssetDatabase.GetAssetPath(Selection.activeObject);
    }
}
