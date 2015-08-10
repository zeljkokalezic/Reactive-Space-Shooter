using System;
using UniRx;
using UnityEngine;

public class TestPresenter : PresenterBase {

    // indicate children dependency
    protected override IPresenter[] Children
    {
        get
        {
            return EmptyChildren;
        }
    }

    // This Phase is Parent -> Child
    // You can pass argument to children, but you can't touch child's property
    protected override void BeforeInitialize()
    {
        
    }

    // This Phase is Child -> Parent
    // You can touch child's property safety
    protected override void Initialize()
    {
        
    }
}