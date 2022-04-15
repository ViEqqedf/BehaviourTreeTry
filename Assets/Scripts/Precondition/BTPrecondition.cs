using BehaviourTree.Nodes;

namespace BehaviourTree.Precondition {
    public class BTPrecondition : BTNode {
        public virtual bool Check() {
            return true;
        }

        public override BTResult Tick() {
            return Check() ? BTResult.Ended : BTResult.Running;
        }
    }
}