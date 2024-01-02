/********************************************
 * Title    : BasketBall Mini Game Object Manager
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace ifland.main.minigame.BASKETBALL
{
    /// <summary>
    /// 농구공 게임 상태에 따른 게임 상호작용 클래스 
    /// </summary>
    public class BasketBallObjectManager : Singleton<BasketBallObjectManager>, IGameObjectManager
    {

        private MiniGame miniGameType;

        private ScoreText mDamageText = null;
     
        private Coroutine mTimeCorutine = null;
        private Coroutine mClickCorutine = null;


        private GameController mGameController = null;
        private BallScreenController mBallScreenController = null;
/*        private IScreenController ScreenHandler = null;*/
        private ClickManager mClickCastCollider = null;
        private BasketBallController mBasketBallController = null;

        public UnityAction<bool , int> GetScoreEvent;
        public UnityAction<FeverType, int> SendFeverTypeEvent;
        private FeverType curFeverType;

        private int stage;
        private readonly int stageStandardNum = 3000;

        protected override void Awake()
        {
            OnINIT();
        }

        protected override void Update()
        {
            base.Update();

        }

        /// <summary>
        /// 게임 score 추가 함수 
        /// </summary>
        /// <param name="isGoalIn"></param>
        private void GetScore(bool isGoalIn, int plusScore)
        {
            int score = curFeverType == FeverType.NONE ? plusScore : plusScore * 2;
            mDamageText.SetScoreText("+", score);
            mBallScreenController.CalculateScore(score, isGoalIn);
        }


        /// <summary>
        /// Mini Game 오브젝트 3D virtual 버튼 클릭 Down, UP시 Event 전달 받는 함수 
        /// </summary>
        /// <param name="isClickDown"> </param>
        private void ClickBtn(bool isClickDown)
        {
            if (isClickDown)
            {
                if (mClickCorutine != null)
                {
                    StopCoroutine(mClickCorutine);
                    mClickCorutine = null;
                }

                mClickCorutine = StartCoroutine(mBallScreenController.ClickVirtualButton(100));
            }

            else
            {
               float gauge =  mBallScreenController.StopClickButtonCorutine();
               float force = gauge / 20f; 
               mBasketBallController.ControlForce(force);
                if (mClickCorutine != null)
                {
                    StopCoroutine(mClickCorutine);
                    mClickCorutine = null;
                }
            }
        }



        /// <summary>
        /// Fever 타임 시작 시 Fever 상태 이벤트 구독 함수 
        /// </summary>
        /// <param name="_feverType"> </param>
        public void GetFeverType(FeverType _feverType)
        {
            Debug.Log(">>BasketBallObjectManager :: Get FeverType is " + _feverType);
            curFeverType = _feverType;
            SendFeverTypeEvent.Invoke(curFeverType, mBallScreenController.stageNum);
            FeverSetting(_feverType);
        }


        /// <summary>
        /// Fever time이 되었을 때 Setting 함수 
        /// </summary>
        /// <param name="isFever"> </param>
        public void FeverSetting(FeverType _feverType)
        {
            bool isFever = _feverType != FeverType.NONE ? true : false;
            mBallScreenController.anim.SetBool("isFever", isFever);
        }


        /// <summary>
        /// 게임 시작 버튼 
        /// </summary>
        public void ClickStartBtn()
        {
            mGameController.SetGameStart(); //게임 시작 
            StartSetting();
        }


        /// <summary>
        /// 게임 종료 사인이 있을 시 실행되는 함수 
        /// * 점수의 기준치가 높다면 다음 스테이지 실행 
        /// * 점수의 기준치가 낮다면 종료 
        /// [점수의 기준치] = 현재 stage * stageStandardNum
        /// </summary>
        /// <param name="isFinish"></param>
        private void GetFinishSign(bool isFinish)
        {
            
            if (isFinish && (Utils.Score <= stageStandardNum * mBallScreenController.stageNum) )
            {
                FinishGame();
            }
            else
            {
                mBallScreenController.stageNum++;
 
                if (mTimeCorutine != null)
                {
                    StopCoroutine(mTimeCorutine);
                    mTimeCorutine = null;
                }

                /*   mTimeCorutine = StartCoroutine(TutorialCO()); */
                mTimeCorutine = StartCoroutine(mBallScreenController.GameStartDelay(Utils.delay));
            }
        }


        #region IGameObjectManager

        public void OnINIT()
        {
            if (mClickCastCollider == null)
            {
                mClickCastCollider = FindObjectOfType<ClickManager>();
            }
            mClickCastCollider.RayCastingClickEvent += ClickBtn;
            miniGameType = GetComponent<MiniGame>();
            mDamageText = GetComponentInChildren<ScoreText>(); //자식 오브젝트의 DamageText 참조 
            mBallScreenController = GetComponentInChildren<BallScreenController>();
            mBasketBallController = GetComponentInChildren<BasketBallController>();

            // 이벤트 구독 
            mBallScreenController.StartGameEvent += StartGame;
            mBallScreenController.FeverTypeEvent += GetFeverType;
            mBallScreenController.SendFinishEvent += GetFinishSign;
            GetScoreEvent += GetScore;

            mGameController = new GameController(this, miniGameType); //GameController 생성 

            // Ball Object 생성 및 setting
            mBasketBallController.CreateBall(11, this.transform);
            mBasketBallController.SetActiveObject(false);

        }
        
        public void StartSetting()
        {
            //생성된 Ball 오브젝트 Active 활성화
            mBasketBallController.SetActiveObject(true);
            mBasketBallController.ResetBall();

            mBallScreenController.ControlActivePanel(false); //점수 패널 제거   
            Utils.RestScore(); //점수 리셋
            GetFeverType(FeverType.NONE);
            if (mTimeCorutine != null)
            {
                StopCoroutine(mTimeCorutine);
                mTimeCorutine = null;
            }
            /*   mTimeCorutine = StartCoroutine(TutorialCO()); */
            mTimeCorutine = StartCoroutine(mBallScreenController.GameStartDelay(Utils.delay));

        }
        private void StartGame(int second)
        {
            mTimeCorutine = StartCoroutine(mBallScreenController.CheckTime(second));
            
        }
        public void StopSetting()
        {

            if (mTimeCorutine != null)
            {
                StopCoroutine(mTimeCorutine);
                mTimeCorutine = null;

            }
            mBallScreenController.Reset(); //UI reset
        }

        public void FinishForceGame()
        {
            mBasketBallController.SetActiveObject(false);
            if (mTimeCorutine != null)
            {
                StopCoroutine(mTimeCorutine);
                mTimeCorutine = null;
            }

            if (mClickCorutine != null)
            {
                StopCoroutine(mClickCorutine);
                mClickCorutine = null;
            }

        }

        public void FinishGame()
        {
            mBallScreenController.ControlActivePanel(true); //점수 패널 생성  
            StopSetting();
            Utils.isExecuteMinigame = false;
            mGameController.SetGameFinish(); //게임 종료 
        }

        public void Release()
        {

            // 이벤트 구독 취소
            if (mBallScreenController != null)
            {
                mBallScreenController.StartGameEvent -= StartGame;
                mBallScreenController.FeverTypeEvent -= GetFeverType;
                mBallScreenController.SendFinishEvent -= GetFinishSign;

                mBallScreenController = null;
            }

            GetScoreEvent -= GetScore;

        }
        #endregion
    }
}