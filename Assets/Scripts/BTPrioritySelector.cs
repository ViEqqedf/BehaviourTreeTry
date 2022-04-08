namespace BehaviourTree {
    public class BTPrioritySelector : BTNode {
        private BTNode curActiveChild;

        public BTPrioritySelector(BTPrecondition precondition) : base(precondition) {}

        protected override bool DoEvaluate() {
            // 选择第一个可以进入的节点
            for (int i = 0, count = children.Count; i < count; i++) {
                if (curActiveChild != null && !curActiveChild.Evaluate()) {
                    // 添加额外的判断？
                }
                BTNode curChild = children[i];
                if (curChild.Evaluate()) {
                    this.curActiveChild = curChild;
                    return true;
                }
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