using UnityEngine;

namespace BehaviourTree.Test {
    public class GameManager : MonoBehaviour {
        private Transform _orcTrans;
        private Transform _goblinTrans;

        void Update() {
            int mouseButton = -1;
            if (Input.GetMouseButtonDown(0)) {
                mouseButton = 0;
            } else if (Input.GetMouseButtonDown(1)) {
                mouseButton = 1;
            }

            if (mouseButton != -1) {
                Vector3 mousePosition = GetMousePositionInWorld();

                if (mouseButton == 0) {
                    _goblinTrans = _goblinTrans == null
                        ? ((GameObject) Instantiate(Resources.Load("Goblin"))).transform
                        : _goblinTrans;
                    _goblinTrans.name = "Goblin";
                    _goblinTrans.transform.position = mousePosition;
                } else if (mouseButton == 1) {
                    _orcTrans = _orcTrans == null
                        ? ((GameObject) Instantiate(Resources.Load("Orc"))).transform
                        : _orcTrans;
                    _orcTrans.name = "Orc";
                    _orcTrans.transform.position = mousePosition;
                }
            }
        }

        private Vector3 GetMousePositionInWorld () {
            float distanceToCamera = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 mousePosOnScreen = Input.mousePosition;
            Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePosOnScreen.x, mousePosOnScreen.y, distanceToCamera));
            return mousePosInWorld;
        }
    }
}