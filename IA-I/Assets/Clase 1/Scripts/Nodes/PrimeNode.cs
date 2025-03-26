using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrimeNode : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    //public virtual void Test()
    //{
    //    print("test " + gameObject.name);
    //}

    public abstract void Test();

    public abstract void Execute(Caitlyn npc);
}
