using UnityEngine;
using System.Collections;
using Zenject;
using UniRx;
using System;
using UniRx.Triggers;

//this can also be injectable, as prefab for example...
public class WeaponSpawner : MonoBehaviour
{
    [Inject]
    private GameModel game;

    [Inject]
    private PlayerModel player;

    [Inject]
    private WeaponPresenter.Factory weaponPresenterFactory;

    public GameObject weapon;
    public Transform mountPosition;
    //move this to weapon model
    private float nextFire;

    [PostInject]
    void Initialize()//GameModel game, PlayerModel player, WeaponPresenter.Factory weaponPresenterFactory)
    {
        //Observable.Interval(TimeSpan.FromSeconds(1))
        //    .Where(_ => game.RxGameState.Value == GameModel.GameState.InProgress)
        //    .Subscribe(x =>
        //        {

        //        })
        //    .AddTo(this);

        //this can be weapon component of the player
        var weaponPresenterSettings = new WeaponPresenter.Settings() { mountPosition = player.PlayerWeapon.RxPlayerWeaponMountPoint.Value };//mountPosition };

        this.gameObject.AddComponent<ObservableUpdateTrigger>()
                .UpdateAsObservable()
                .Where(_ => player.RxPlayerState.Value == PlayerModel.PlayerState.Active
                    && Input.GetButton("Fire1")
                    && Time.time > nextFire)//next fire should be a weapon property
                .Subscribe(x =>
                {
                    weaponPresenterFactory.Create(weapon, weaponPresenterSettings);
                    //Instantiate(weapon, shotSpawn.position, shotSpawn.rotation);
                    nextFire = Time.time + player.RxPlayerFireRate.Value;
                });
    }
}
