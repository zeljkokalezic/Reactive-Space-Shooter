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

        if (GUILayout.Button("Create"))
        {
            if (mvpname == "")
            {
                EditorUtility.DisplayDialog("Info", "Please enter name.", "Ok");
            }
            else
            {

                //"View", view is the unity object(s) in the scene

                //"Presenter"
                string filename = path + "/" + mvpname + "Presenter.cs";
                Debug.Log(filename);
                if (!File.Exists(filename))
                {
                    using (StreamWriter outfile = new StreamWriter(filename))
                    {
                        outfile.Write(
@"using System;
using UniRx;
using UnityEngine;

public class "+ mvpname +@"Presenter : PresenterBase {

    // indicate children dependency
    protected override IPresenter[] Children
    {
        get
        {
            return EmptyChildren;
        }
    }

    // This Phase is Parent -> Child
    // You can pass argument to children, but you can't touch child's property
    protected override void BeforeInitialize()
    {
        
    }

    // This Phase is Child -> Parent
    // You can touch child's property safety
    protected override void Initialize()
    {
        
    }
}");
                    }
                }

                //"Model"
                filename = path + "/" + mvpname + "Model.cs";
                Debug.Log(filename);
                if (!File.Exists(filename))
                {
                    using (StreamWriter outfile = new StreamWriter(filename))
                    {
                        //outfile.WriteLine("using UnityEngine;");
                        //outfile.WriteLine("using System.Collections;");
                        //outfile.WriteLine("");
                        //outfile.WriteLine("public class " + mvpname + "Model {");
                        //outfile.WriteLine("}");
                        outfile.Write(
@"using UnityEngine;
using System.Collections;

public class " + mvpname + @"Model
{

}");
                    }
                }


                AssetDatabase.Refresh();

                Close();
            }          
        }
    }

}
