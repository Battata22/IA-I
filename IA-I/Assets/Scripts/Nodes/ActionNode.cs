using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : PrimeNode
{
    public TypeQuestion actionType;

    public enum TypeQuestion
    {
        Patrullar, Perseguir, Refuerzos, Arrestar
    }

    public override void Execute(Caitlyn npc)
    {
        switch (actionType)
        {
            case TypeQuestion.Patrullar:
                print("Avance ciudadano marica");
                break;
            case TypeQuestion.Perseguir:
                var dir = npc._viScript.transform.position - npc.transform.position;
                npc.transform.position += dir.normalized * Time.deltaTime * npc._speed;
                print("TestChase");
                break;
            case TypeQuestion.Refuerzos:
                print("conche tu madre, manden refuezos");
                break;
            case TypeQuestion.Arrestar:
                print("arrestado");
                break;
        }
    }
    public override void Test()
    {
        print("test " + gameObject.name);
    }
}
