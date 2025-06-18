using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerP2 : FOV_Target
{
    private void Awake()
    {
        ManagerParcial2.Instance.Player = this;
    }
    protected override void Start()
    {
        base.Start();
        ManagerParcial2.Instance.Player = this;
    }


    void Update()
    {

    }
}
