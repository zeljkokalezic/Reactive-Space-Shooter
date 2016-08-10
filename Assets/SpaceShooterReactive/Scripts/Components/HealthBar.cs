using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class HealthBar : MonoBehaviour
{
    public class Factory : PrefabFactory<IDamageable, HealthBar>
    {
    }

    [Inject]
    public IDamageable Model;

    public Slider healthBar;

    [PostInject]
    void InitializeComponent()
    {
        healthBar.maxValue = Model.RxHealth.Value;

        this.gameObject.LateUpdateAsObservable().DelaySubscription(TimeSpan.FromMilliseconds(100))
            .Subscribe(x =>
            {
                healthBar.gameObject.SetActive(true);
                var point = Camera.main.WorldToScreenPoint(this.transform.parent.position);
                healthBar.transform.position = point;
                healthBar.value = Model.RxHealth.Value;
                //see how to handle parent rotation later
                //rework strucure so that empty game object holds 3d model and UI elements separately
                //then rotate just 3d model :)
                //healthBar.transform.rotation = Quaternion.identity;
            }).AddTo(this);
    }

    public void Hide()
    {
        //hide the slider until it is moved to position
        healthBar.gameObject.SetActive(false);
    }
}
