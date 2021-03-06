﻿using UnityEngine;
using System.Collections;
using Zenject;
using UniRx;
using UniRx.Triggers;

public class WeaponTriggerButton : MonoBehaviour
{
    [Inject]
	public string buttonName;

    [Inject]
    public WeaponModel Model;

    [PostInject]
    void InitializeComponent()
	{
        this.gameObject.UpdateAsObservable()
            .Where(x => Model.RxWeaponState.Value == WeaponModel.WeaponState.Active && Input.GetButton(buttonName))
            .Subscribe(x => {
                Model.Fire();
            }).AddTo(this);
    }
}
