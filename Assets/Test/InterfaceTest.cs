using UnityEngine;
using System.Collections;

public class InterfaceTest : MonoBehaviour {

    public Object testClass;
    public string testString;

	// Use this for initialization
	void Start () {
        Debug.Log(testClass is ITest);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}