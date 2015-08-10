using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx.Examples;
using UnityEngine;
using UniRx;

public class RxTester : MonoBehaviour
{
	// Use this for initialization
	void Start () {
        Sample05_ConvertFromCoroutine.GetWWW("http://google.com").Subscribe(x => Debug.Log(x));
        new Sample10_MainThreadDispatcher().Run();
        var logger = new Sample11_Logger();
        logger.ApplicationInitialize();
        logger.Run();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
