/********************************************
 * Title    : Score Panel Controller 
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace ifland.main.minigame
{
    /// <summary>
    /// 미니게임 점수 패널 오브젝트  제어 클래스 
    /// </summary>
    public class ScorePanelContorller : MonoBehaviour
    {
        public Button btn;
        public TMP_Text text;
        public GameObject panel;
        private void Awake()
        {
            btn = GetComponentInChildren<Button>();
            btn.onClick.AddListener(ClickBtn); //버튼 이벤트 등록
            panel = this.gameObject;
            panel.SetActive(false);
        }

        private void OnEnable()
        {
            text.text = string.Format("{0:#,###}", Utils.Score); //text 문자열 형식 지정 
        }

        /// <summary>
        /// 버튼 클릭시 실행 함수
        /// </summary>
        public void ClickBtn()
        {
            panel.SetActive(false);
        }
    }
}