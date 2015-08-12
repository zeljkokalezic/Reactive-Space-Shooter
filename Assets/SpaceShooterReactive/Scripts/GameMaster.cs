using UnityEngine;
using System.Collections;

//this will be a singleton that will track all the models in the game
//step 1: create models
//step 2: create events & subscriptions (on projectile hit -> destroy, on player health 0 -> game over, ...)
//step 3: wire the backend with the "frontend" (unity components)
//play :)

public class GameMaster : Singleton<GameMaster>
{
    //this is a toolbox http://wiki.unity3d.com/index.php/Toolbox
    protected GameMaster() { } // guarantee this will be always a singleton only - can't use the constructor!

    void Awake()
    {
        // Your initialization code here
        //initialize game model - handle spawning & game over
        //initialize player model
        //initialize player weapon model
        //initilaize asteroid model(s) - on spawn
    }

    //moved to the singleton
    // (optional) allow runtime registration of global objects
    //static public T RegisterComponent<T>() where T : Component
    //{
    //    return I.GetOrAddComponent<T>();
    //}
}
