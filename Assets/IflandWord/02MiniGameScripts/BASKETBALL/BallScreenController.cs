/********************************************
 * Title    : Basket Ball Screen UI Controller 
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace ifland.main.minigame.BASKETBALL
{
    /// <summary>
    /// 미니게임 BasketBall 화면 컨트롤러 클래스 
    /// </summary>
    public class BallScreenController : MonoBehaviour, IScreenController
    {
        public TMP_Text countTxt;
        public GameObject scorePanel; //점수  Panel
        public TMP_Text scoreTxt; //점수 UI
        public Image guageImg;
        public TMP_Text timeTxt;
        private bool isFinish = false;

        public Animator anim;

        public UnityAction<bool> SendFinishEvent;
        public UnityAction<FeverType> FeverTypeEvent;
        public UnityAction<int> StartGameEvent;

        public bool isClickUP = false;
        public float gauge = 0f;


        public int stageNum = 1;
        private void Awake()
        {
            Reset();
            anim = GetComponent<Animator>();
        }


        /// <summary>
        /// 카운트 다운 코루틴
        /// </summary>
        /// <param name="countNumber">카운트 숫자 </param>
        /// <returns></returns>
        public IEnumerator GameStartDelay(int countNumber)
        {
            Utils.isExecuteMinigame = false;
            Debug.Log(">>BallScreenController :: GameStartDelay Start -----------------");
            int time = 0; // 시간 초기화 
            ControlActiveObject(countTxt.gameObject, true); // 카운트 UI 오브젝트 가시화
            countTxt.text = "STAGE " + stageNum; 

            yield return new WaitForSeconds(1f);
            while (time <= countNumber)
            {
                int countDown = (countNumber - time);
                string txt = countDown == 0 ? "START" : countDown.ToString();
                countTxt.text = txt;

                yield return new WaitForSeconds(1f);
                Debug.Log(">>BallScreenController :: GameStartDelay : Count Text \" " + txt + " \"");
                time ++;
            }
            ControlActiveObject(countTxt.gameObject, false);
            Debug.Log(">>BallScreenController :: GameStartDelay End-----------------");

            StartGameEvent.Invoke(Utils.gameTime);
        }

        /// <summary>
        /// 3d virtual 버튼 클릭 진행 시 bar image fillamount 제어 처리
        /// </summary>
        /// <param name="maxGauge">최대 Guage </param>
        /// <returns></returns>
        public IEnumerator ClickVirtualButton(float maxGauge)
        {
            Debug.Log(">>BallScreenController :: ClickButton Start-----------------");
            isClickUP = true;
            gauge = 0f;
            bool isPlus = true;
            float num = 1f;
            
            while (isClickUP)
            {
                gauge = Mathf.Clamp(gauge, 0, maxGauge); // guage 는 0 ~ 100 사이의 값을 가진다.

                if (gauge == 100)
                    num = -3f;
                else if (gauge == 0)
                    num = 3f;

                gauge += num;

                guageImg.fillAmount = gauge / maxGauge; 
                
                yield return new WaitForSeconds(0.01f);
                Debug.Log(">>BallScreenController :: ClickButton gauge : " + gauge);
            }
            Debug.Log(">>BallScreenController :: ClickButton End-----------------");
        }

        /// <summary>
        /// ClickButton 코루틴 stop 시 실행하는 함수 
        /// </summary>
        /// <returns>버튼 클릭 정도에 따른 게이지 수치</returns>
        public float StopClickButtonCorutine()
        {
            isClickUP = false;
            guageImg.fillAmount = 0f;
            return gauge;
        }


        /// <summary>
        /// 게임 제한 시간 동안 UI 및 게임 동작 제어 코루틴 
        /// </summary>
        /// <param name="limitTime"></param>
        /// <returns></returns>
        public IEnumerator CheckTime(int limitTime)
        {
            Debug.Log(">>BallScreenController :: CheckTime Corutine Start -----------------" + isFinish);
            int time = 0;
            int feverTime = (limitTime/10) * 5; //FeverTIme은 시작 후 반이 지날 때 Start
            isFinish = false;
            Utils.isExecuteMinigame = true;

            while (time < limitTime)
            {
                time ++;
                //시간 별 타이머 ui 처리 
                timeTxt.text = string.Format("{0:00}", time);

                //Fever Time Start
                if (time == feverTime)
                {
                    FeverTypeEvent.Invoke(FeverType.TIMEFEVER);
                    Debug.Log(">>BallScreenController :: CheckTime : Fever!!!");
                }
                    
                // Fever Time Finish
                else if (time >= limitTime)
                    FeverTypeEvent.Invoke(FeverType.NONE);

                yield return new WaitForSeconds(1f);
                Debug.Log(">>BallScreenController :: CheckTime : Time = " + time);
            }
            isFinish = true;
            SendFinishEvent.Invoke(isFinish);
            Debug.Log(">>BallScreenController :: CheckTime Corutine Finish -----------------" + isFinish);

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
                Debug.Log(">>BallScreenController :: CalculateScore isCollect " + Utils.Score);
            }
            else
            {
                Utils.Score -= num;
                if (Utils.Score < 0)
                    Utils.Score = 0;
                Debug.Log(">>BallScreenController :: CalculateScore is not Collect " + Utils.Score);
            }
            scoreTxt.text = string.Format("{0:#,###}", Utils.Score); //점수 text 문자열 형식 지정
        }

        /// <summary>
        /// UI 리셋 함수 
        /// </summary>
        public void Reset()
        {
            scoreTxt.text = "0";
            timeTxt.text = "00";
            stageNum = 1;
            Debug.Log(">>BallScreenController :: Reset / scoreText " + scoreTxt.text + "/timeText " + timeTxt.text + "/stageNum " + stageNum);
            ControlActiveObject(countTxt.gameObject, false);
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
            Debug.Log(">>BallScreenController :: ControlActiveObject obj active self " + obj.activeSelf);
        }


        /// <summary>
        /// 스코어 팝업 패널 Active control 함수 
        /// </summary>
        /// <param name="isActive"></param>
        public void ControlActivePanel(bool isActive)
        {
            if (scorePanel.activeSelf != isActive)
                scorePanel.SetActive(isActive);
            Debug.Log(">>BallScreenController :: ControlActivePanel scorePanel active self " + scorePanel.activeSelf);
        }
    }
}

