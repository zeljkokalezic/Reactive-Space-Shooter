using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;

public class UIPresenter : MonoBehaviour
{

    public Button MyButton;

    //cashe the reference ?
    private PlayerModel Player { get { return ModelRegistry.I.GetModel<PlayerModel>("player");}}

	// Use this for initialization
	void Start () {
        Player.PlayerName.Subscribe(x => Debug.Log(x));

        MyButton.onClick.AsObservable().Subscribe(_ =>
            { 
                ////Debug.Log(GameMaster.I.name);                
                ////GameMaster.I.GetOrAddComponent<PlayerModel>();
                //var player = ModelRegistry.I.GetModel<PlayerModel>("player");
                Player.ChangeName(DateTime.Now.ToString());
                //Debug.Log(ModelRegistry.I.GetModel<PlayerModel>("player"));
            });

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
