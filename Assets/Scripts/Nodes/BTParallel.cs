using System.Collections.Generic;
using BehaviourTree.Precondition;

namespace BehaviourTree.Nodes {
    public class BTParallel : BTNode {
        public enum ParallelFunction {
            And = 1,	// 所有被循环的节点都停止时阶段结束
            Or = 2,		// 任一被循环的节点停止时阶段结束
        }

        protected List<BTResult> results;
        protected ParallelFunction func;

        public BTParallel(string nodeName, ParallelFunction func, BTPrecondition precondition = null) :
            base (nodeName, precondition) {
            results = new List<BTResult>();
            this.func = func;
        }

        protected override bool DoEvaluate() {
            foreach (BTNode child in children) {
                // 所有子节点都必须通过检查
                if (!child.Evaluate()) {
                    return false;
                }
            }
            return true;
        }

        public override BTResult Tick() {
            int endingResultCount = 0;

            for (int i = 0, count = children.Count; i < count; i++) {
                if (func == ParallelFunction.And) {
                    if (results[i] == BTResult.Running) {
                        results[i] = children[i].Tick();
                    }
                    if (results[i] != BTResult.Running) {
                        endingResultCount++;
                    }
                } else {
                    if (results[i] == BTResult.Running) {
                        results[i] = children[i].Tick();
                    }
                    if (results[i] != BTResult.Running) {
                        ResetResults();
                        return BTResult.Ended;
                    }
                }
            }

            // 仅对AND类型循环生效
            if (endingResultCount == children.Count) {
                ResetResults();
                return BTResult.Ended;
            }
            return BTResult.Running;
        }

        public override void Clean() {
            ResetResults();

            foreach (BTNode child in children) {
                child.Clean();
            }
        }

        public override void AddChild(BTNode aNode) {
            base.AddChild (aNode);
            results.Add(BTResult.Running);
        }

        public override void RemoveChild(BTNode aNode) {
            int index = children.IndexOf(aNode);
            results.RemoveAt(index);
            base.RemoveChild(aNode);
        }

        private void ResetResults() {
            for (int i = 0; i < results.Count; i++) {
                results[i] = BTResult.Running;
            }
        }
    }
}