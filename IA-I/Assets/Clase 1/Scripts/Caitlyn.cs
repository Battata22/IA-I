using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caitlyn : MonoBehaviour
{
    public Vi _viScript;
    [SerializeField, Range(0.5f, 5f)] public float _speed; 

    public PrimeNode _padreNode;

    //patrullar, pedir refuerzos, acercarce, arrestar
    void Update()
    {
        #region Comment
        //if (_viScript.isStealing == true)
        //{
        //    

        //    if (dir.magnitude < 2)
        //    {
        //        if (_viScript.isArmed == true)
        //        {
        //            print("conche tu madre, manden refuezos");
        //        }
        //        else
        //        {
        //            
        //        }
        //    }
        //    else
        //    {
        //        //normalized para que no acelere ni desacelere al estar mas lejos/cerca
        //        transform.position += dir.normalized * Time.deltaTime * _speed;
        //    }
        //}
        //else
        //{
        //    
        //} 
        #endregion

        if (Input.GetKey(KeyCode.Space))
        {
            _padreNode.Execute(this);
        }

    }

}
