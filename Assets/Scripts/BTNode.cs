using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public abstract class BTNode {
    /// <summary>
    /// 节点可进性检测
    /// </summary>
    public BTPrecondition precondition;
    /// <summary>
    /// 节点冷却
    /// </summary>
    public float interval = 1;
    /// <summary>
    /// 节点激活状态
    /// </summary>
    public bool isActivated;
    public BTContext btContext;
    public List<BTNode> children;

    private float lastTimeEvaluated = 0;

    // 节点可以不需要进场条件
    // 需要响应的Node怎么办？
    public BTNode(BTPrecondition precondition = null) {
        this.precondition = precondition;
    }

    public virtual void Activate(BTContext btContext) {
        if (isActivated) {
            return;
        }

        this.btContext = btContext;
        this.isActivated = true;
    }

    public bool Evaluate() {
        return isActivated && CheckCoolDown() &&
               (precondition == null || precondition.Check()) && DoEvaluate();
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
