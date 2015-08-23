using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class PlayerPresenter : MonoBehaviour {

    //public float speed;
    public float tilt;
    public Done_Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;

    [Inject]
    private PlayerModel player;
    //move this to weapon model
    private float nextFire;

    [PostInject]
    void InitializePresenter()
    {
        Assert.IsNotNull(player);

        //this can be ship driver component of the player
        this.gameObject.AddComponent<ObservableFixedUpdateTrigger>()
                .FixedUpdateAsObservable()
                .Where(_ => player.RxPlayerState.Value == PlayerModel.PlayerState.Active)
                .Subscribe(x =>
                {
                    //if (player.RxPlayerState.Value == PlayerModel.PlayerState.Active)
                    {
                        float moveHorizontal = Input.GetAxis("Horizontal");
                        float moveVertical = Input.GetAxis("Vertical");

                        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
                        GetComponent<Rigidbody>().velocity = movement * player.RxPlayerSpeed.Value;

                        GetComponent<Rigidbody>().position = new Vector3
                        (
                            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                            0.0f,
                            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
                        );

                        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
                    }                    
                });

        //this can be weapon component of the player
        this.gameObject.AddComponent<ObservableUpdateTrigger>()
                .UpdateAsObservable()                
                .Where(_ => player.RxPlayerState.Value == PlayerModel.PlayerState.Active 
                    && Input.GetButton("Fire1")
                    && Time.time > nextFire)//next fire should be a weapon property
                .Subscribe(x =>
                {                    
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                    nextFire = Time.time + player.RxPlayerFireRate.Value;
                });        
    }
}
