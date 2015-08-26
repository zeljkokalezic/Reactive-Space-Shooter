using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class PlayerPresenter : MonoBehaviour
{

    public float tilt;
    public Done_Boundary boundary;
    public Transform shotSpawn;

    [Inject]
    private PlayerModel Model;

    //switch to factory !
    [Inject]
    private WeaponSpawner weaponSpawner;

    [PostInject]
    void InitializePresenter()
    {
        Assert.IsNotNull(Model);

        //for each player weapon create a weapon spawner - this naming convention is wierd - redesign

        //this can be ship driver component of the player
        this.gameObject.AddComponent<ObservableFixedUpdateTrigger>()
            .FixedUpdateAsObservable()
            .Where(_ => Model.RxPlayerState.Value == PlayerModel.PlayerState.Active)
            .Subscribe(x =>
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");

                Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
                GetComponent<Rigidbody>().velocity = movement * Model.RxPlayerSpeed.Value;

                GetComponent<Rigidbody>().position = new Vector3
                (
                    Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                    0.0f,
                    Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
                );

                GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
            });
    }
}
