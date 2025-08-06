using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseManager : MonoBehaviour
{
    public static event Action OnClick0Event;
    public static event Action OnClick1Event;
    public static event Action RefreshSearchNaranja;
    public static event Action RefreshSearchCeleste;

    public Node _tempNodeNaranja;
    public Node _tempNodeCeleste;

    [SerializeField] JefesBehaviour _jefeNaranja;
    [SerializeField] JefesBehaviour _jefeCeleste;

    Ray ray;
    RaycastHit hit;

    #region Singleton't
    public static MouseManager instance;

    private void Awake()
    {
        instance = this;
    } 
    #endregion

    void Start()
    {
        OnClick0Event += Click0;
        OnClick1Event += Click1;
    }


    void Update()
    {
        //Team Naranja
        if (Input.GetMouseButtonDown(0))
        {
            OnClick0Event();
            RefreshSearchNaranja();
            _jefeNaranja.ResetIndex();
        }

        //Team Celeste
        if (Input.GetMouseButtonDown(1))
        {
            OnClick1Event();
            RefreshSearchCeleste();
            _jefeCeleste.ResetIndex();
        }

        //--------------------------------------------

        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void Click0()
    {
        //Team Naranja

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.gameObject.layer == 18)
        {
            _tempNodeNaranja.transform.position = new Vector3(hit.point.x, 0, hit.point.z);

            _tempNodeNaranja.EjecutarTempNode();

            _jefeNaranja.GoToClick();
        }
    }

    void Click1()
    {
        //Team Celeste

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.gameObject.layer == 18)
        {
            _tempNodeCeleste.transform.position = new Vector3(hit.point.x, 0, hit.point.z);

            _tempNodeCeleste.EjecutarTempNode();

            _jefeCeleste.GoToClick();
        }
    }
}
