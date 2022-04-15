using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree {
    public class BTContext : MonoBehaviour {
        private Dictionary<string, object> dtoPool = new Dictionary<string, object>();

        public T GetData<T>(string dataName) {
            if (dtoPool.TryGetValue(dataName, out object value)) {
                if (value.GetType() == typeof(T)) {
                    return (T) value;
                }
            }

            Debug.LogError($"[ViE] 找不到{dataName}对应的{typeof(T)}类型的值");
            return (T) value;
        }

        public void SetData<T>(string dataName, T data) {
            if (dtoPool.ContainsKey(dataName)) {
                dtoPool[dataName] = data;
            } else {
                dtoPool.Add(dataName, data);
            }
        }
    }
}