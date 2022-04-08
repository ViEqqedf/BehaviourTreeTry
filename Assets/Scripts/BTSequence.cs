using UnityEngine;

namespace BehaviourTree {
    public class BTSequence : BTNode {
        private BTNode curActiveChild;

        public BTSequence(BTPrecondition precondition) : base (precondition) {}

        protected override bool DoEvaluate() {
            if (curActiveChild != null) {
                bool result = curActiveChild.Evaluate();
                if (!result) {
                    // 任意一个阶段的检查不通过，整个队列中止
                    curActiveChild.Clean();
                    curActiveChild = null;
                }

                return result;
            } else if (children.Count <= 0) {
                Debug.LogError("[ViE] 该队列节点没有子节点！");
                return false;
            }
            else {
                return children[0].Evaluate();
            }
        }

        public override BTResult Tick() {
            if (curActiveChild == null) {
                curActiveChild = children[0];
            }

            BTResult result = curActiveChild.Tick();
            if (result == BTResult.Ended) {
                int nextIndex = children.IndexOf(curActiveChild) + 1;
                curActiveChild.Clean();
                if (nextIndex < children.Count) {
                    curActiveChild = children[nextIndex];
                    result = BTResult.Running;
                } else {
                    result = BTResult.Ended;
                }
            }

            return result;
        }

        public override void Clean() {
            if (curActiveChild != null) {
                curActiveChild.Clean();
                curActiveChild = null;
            }

            for (int i = 0, count = children.Count; i < count; i++) {
                children[i].Clean();
            }
            children.Clear();
        }
    }
}