using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    int _xPos, _yPos;
    public int Cost { get; private set; }

    [SerializeField] float _rangoScan;
    [SerializeField] List<Node> vecinos;
    [SerializeField] LayerMask _obstacle, _nodos;
    public bool tempNode = false;
    [SerializeField] bool checkForVecinos = false;



    void Start()
    {
        _nodoMasCercano = 100000;

        if (!tempNode)
        {
            var nodos = Physics.OverlapSphere(transform.position, _rangoScan, _nodos);

            foreach (var node in nodos)
            {
                if (InLOS(transform.position, node.transform.position) && node.name != name)
                {
                    vecinos.Add(node.GetComponent<Node>());
                }
            }
        }
        else
        {
            //ManagerParcial2.Instance.tempNode = this;
            //ManagerParcial2.Instance.PlayerEvent.FueraDeFOV += EjecutarTempNode;
        }
    }

    private void Update()
    {
        if (tempNode)
        {
            //if (ManagerParcial2.Instance.PlayerEvent.meVen.Count > 0 && checkForVecinos == false)
            //{
            //    var nodos = Physics.OverlapSphere(transform.position, _rangoScan, _nodos);

            //    foreach (var node in nodos)
            //    {
            //        //print(node.name);
            //        if (InLOS(transform.position, node.transform.position) && node.name != name)
            //        {
            //            vecinos.Add(node.GetComponent<Node>());
            //        }
            //    }
            //    checkForVecinos = true;
            //    Quieto();
            //}
            //else if (ManagerParcial2.Instance.PlayerEvent.meVen.Count <= 0)
            //{
            //    checkForVecinos = false;
            //    vecinos.Clear();
            //}

            #region Testing
            //if (Input.GetKey(KeyCode.Space) && checkForVecinos == false)
            //{
            //    var nodos = Physics.OverlapSphere(transform.position, _rangoScan, _nodos);

            //    foreach (var node in nodos)
            //    {
            //        //print(node.name);
            //        if (InLOS(transform.position, node.transform.position) && node.name != name)
            //        {
            //            vecinos.Add(node.GetComponent<Node>());
            //        }
            //    }
            //    checkForVecinos = true;
            //    Quieto();
            //}
            //else if (!Input.GetKey(KeyCode.Space))
            //{
            //    checkForVecinos = false;
            //    vecinos.Clear();
            //} 
            #endregion
        }
    }

    public float _nodoMasCercano = 100000;
    Node _nodoMasCercanoNode;
    public void EjecutarTempNode()
    {
        if (_nodoMasCercanoNode != null)
        {
            _nodoMasCercanoNode.vecinos.Remove(this);
        }

        vecinos.Clear();

        var nodos = Physics.OverlapSphere(transform.position, _rangoScan, _nodos);

        for (int i = 0; i < nodos.Length; i++)
        {
            if (InLOS(transform.position, nodos[i].transform.position) && nodos[i].name != name)
            {
                vecinos.Add(nodos[i].GetComponent<Node>());
            }
        }
        for (int i = 0; i < vecinos.Count; i++)
        {
            if (i == 0)
            {
                _nodoMasCercanoNode = vecinos[i];
            }
            if (Vector3.Distance(_nodoMasCercanoNode.transform.position, transform.position) > Vector3.Distance(vecinos[i].transform.position, transform.position))
            {
                _nodoMasCercanoNode = vecinos[i];
            }
        }

        vecinos.Clear();

        vecinos.Add(_nodoMasCercanoNode.GetComponent<Node>());

        _nodoMasCercanoNode.vecinos.Add(this);

        //Quieto();

        #region Etc
        //if (checkForVecinos == false)
        //{
        //    var nodos = Physics.OverlapSphere(transform.position, _rangoScan, _nodos);

        //    foreach (var node in nodos)
        //    {
        //        //print(node.name);
        //        if (InLOS(transform.position, node.transform.position) && node.name != name)
        //        {
        //            vecinos.Add(node.GetComponent<Node>());
        //        }
        //    }
        //    checkForVecinos = true;
        //    Quieto();
        //}
        //else if (ManagerParcial2.Instance.PlayerEvent.meVen.Count <= 0)
        //{
        //    checkForVecinos = false;
        //    vecinos.Clear();
        //}
        #endregion

    }

    //public void Quieto()
    //{
    //    //print("inicio quieto");
    //    ManagerParcial2.Instance.WhiteGhost.CallingAvengers();
    //    ManagerParcial2.Instance.RedGhost.CallingAvengers();
    //    ManagerParcial2.Instance.BlueGhost.CallingAvengers();
    //    ManagerParcial2.Instance.YellowGhost.CallingAvengers();
    //    //print("fin quieto");
    //}

    protected bool InLOS(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;

        return !Physics.Raycast(start, dir.normalized, dir.magnitude, _obstacle);
    }

    public void Initialize(int xPos, int yPos)
    {
        _xPos = xPos;
        _yPos = yPos;
        Cost = 1;
    }

    public List<Node> GetNeighbors
    {
        get
        {
            if (vecinos.Count > 0)
            {
                return vecinos;
            }
            else
            {
                return default;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, _rangoScan);
    }



}
