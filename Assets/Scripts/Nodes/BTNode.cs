using System.Collections.Generic;
using BehaviourTree;
using BehaviourTree.Nodes;
using BehaviourTree.Precondition;
using UnityEngine;

public abstract class BTNode {
    /// <summary>
    /// 节点可进性检测
    /// </summary>
    public BTPrecondition precondition;
    /// <summary>
    /// 节点冷却
    /// </summary>
    public float interval = 0;
    /// <summary>
    /// 节点激活状态
    /// </summary>
    public bool isActivated;
    public string nodeName;
    public BTContext btContext;
    public List<BTNode> children;

    private float lastTimeEvaluated = 0;

    // 节点可以不需要进场条件
    // 需要响应的Node怎么办？
    public BTNode(string nodeName = "", BTPrecondition precondition = null) {
        this.nodeName = nodeName;
        this.precondition = precondition;
    }

    public virtual void Activate(BTContext context) {
        if (isActivated) {
            return;
        }

        if (precondition != null) {
            precondition.Activate(context);
        }
        if (children != null) {
            for (int i = 0, count = children.Count; i < count; i++) {
                children[i].Activate(context);
            }
        }

        this.btContext = context;
        this.isActivated = true;
    }

    public bool Evaluate() {
        bool coolDownComplete = CheckCoolDown();
        bool customCheck = DoEvaluate();
        bool preconditionCheck = precondition == null || precondition.Check();
        return isActivated && coolDownComplete && preconditionCheck && customCheck;
    }

    protected virtual bool DoEvaluate() { return true; }

    public virtual BTResult Tick() { return BTResult.Ended; }

    public virtual void AddChild(BTNode node) {
        if (children == null) {
            children = new List<BTNode>();
        }

        if (node != null) {
            children.Add(node);
        }
    }

    public virtual void RemoveChild(BTNode node) {
        if (node != null && children.Contains(node)) {
            children.Remove(node);
        }
    }

    // 需不需要让Clean在节点生命周期结束时自动调用？
    public virtual void Clean() { }

    // 为什么要设置节点冷却时间？为了避免边界情况下的两个阶段决策反复横跳
    private bool CheckCoolDown() {
        float curTime = Time.time;
        if (curTime - lastTimeEvaluated > interval) {
            lastTimeEvaluated = curTime;
            return true;
        }

        return false;
    }
}
