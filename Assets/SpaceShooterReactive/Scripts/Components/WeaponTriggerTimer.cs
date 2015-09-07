using UnityEngine;
using System.Collections;
using Zenject;
using UniRx;
using UniRx.Triggers;
using System;

public class WeaponTriggerTimer : MonoBehaviour
{
    [Inject]
    public WeaponModel Model;

    [PostInject]
    void InitializeComponent()
	{
         Observable.Interval(TimeSpan.FromSeconds(0.5))
            .Where(_ => Model.RxWeaponState.Value == WeaponModel.WeaponState.Active)
            .Subscribe(x => {
                Model.Fire();
            }).AddTo(this);            
	}
}
