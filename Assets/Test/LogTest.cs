using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class LogTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;

        Debug.LogWarning("Warning");

        //StartCoroutine(MyMethod());        
	}

    void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
    {
        Debug.Log(condition + stackTrace + type);
    }

    //IEnumerator MyMethod()
    //{
    //    Debug.LogWarning("Warning");
    //    yield return new WaitForSeconds(2);
    //    Debug.LogError("Error");
    //}
	
	// Update is called once per frame
	void Update () {
        
	}
}
