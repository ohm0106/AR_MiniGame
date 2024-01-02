/********************************************
 * Title    : Number choose Mini Game Object Manager
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ifland.main.minigame;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


namespace ifland.main.minigame.NUMBERCHOOSE
{
    /// <summary>
    /// 미니게임 NumberScreen 화면 컨트롤러 클래스 
    /// </summary>
    public class NumberScreenController : MonoBehaviour, IScreenController
    {
        public GameObject btnGrid; //Grid Layout Group Component를 가진 객체 
        public GameObject btnPrefab; // button prefab

        public TMP_Text scoreTxt; //점수 UI 
        public Image timeImg; // 시간 UI 
        private float time; // 시간 



        public GameObject scorePanel; //점수 Panel 

        private bool isFinish = false;

        public Animator anim;
        public TMP_Text countTxt;

        public UnityAction<bool> SendFinishEvent;
        public UnityAction<FeverType> FeverTypeEvent;
        public UnityAction<int> StartGameEvent;

  
        public void Awake()
        {
            scoreTxt.text = "0"; // text 초기화 
            anim = GetComponent<Animator>(); 
            countTxt.gameObject.SetActive(false); 

        }

        /// <summary>
        /// 스크린 상 GameObj 객체 생성 및 생성 된 오브젝트 설정 함수 
        /// </summary>
        /// <returns></returns>
        public NumberBtn CreatBtnObj(int id, int num, NumberChooseObjectManager objManager)
        {
            GameObject btn = Instantiate(btnPrefab, this.btnGrid.transform); // Screen 의 자식으로 오브젝트 생성
            NumberBtn numberBtn = btn.AddComponent<NumberBtn>(); //monobehavior의 자식 스크립트는 new 생성자를 사용할 수 없음.
            numberBtn.ID = id; 
            numberBtn.Obj = btn; 
            numberBtn.Number = num;  
            numberBtn.objManager = objManager; 
            Debug.Log(">>ScreenController :: Create Btn Object / id : " + id + "/num :" + num + "/objManager :" + objManager);
            return numberBtn;
        }

        /// <summary>
        /// 점수 UI 및 점수 제어 함수
        /// </summary>
        /// <param name="num"></param>
        /// <param name="isCollect">true : Score + num / false: Score - num </param>
        public void CalculateScore(int num, bool isCollect)
        {
            if (isCollect)
            {
                Utils.Score += num;
            }
            else
            {
                Utils.Score -= num;
                if (Utils.Score < 0)
                    Utils.Score = 0;
            }
            scoreTxt.text = string.Format("{0:#,###}", Utils.Score);
        }

       /// <summary>
       /// 리셋 함수 
       /// </summary>
        public void Reset()
        {
            scoreTxt.text = "0";
            timeImg.fillAmount = 0;
            ControlActiveObject(countTxt.gameObject,false);
            anim.SetBool("isFever", false);
        }

        /// <summary>
        /// 게임 제한 시간 동안 UI 및 게임 동작 제어 코루틴 
        /// </summary>
        /// <param name="limitTime"></param>
        /// <returns></returns>
        public IEnumerator CheckTime(int limitTime)
        {
            Debug.Log("ScreenController :: CheckTime Corutine Start -----------------" + isFinish);
            time = 0;
            isFinish = false;
            while (time < limitTime)
            {
                time++;
                //시간 별 타이머 ui 처리 
                timeImg.fillAmount = (limitTime - time )/ limitTime;

                //Fever Time Start
                if (time == limitTime * 0.5)
                    FeverTypeEvent.Invoke(FeverType.TIMEFEVER);
                // Fever Time Finish
                else if (time == limitTime * 0.55)
                    FeverTypeEvent.Invoke(FeverType.NONE);

                yield return new WaitForSeconds(1f);
                Debug.Log("ScreenController :: CheckTime : Time = " + time);
            }
            isFinish = true;
            SendFinishEvent.Invoke(isFinish);
            Debug.Log("ScreenController :: CheckTime Corutine Finish -----------------" + isFinish);
        }




        /// <summary>
        /// 카운트 다운 코루틴
        /// </summary>
        /// <param name="countNumber"></param>
        /// <returns></returns>
        public IEnumerator GameStartDelay(int countNumber)
        {
            Debug.Log("ScreenController :: Game Delay Start -----------------");
            int time = 0;
            ControlActiveObject(countTxt.gameObject, true);
            while (time <= countNumber)
            {
                int countDown = (countNumber - time);
                string txt = countDown == 0 ? "START" : countDown.ToString();
                countTxt.text = txt;
                
                yield return new WaitForSeconds(1f);
                time++;
            }
            ControlActiveObject(countTxt.gameObject, false);
            StartGameEvent.Invoke(Utils.gameTime);
            Debug.Log("ScreenController :: Game Delay End-----------------");
        }



        /// <summary>
        /// GameObject Active Contol 함수 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isActive"></param>
        public void ControlActiveObject(GameObject obj, bool isActive)
        {
            if (obj.activeSelf != isActive)
                obj.SetActive(isActive);
        }


        /// <summary>
        /// 스코어 팝업 패널 Active control 함수 
        /// </summary>
        /// <param name="isActive"></param>
        public void ControlActivePanel(bool isActive)
        {
            if(scorePanel.activeSelf != isActive)
                scorePanel.SetActive(isActive);
        }
    }
}
