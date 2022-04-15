using System;
using BehaviourTree.Nodes;
using Unity.VisualScripting;
using UnityEngine;

namespace BehaviourTree {
    public class BTTree : MonoBehaviour {
        protected BTNode root = null;

        [HideInInspector] public BTContext context;

        private void Awake() {
            Init();

            root.Activate(context);
        }

        private void Update() {
            if (root.Evaluate()) {
                root.Tick();
            }
        }

        private void OnDestroy() {
            Clean();
        }

        protected virtual void Init() {
            context = GetComponent<BTContext>();
            if (context == null) {
                context = gameObject.AddComponent<BTContext>();
            }

            // 默认生成一个优先选择节点
            root = new BTSequence("Root");
        }

        protected void Clean() {
            if (root != null) {
                root.Clean();
            }
        }
    }
}