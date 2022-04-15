using System.Collections.Generic;
using BehaviourTree.Precondition;

namespace BehaviourTree.Nodes {
    public class BTParallelFlexible : BTNode {
        private List<bool> activeList = new List<bool>();

        public BTParallelFlexible(string nodeName, BTPrecondition precondition = null) :
            base(nodeName, precondition) {
        }

        protected override bool DoEvaluate() {
            int numActiveChildren = 0;

            for (int i = 0, count = children.Count; i < count; i++) {
                BTNode child = children[i];
                if (child.Evaluate()) {
                    activeList[i] = true;
                    numActiveChildren++;
                } else {
                    activeList[i] = false;
                }
            }

            return numActiveChildren > 0;
        }

        public override BTResult Tick() {
            int runningChildNum = 0;

            for (int i = 0, count = children.Count; i < count; i++) {
                bool active = activeList[i];
                if (active) {
                    BTResult result = children[i].Tick();
                    if (result == BTResult.Running) {
                        runningChildNum++;
                    }
                }
            }

            return runningChildNum == 0 ? BTResult.Ended : BTResult.Running;
        }

        public override void AddChild(BTNode node) {
            base.AddChild(node);

            // 新添加的节点默认情况下不运行
            activeList.Add(false);
        }

        public override void RemoveChild(BTNode node) {
            int index = children.IndexOf(node);
            activeList.RemoveAt(index);
            base.RemoveChild(node);
        }

        public override void Clean() {
            base.Clean();

            for (int i = 0, count = children.Count; i < count; i++) {
                children[i].Clean();
            }
        }
    }
}