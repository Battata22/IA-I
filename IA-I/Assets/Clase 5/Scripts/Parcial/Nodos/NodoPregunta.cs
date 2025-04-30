using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoPregunta : PapaNodo
{
    public PapaNodo trueNode;
    public PapaNodo falseNode;

    public TypeQuestion questionType;

    public enum TypeQuestion
    {
        isFood, isHunter, isBoid
    }

    public override void Execute(BoidBehaivour boid)
    {
    //    switch (questionType)
    //    {
    //        case TypeQuestion.isFood:
    //            if (boid._viScript.isStealing == true)
    //            {
    //                trueNode.Execute(boid);
    //            }
    //            else
    //            {
    //                falseNode.Execute(boid);
    //            }
    //            break;

    //        case TypeQuestion.Run:
    //            if (Vector3.Distance(boid._viScript.transform.position, boid.transform.position) < 2)
    //            {
    //                trueNode.Execute(boid);
    //            }
    //            else
    //            {
    //                falseNode.Execute(boid);
    //            }
    //            break;

    //        case TypeQuestion.Flocking:
    //            if (boid._viScript.isArmed == true)
    //            {
    //                trueNode.Execute(boid);
    //            }
    //            else
    //            {
    //                falseNode.Execute(boid, velocity, maxSpeed);
    //            }
    //            break;
    //    }
    }

    #region OG
    //public override void Execute(Caitlyn npc)
    //{
    //    switch (questionType)
    //    {
    //        case TypeQuestion.Stealing:
    //            if (npc._viScript.isStealing == true)
    //            {
    //                trueNode.Execute(npc);
    //            }
    //            else
    //            {
    //                falseNode.Execute(npc);
    //            }
    //            break;

    //        case TypeQuestion.Distance:
    //            if (Vector3.Distance(npc._viScript.transform.position, npc.transform.position) < 2)
    //            {
    //                trueNode.Execute(npc);
    //            }
    //            else
    //            {
    //                falseNode.Execute(npc);
    //            }
    //            break;

    //        case TypeQuestion.Armed:
    //            if (npc._viScript.isArmed == true)
    //            {
    //                trueNode.Execute(npc);
    //            }
    //            else
    //            {
    //                falseNode.Execute(npc);
    //            }
    //            break;
    //    }
    //} 
    #endregion

}
