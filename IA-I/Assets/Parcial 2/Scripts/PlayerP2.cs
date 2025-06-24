using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerP2 : FOV_Target
{

    //public event Action outFOV;
    public event Action FueraDeFOV;
    public bool activarEventoFueraDeFOV = false;
    [SerializeField] Node nodeTemp;
    public List<Ghostly> meVen;
    public bool inFOV = false;

    private void Awake()
    {
        ManagerParcial2.Instance.Player = this;
        ManagerParcial2.Instance.PlayerEvent = this;
        //outFOV += GenerarNodoTemporal;
        FueraDeFOV += MoveTemp;
    }

    protected override void Start()
    {
        base.Start();
    }


    void Update()
    {

        #region Testeo
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    outFOV();
        //}

        //if (!inFOV)
        //{
        //    MoveTemp();
        //}

        //if (!Input.GetKey(KeyCode.Space))
        //{
        //    MoveTemp();
        //}
        #endregion

        if (activarEventoFueraDeFOV)
        {
            FueraDeFOV();
            activarEventoFueraDeFOV = false;
        }

        //if (meVen.Count <= 0)
        //{
        //    MoveTemp();
        //}
    }

    void MoveTemp()
    {
        nodeTemp.transform.position = new Vector3(transform.position.x, 0.1512671f, transform.position.z);
    }

    //void GenerarNodoTemporal()
    //{
    //    var node = Instantiate(nodePrefab, new Vector3(transform.position.x, 0.1512671f, transform.position.z), Quaternion.identity);
    //    node.Temporal();
    //}

}
