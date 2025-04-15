using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV_Agent : FOV_Target
{

    [SerializeField] List<FOV_Target> _otherAgents = new();

    [SerializeField] LayerMask _obstacle;

    [SerializeField, Range(5, 360)] float _viewAngle;
    [SerializeField, Range(0.5f, 15)] float _viewRange;

    #region In Field Of View
    //In Field Of View
    bool inFOV(Vector3 endPos)
    {
        Vector3 dir = endPos - transform.position;

        if (dir.magnitude > _viewRange) return false;

        if (Vector3.Angle(transform.forward, dir) > _viewAngle / 2) return false;

        if (!InLOS(transform.position, endPos)) return false;


        return true;
    } 
    #endregion

    #region Line Of Sight
    //In Line Of Sight
    bool InLOS(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;

        return !Physics.Raycast(start, dir.normalized, dir.magnitude, _obstacle);
    } 
    #endregion


    protected override void Start()
    {
        base.Start();
        ChangeColor(Color.gray);
    }

    
    void Update()
    {
        foreach(var agent in _otherAgents)
        {
            //Manera Resumida Pregunta ? Condicion para True : Condicion para False
            agent.ChangeColor(inFOV(agent.transform.position) ? Color.red : Color.white);

            //Manera Tradicional
            //if (inFOV(agent.transform.position))
            //{
            //    agent.ChangeColor(Color.black);
            //}
            //else
            //{
            //    agent.ChangeColor(Color.white);
            //}
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _viewRange);

        Gizmos.color = Color.cyan;
        Vector3 dirA = GetAngleFromDir(_viewAngle / 2 + transform.eulerAngles.y);
        Vector3 dirB = GetAngleFromDir(-_viewAngle / 2 + transform.eulerAngles.y);
        Gizmos.DrawLine(transform.position, transform.position + dirA.normalized * _viewRange);
        Gizmos.DrawLine(transform.position, transform.position + dirB.normalized * _viewRange);
    }

    Vector3 GetAngleFromDir(float angleInDegrees) => new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

}
