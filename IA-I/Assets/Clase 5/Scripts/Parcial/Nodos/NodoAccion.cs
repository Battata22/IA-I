using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoAccion : PapaNodo
{

    public TypeQuestion actionType;

    public enum TypeQuestion
    {
        Flocking, LookForFood, Run
    }

    public override void Execute(BoidBehaivour boid)
    {
        switch (actionType)
        {
            case TypeQuestion.Flocking:
                //movimiento normal
                //Floking();
                boid.useFlocking = true;

                break;
            case TypeQuestion.LookForFood:
                //buscar comida con el fov y si hay hacer el arrive y comerla

                break;
            case TypeQuestion.Run:
                //chequear si esta el cazador y correr de el
                boid.useEvade = true;

                break;
        }
    }

    //void Floking()
    //{
    //    AddForce(Separation(GameManager.instance._myBoidsParcial, GameManager.instance._radioSeparation) * GameManager.instance._separationForce);
    //    AddForce(Allignment(GameManager.instance._myBoidsParcial, GameManager.instance._radioAllignment) * GameManager.instance._allignmentForce);
    //    AddForce(Cohesion(GameManager.instance._myBoidsParcial, GameManager.instance._radioAllignment) * GameManager.instance._cohesionForce);
    //}

    //Vector3 Separation(List<BoidBehaivour> myBoids, float radio)
    //{
    //    Vector3 desired = Vector3.zero;

    //    foreach (BoidBehaivour boid in myBoids)
    //    {
    //        var dir = boid.transform.position - transform.position;

    //        if (boid == this || dir.magnitude > radio) continue;

    //        desired -= dir;
    //    }

    //    if (desired == Vector3.zero) return Vector3.zero;

    //    return Seek(desired);
    //}

    //Vector3 Cohesion(List<BoidBehaivour> myBoids, float radio)
    //{
    //    Vector3 desired = transform.position;

    //    int count = 0;

    //    foreach (BoidBehaivour boid in myBoids)
    //    {
    //        if (boid == this || Vector3.Distance(transform.position, boid.transform.position) > radio) continue;

    //        count++;

    //        desired += boid.transform.position;
    //    }

    //    if (count == 0) return Vector3.zero;

    //    desired /= count;

    //    desired -= transform.position;

    //    return Seek(desired);
    //}

    //Vector3 Allignment(List<BoidBehaivour> myBoids, float radio)
    //{
    //    Vector3 desired = Vector3.zero;

    //    int count = 0;

    //    foreach (BoidBehaivour boid in myBoids)
    //    {
    //        if (boid == this) continue;

    //        if (Vector3.Distance(transform.position, boid.transform.position) <= radio)
    //        {
    //            count++;

    //            desired += boid.Velocity;
    //        }
    //    }

    //    if (count == 0) return Vector3.zero;

    //    desired /= count;

    //    #region Otra Manera
    //    //desired = desired.normalized;

    //    //desired *= _maxSpeed;

    //    //Vector3 steering = desired - _velocity;

    //    //steering = Vector3.ClampMagnitude(steering, _maxSpeed);

    //    //return steering; 
    //    #endregion

    //    return Seek(desired);
    //}
    //void AddForce(Vector3 dir)
    //{
    //    _velocity = Vector3.ClampMagnitude(_velocity + dir, _maxSpeed);
    //}

    //public Vector3 Seek(Vector3 desired)
    //{
    //    desired = desired.normalized;
    //    desired *= _maxSpeed;

    //    Vector3 steering = desired - _velocity;
    //    steering = Vector3.ClampMagnitude(steering, _maxSpeed);

    //    return new Vector3(steering.x, 0, steering.z);
    //}


    #region OG
    //public override void Execute(Caitlyn npc)
    //{
    //    switch (actionType)
    //    {
    //        case TypeQuestion.Patrullar:
    //            print("Avance ciudadano marica");
    //            break;
    //        case TypeQuestion.Perseguir:
    //            var dir = npc._viScript.transform.position - npc.transform.position;
    //            npc.transform.position += dir.normalized * Time.deltaTime * npc._speed;
    //            print("TestChase");
    //            break;
    //        case TypeQuestion.Refuerzos:
    //            print("conche tu madre, manden refuezos");
    //            break;
    //        case TypeQuestion.Arrestar:
    //            print("arrestado");
    //            break;
    //    }
    //} 
    #endregion

}
