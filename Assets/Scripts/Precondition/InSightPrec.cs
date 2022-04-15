using UnityEngine;

namespace BehaviourTree.Precondition {
    public class InSightPrec : BTPrecondition {
        private float sightLength;
        private string targetName;
        private Transform trans;

        public InSightPrec(float sightLength, string targetName){
            this.sightLength = sightLength;
            this.targetName = targetName;
        }

        public override void Activate(BTContext context) {
            base.Activate(context);
            trans = context.transform;
        }

        public override bool Check() {
            GameObject target = GameObject.Find(targetName);
            if (target == null) return false;

            float distance = Vector3.Distance(target.transform.position, trans.position);
            return distance <= sightLength;
        }
    }
}