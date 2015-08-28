using UnityEngine;
using System.Collections;
using System;
using Zenject;
using UniRx;
using UniRx.Triggers;

public class ShipDriverAI : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public Boundary boundary;
        public float tilt;
        public float dodge;
        public float smoothing;
        public MinMax startWait;
        public MinMax maneuverTime;
        public MinMax maneuverWait;
    }

    [Inject]
    private Settings settings;

	private float currentSpeed;
	private float targetManeuver;

    [PostInject]
	void InitializeComponent ()
    {
        var rigidBody = GetComponent<Rigidbody>();
        currentSpeed = rigidBody.velocity.z;

        //corutine is simpler than to build the rx events
        //see http://stackoverflow.com/questions/3670534/change-interval-of-rx-operators for more info
        //StartCoroutine(Evade());
        //equivalent
        Observable.FromCoroutine(Evade).Subscribe().AddTo(this);


        //Observable.Interval(TimeSpan.FromSeconds(1))
        //    //.Where(_ => game.RxGameState.Value == GameModel.GameState.InProgress)
        //    .Subscribe(x =>
        //    {
        //        targetManeuver = UnityEngine.Random.Range(1, settings.dodge) * -Mathf.Sign(transform.position.x);
        //    })
        //    .AddTo(this);

        this.gameObject.FixedUpdateAsObservable()
            //.Where(_ => Model.RxPlayerState.Value == PlayerModel.PlayerState.Active)
            .Subscribe(x =>
            {
                float newManeuver = Mathf.MoveTowards(GetComponent<Rigidbody>().velocity.x, targetManeuver, settings.smoothing * Time.deltaTime);
                rigidBody.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
                rigidBody.position = new Vector3
                (
                    Mathf.Clamp(rigidBody.position.x, settings.boundary.xMin, settings.boundary.xMax),
                    0.0f,
                    Mathf.Clamp(rigidBody.position.z, settings.boundary.zMin, settings.boundary.zMax)
                );

                rigidBody.rotation = Quaternion.Euler(0, 0, rigidBody.velocity.x * -settings.tilt);
            }).AddTo(this);
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(settings.startWait.Min, settings.startWait.Max));
        while (true)
        {
            targetManeuver = UnityEngine.Random.Range(1, settings.dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(UnityEngine.Random.Range(settings.maneuverTime.Min, settings.maneuverTime.Max));
            targetManeuver = 0;
            yield return new WaitForSeconds(UnityEngine.Random.Range(settings.maneuverWait.Min, settings.maneuverWait.Max));
        }
    }
	

}
