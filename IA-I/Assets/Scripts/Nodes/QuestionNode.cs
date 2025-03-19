using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : PrimeNode
{
    public PrimeNode trueNode;
    public PrimeNode falseNode;

    public TypeQuestion questionType;

    public enum TypeQuestion
    {
        Stealing, Distance, Armed
    }

    public override void Test()
    {
        print("test " + gameObject.name);
    }

    public override void Execute(Caitlyn npc)
    {
        switch(questionType)
        {
            case TypeQuestion.Stealing:
                if(npc._viScript.isStealing == true)
                {
                    trueNode.Execute(npc);
                }
                else
                {
                    falseNode.Execute(npc);
                }
                break;

            case TypeQuestion.Distance:
                if (Vector3.Distance(npc._viScript.transform.position, npc.transform.position) < 2)
                {
                    trueNode.Execute(npc);
                }
                else
                {
                    falseNode.Execute(npc);
                }
                break;

            case TypeQuestion.Armed:
                if (npc._viScript.isArmed == true)
                {
                    trueNode.Execute(npc);
                }
                else
                {
                    falseNode.Execute(npc);
                }
                break;
        }
    }


}
