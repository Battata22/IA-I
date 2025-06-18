using UnityEngine;

public class Ghostly : FOV_Agent
{
    Pathfinding _path;
    [SerializeField] FOV_Target _player;

    protected override void Start()
    {
        base.Start();
        _player = ManagerParcial2.Instance.Player;
        _path = new Pathfinding();
    }


    protected override void Update()
    {
        //base.Update();

        if (inFOV(_player.transform.position))
        {
            print(gameObject.name + " ve al Player");
        }
    }
}
