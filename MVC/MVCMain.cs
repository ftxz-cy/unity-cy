using UnityEngine;
namespace MVC.core {
    public class MVCMain : MonoBehaviour {
        
        void Awake(){
            AudioManager.init(gameObject);
        }
    }
}