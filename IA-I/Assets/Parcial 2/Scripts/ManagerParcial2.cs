using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerParcial2 : MonoBehaviour
{
    public static ManagerParcial2 Instance;

    public FOV_Target Player;
    public PlayerP2 PlayerEvent;
    public Node nodoTest;

    public bool FSM = false;

    public Ghostly WhiteGhost;
    public Ghostly RedGhost;
    public Ghostly BlueGhost;
    public Ghostly YellowGhost;

    public List<Node> WhiteZone;
    public List<Node> BlueZone;
    public List<Node> RedZone;
    public List<Node> YellowZone;

    public Node tempNode;

    public Toggle FSMCheckBox;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            SceneManager.LoadScene(0);
        }
    }

}
