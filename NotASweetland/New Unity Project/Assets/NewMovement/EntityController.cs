using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all entities
public abstract class EntityController : ScriptableObject
{
    public Character character { get; set; }

    public abstract void EntityInit();
    public abstract void EntityUpdate();
    public abstract void EntityFixedUpdate();

}
