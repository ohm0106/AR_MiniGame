using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ifland.main.minigame
{
    public class ButtonController : MonoBehaviour
    {
        public PirateRulletObjectManager mObjManager;
        private void Awake()
        {

        }

        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }

        /// <summary>
        /// 버튼 클릭 이벤트 함수 
        /// </summary>
        public void CheckClick()
        {
            mObjManager.mClickEvent.Invoke(this.gameObject);
            this.gameObject.SetActive(false);
            Debug.Log(">>ButtonController :: " + this.gameObject.activeSelf + "" + this.gameObject.name);
        }
    }
}
