using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class TestScript : MonoBehaviour {

    public RectTransform tracker;

    // Use this for initialization
    void Start () {
        this.gameObject.UpdateAsObservable()
                .Subscribe(x =>
                {
                    var point = Camera.main.WorldToScreenPoint(this.transform.position);
                    tracker.transform.position = point;
                }).AddTo(this);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
