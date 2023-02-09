using UnityEngine;

namespace ITintouch.Components
{
    [ExecuteAlways]
    public class ChildSwitch : MonoBehaviour
    {
        public bool activateChild = true;
        
        private void Update()
        {
            if (transform.childCount == 0) return;
            
            var child = transform.GetChild(0);
            if (activateChild != child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(activateChild);
            }
        }
    }
}
