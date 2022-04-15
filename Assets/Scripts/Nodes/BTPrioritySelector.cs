using BehaviourTree.Precondition;

namespace BehaviourTree.Nodes {
    public class BTPrioritySelector : BTNode {
        private BTNode curActiveChild;

        public BTPrioritySelector(string nodeName, BTPrecondition precondition = null) :
            base(nodeName, precondition) {}

        protected override bool DoEvaluate() {
            // 选择第一个可以进入的节点
            for (int i = 0, count = children.Count; i < count; i++) {
            	BTNode curChild = children[i];
            	if (curChild.Evaluate()) {
                    // 找到新的可进入的节点
            		if (curActiveChild != null && curActiveChild != curChild) {
                        // 清理上一次Tick的节点
            			curActiveChild.Clean();
            		}
            		this.curActiveChild = curChild;
            		return true;
            	}
            }

            // 没有节点可以进入
            if (curActiveChild != null) {
                // 清理上一次Tick的节点
            	curActiveChild.Clean();
            	curActiveChild = null;
            }

            return false;
        }

        public override BTResult Tick() {
            if (curActiveChild == null) {
                return BTResult.Ended;
            }

            BTResult result = curActiveChild.Tick();
            if (result != BTResult.Running) {
                curActiveChild.Clean();
                curActiveChild = null;
            }

            return result;
        }

        public override void Clean() {
            if (curActiveChild != null) {
                curActiveChild.Clean();
                curActiveChild = null;
            }
        }
    }
}