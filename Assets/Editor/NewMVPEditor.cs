using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class NewMVPEditor : EditorWindow
{
    public string path = "";
    public string mvpname = "";


    public void OnGUI()
    {
        this.Focus();

        GUILayout.Label("MVP Name");
        GUI.SetNextControlName("mvpname");
        mvpname = EditorGUILayout.TextField(mvpname);
        EditorGUI.FocusTextInControl("mvpname");

        //"View", view is the unity object in the scene

        if (GUILayout.Button("Create Model"))
        {
            if (mvpname == "")
            {
                EditorUtility.DisplayDialog("Info", "Please enter name.", "Ok");
            }
            else
            {
                //"Model"
                string filename = path + "/" + mvpname + "Model.cs";
                Debug.Log(filename);
                if (!File.Exists(filename))
                {
                    using (StreamWriter outfile = new StreamWriter(filename))
                    {
                        #region ModelFile
                        outfile.Write(
@"using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class " + mvpname + @"Model
{
    public class Factory : Factory<Settings, " + mvpname + @"Model>
    {
    }

    [Serializable]
    public class Settings
    {

    }

    [Inject]
    public " + mvpname + @"Model(Settings settings)
    {

    }
}");
                        #endregion
                    }
                }

                AssetDatabase.Refresh();

                //Close();
            }
        }

        if (GUILayout.Button("Create Presenter"))
        {
            if (mvpname == "")
            {
                EditorUtility.DisplayDialog("Info", "Please enter name.", "Ok");
            }
            else
            {
                //"Presenter"
                string filename = path + "/" + mvpname + "Presenter.cs";
                Debug.Log(filename);
                if (!File.Exists(filename))
                {
                    using (StreamWriter outfile = new StreamWriter(filename))
                    {
                        #region PresenterFile
                        outfile.Write(
@"using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class " + mvpname + @"Presenter : MonoBehaviour
{
    public class Factory : PrefabFactory<" + mvpname + @"Model, " + mvpname + @"Presenter>
    {
    }

    [Serializable]
    public class Settings
    {
    }

    [Inject]
    public " + mvpname + @"Model Model { get; private set; }

    [Inject]
    private Settings settings { get; private set; }

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {

    }
}");
                        #endregion
                    }
                }
                AssetDatabase.Refresh();

                //Close();

            }
        }
    }
}
