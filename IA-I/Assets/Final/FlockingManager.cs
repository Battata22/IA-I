using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{

    [SerializeField, Range(0f, 1f)] public float _separationForce;
    [SerializeField, Range(0f, 1f)] public float _cohesionForce;

    [SerializeField] public float _radioAllignment;
    [SerializeField] public float _radioSeparation;

    public List<MinionBehaivour> _myCelesteTeammates = new();

    public List<MinionBehaivour> _myNaranjaTeammates = new();


    #region Singleton
    public static FlockingManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

}

public interface IBoidFinal
{
    public Transform GetTransform();

    public GameObject GetGameObject();
}
