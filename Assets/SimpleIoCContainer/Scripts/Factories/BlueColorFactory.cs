using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BlueColorFactory : IColorFactory
{
    #region IColorFactory Members

    public GameObject GetColorObject()
    {
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.name = "blue";
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        gameObject.AddComponent<Collectable>();
        gameObject.AddComponent<Rigidbody>();
        return gameObject;
    }

    #endregion
}
