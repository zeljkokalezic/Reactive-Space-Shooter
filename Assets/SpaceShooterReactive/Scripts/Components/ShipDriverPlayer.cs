﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class ShipDriverPlayer : MonoBehaviour
{
    public class Factory : ComponentFactory<PlayerModel, ShipDriverPlayer>
    {
    }

    [Serializable]
    public class Settings
    {
        public Boundary boundary;
        public float tilt;
    }

    [Inject]
    private ShipModel Model;

    [Inject]
    private Settings settings;

    [PostInject]
	void InitializeComponent ()
	{
        this.gameObject.FixedUpdateAsObservable()
            .Where(_ => Model.RxShipState.Value == ShipModel.ShipState.Active)
            .Subscribe(x =>
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");

                //cashe rigidbody here
                Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
                GetComponent<Rigidbody>().velocity = movement * Model.RxShipSpeed.Value;

                GetComponent<Rigidbody>().position = new Vector3
                (
                    Mathf.Clamp(GetComponent<Rigidbody>().position.x, settings.boundary.xMin, settings.boundary.xMax),
                    0.0f,
                    Mathf.Clamp(GetComponent<Rigidbody>().position.z, settings.boundary.zMin, settings.boundary.zMax)
                );

                GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -settings.tilt);
            });
	}
}
