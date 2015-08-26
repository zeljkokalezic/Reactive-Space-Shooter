using UnityEngine;
using System.Collections;
using Zenject;
using UniRx;
using System;
using UniRx.Triggers;

public class WeaponSpawner : MonoBehaviour
{
    [Inject]
    private GameModel game;

    [Inject]
    private PlayerModel player;

    [Inject]
    private WeaponPresenter.Factory weaponPresenterFactory;

    public GameObject weapon;
    private float nextFire;

    [PostInject]
    void Initialize()
    {
        this.gameObject.AddComponent<ObservableUpdateTrigger>()
            .UpdateAsObservable()
            .Where(_ => player.RxPlayerState.Value == PlayerModel.PlayerState.Active
                && Input.GetButton("Fire1")
                && Time.time > nextFire)//next fire should be a weapon property
            .Subscribe(x =>
            {
                weaponPresenterFactory.Create(weapon, player.PlayerWeapon);        
                nextFire = Time.time + player.RxPlayerFireRate.Value;
            });
    }
}
