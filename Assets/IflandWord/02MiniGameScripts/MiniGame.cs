using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ifland.main
{

    public class MiniGame : MonoBehaviour
    {
        public MiniGameName name;

        public GameType type;

        public GameObject obj;

        public Transform point;


        private void OnEnable()
        {
            obj = this.gameObject;
        }

       

    }

}

