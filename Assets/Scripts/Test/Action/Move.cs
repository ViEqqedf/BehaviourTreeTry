using BehaviourTree;
using BehaviourTree.Nodes;
using UnityEngine;

public class Move : BTAction {
    private string destinationDataName;
    private Vector3 destination;
    private float tolerance = 0.01f;
    private float speed;
    private Transform trans;

    public Move(string destinationDataName, float speed) : base("Move") {
        this.destinationDataName = destinationDataName;
        this.speed = speed;
    }

    public override void Activate (BTContext context) {
        base.Activate(context);

        trans = context.transform;
    }

    protected override BTResult Execute () {
        UpdateDestination();
        UpdateFaceDirection();

        if (CheckArrived()) {
            return BTResult.Ended;
        }
        MoveToDestination();
        return BTResult.Running;
    }

    private void UpdateDestination () {
        destination = btContext.GetData<Vector3>(destinationDataName);
    }

    private void UpdateFaceDirection () {
        Vector3 offset = destination - trans.position;
        trans.localEulerAngles = offset.x >= 0 ? new Vector3(0, 180, 0) : Vector3.zero;
    }

    private bool CheckArrived () {
        Vector3 offset = destination - trans.position;
        return offset.sqrMagnitude < tolerance * tolerance;
    }

    private void MoveToDestination () {
        Vector3 direction = (destination - trans.position).normalized;
        trans.position += (direction * speed * Time.deltaTime);
    }
}