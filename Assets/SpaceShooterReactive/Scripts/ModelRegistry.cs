using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

//this will be a singleton that will track all the models in the game
//step 1: create models
//step 2: create events & subscriptions (on projectile hit -> destroy, on player health 0 -> game over, ...)
//step 3: wire the backend with the "frontend" (unity components)
//play :)


//this is the game data DB actually
//make this implement something like IRepository interface
//to enable testing of the game models later ?
public class ModelRegistry : Singleton<ModelRegistry>
{
    //make every model implement interface so that we can implement DI later ?
    //probably not every model will need interface
    //we can register model-interface pairs -> that whould actually implement simple DI
    //lets roll with this simple implementation for now and see how it goes
    public class ModelRegistryEntry
	{
        public object Model { get; set; } //base class for all models with the id ?
        public string ModelID { get; set; }
        public string PresenterID { get; set; } //this should be a list for multiple presenters
	}

    private int autoincrement = 0;
    private List<ModelRegistryEntry> models = new List<ModelRegistryEntry>();

    //this is a toolbox http://wiki.unity3d.com/index.php/Toolbox
    protected ModelRegistry() { } // guarantee this will be always a singleton only - can't use the constructor!

    void Awake()
    {

    }

    public T GetModel<T>(string id) where T : new()
    {
        T result = default(T);
        var registryEntry = I.models.Where(x => x.ModelID == id).FirstOrDefault();
        if (registryEntry == null)
        {
            //this can be transformed into basic dependency injection bu creating inject extension for mono behavours
            //and by creating [Inject] attribute, so the activator shoud now go recursively into creating all dependencies for the model
            //marked with [Inject] attribute
            result = Activator.CreateInstance<T>();//new T();
            I.models.Add(new ModelRegistryEntry() {
                ModelID = id,
                Model = result
            });
            autoincrement++;
        }
        else
        {
            result = (T)registryEntry.Model;
        }
        return result;
    }

    //function for removing the model
}
