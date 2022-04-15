using UnityEngine;
using BehaviourTree;
using BehaviourTree.Nodes;
using BehaviourTree.Precondition;

public abstract class FindTargetDestination : BTAction {
	protected string targetName;
	protected string destinationDataName;
	protected Transform trans;

	public FindTargetDestination(string targetName, string destinationDataName, BTPrecondition precondition = null) :
		base("FindTargetDestination", precondition) {
		this.targetName = targetName;
		this.destinationDataName = destinationDataName;
	}

	public override void Activate (BTContext context) {
		base.Activate(context);
		trans = context.transform;
	}

	protected Vector3 GetToTargetOffset () {
		GameObject targetGo = GameObject.Find(targetName);
		if (targetGo == null) {
			return Vector3.zero;
		}

		return targetGo.transform.position - trans.position;
	}

	protected bool checkTarget () {
		return GameObject.Find(targetName) != null;
	}
}

public class FindEscapeDestination : FindTargetDestination {
	private float safeDistance;

	public FindEscapeDestination (string targetName, string destinationDataName, float safeDistance,
		BTPrecondition precondition = null) : base (targetName, destinationDataName, precondition) {
		this.safeDistance = safeDistance;
	}

	protected override BTResult Execute () {
		if (!checkTarget()) {
			return BTResult.Ended;
		}

		Vector3 offset = GetToTargetOffset();

		if (offset.sqrMagnitude <= safeDistance * safeDistance) {
			Vector3 direction = -offset.normalized;
			Vector3 destination = safeDistance * direction * Random.Range(1.2f, 1.3f);
			btContext.SetData<Vector3>(destinationDataName, destination);
			return BTResult.Running;
		}

		return BTResult.Ended;
	}
}

public class FindToTargetDestination : FindTargetDestination {
	private float minDistance;

	public FindToTargetDestination (string targetName, string destinationDataName, float minDistance,
		BTPrecondition precondition = null) : base (targetName, destinationDataName, precondition) {
		this.minDistance = minDistance;
	}

	protected override BTResult Execute () {
		if (!checkTarget()) {
			return BTResult.Ended;
		}

		Vector3 offset = GetToTargetOffset();

		if (offset.sqrMagnitude >= minDistance * minDistance) {
			Vector3 direction = offset.normalized;
			Vector3 destination = trans.position + offset - minDistance * direction;
			btContext.SetData<Vector3>(destinationDataName, destination);
			return BTResult.Running;
		}
		return BTResult.Ended;
	}
}