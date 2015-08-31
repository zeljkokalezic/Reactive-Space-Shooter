using UnityEngine;
using System.Collections;
using Zenject;

public class Damageable : MonoBehaviour
{
    public class Factory : ComponentFactory<IDamageable, Damageable>
    {
    }

    [Inject]
    public IDamageable Model;    
}
