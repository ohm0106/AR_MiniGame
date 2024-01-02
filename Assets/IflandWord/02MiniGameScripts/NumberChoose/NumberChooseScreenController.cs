/********************************************
 * Title    : BasketBall Mini Game Object Manager
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ifland.main.minigame.NUMBERCHOOSE
{
    /// <summary>
    /// 숫자 선택 게임 상태에 따른 게임 상호작용 클래스 
    /// </summary>
    public class NumberChooseObjectManager : Singleton<NumberChooseObjectManager>,IGameObjectManager
    {

        private GameController mMiniGameController;
        private MiniGame miniGameType;

        private NumberScreenController mScreenController = null;

        private List<NumberBtn> screenBtns = new List<NumberBtn>();
        private List<int> randomNums = new List<int>();

        private MiniGameName miniGameName = MiniGameName.INORDER;

        public Animator anim; 

        private int answerID = 0; //정답 ID
        private int collectCount = 0;

        private Coroutine mGameflowCO = null; 

        private ScoreText mDamageText = null;

        protected override void OnEnable()
        {
            miniGameType = GetComponent<MiniGame>();
            mMiniGameController = new GameController(this, miniGameType); //GameController 생성 
            mScreenController = GetComponentInChildren<NumberScreenController>(); //자식 오브젝트의 ScreenController 참조 
            mDamageText = GetComponentInChildren<ScoreText>(); //자식 오브젝트의 DamageText 참조 
        }

        protected override void Start()
        {
            OnINIT();
        }

        protected override void OnDestroy()
        {
            Release();
        }

        /// <summary>
        /// 램덤으로 4개의 중복이 없는 숫자를 뽑아 오른차순으로 정렬 함수 
        /// </summary>
        private void RandomNumber()
        {
            if (randomNums.Count != 0)
                randomNums.Clear();

            randomNums = Utils.NonDuplicationRandom( 100, 1, (int)MiniGameName.INORDER);
            randomNums.Sort(); //오름차순 정렬 
        }




        #region IGameObjectManager


        /// <summary>
        /// 초기화 함수 
        /// </summary>
        public void OnINIT()
        {
            RandomNumber();

            Debug.Log(">>UIObjectManager :: StartSetting number count is " + randomNums.Count);
            for (int i = 0; i < (int)miniGameName; i++)
            {
                NumberBtn numberBtn = mScreenController.CreatBtnObj(i, randomNums[i], this);
                screenBtns.Add(numberBtn);

            }
            mScreenController.FeverTypeEvent += GetFeverType;
            mScreenController.SendFinishEvent += GetFinishSign;
            mScreenController.StartGameEvent += StartGame;
        }
        /// <summary>
        /// 게임 시작 함수 
        /// </summary>
        /// <param name="second"></param>
        private void StartGame(int second)
        {
            if (mGameflowCO != null)
            {
                StopCoroutine(mGameflowCO);
                mGameflowCO = null;
            }

            mGameflowCO = StartCoroutine(mScreenController.CheckTime(second));

            SetRandomNumber();
        }

       
        /// <summary>
        /// 게임 시작 setting 함수 
        /// </summary>
        public void StartSetting()
        {
            mScreenController.ControlActivePanel(false); //점수 패널 제거   
            Utils.RestScore();
            GetFeverType(FeverType.NONE);
            if (mGameflowCO != null)
            {
                StopCoroutine(mGameflowCO);
                mGameflowCO = null;
            }
            /*   mTimeCorutine = StartCoroutine(TutorialCO());*/
            mGameflowCO = StartCoroutine(mScreenController.GameStartDelay(Utils.delay));
            answerID = 0;
            collectCount = 0;
        }

        /// <summary>
        /// 게임 stop 시 종료 setting 함수 
        /// </summary>
        public void StopSetting()
        {
            for (int i = 0; i < screenBtns.Count; i++)
            {
                SetEnableBtnRenderer(screenBtns[i], false);

            }
            if (mGameflowCO != null)
            {
                StopCoroutine(mGameflowCO);
                mGameflowCO = null;

            }


            mScreenController.Reset(); //UI reset

        }

        /// <summary>
        /// 게임 종료 
        /// </summary>
        public void FinishGame()
        {
            mScreenController.ControlActivePanel(true); //점수 패널 생성  
            StopSetting();
            mMiniGameController.SetGameFinish(); //게임 종료 

        }


        public void Release()
        {
            //List 항목 삭제 
            if (randomNums.Count != 0)
                randomNums.Clear();
            //List 항목 삭제 및 버튼 오브젝트 삭제 
            if (screenBtns.Count != 0)
            {
                foreach (var obj in screenBtns)
                {
                    Destroy(obj.Obj);
                }
                randomNums.Clear();
            }
            //이벤트 구독 취소 
            if (mScreenController != null)
            {
                mScreenController.SendFinishEvent -= GetFinishSign;
                mScreenController.FeverTypeEvent -= GetFeverType;
                mScreenController.StartGameEvent -= StartGame;
                mScreenController = null;
            }

        }

        #endregion



        /// <summary>
        /// 숫자 리스트에서 랜덤 버튼 id 및 number 값 부여 함수 
        /// </summary>
        private void SetRandomNumber()
        {
            List<int> nums = Utils.NonDuplicationRandom(4, 0, (int)MiniGameName.INORDER);

            for (int i = 0; i < screenBtns.Count; i++)
            {
                SetEnableBtnRenderer(screenBtns[nums[i]], true);
                screenBtns[nums[i]].Setting(i, randomNums[i].ToString());
            }
        }


        /// <summary>
        /// screen ui renderer enable set 함수 
        /// </summary>
        /// <param name="isEnable">Enable 여부</param>
        /// <param name="screenBtn">Enalble 적용될 ScreenBtn</param>
        private void SetEnableBtnRenderer(NumberBtn numberBtn, bool isEnable)
        {
            numberBtn.img.enabled = isEnable;
            numberBtn.txt.enabled = isEnable;
        }



        /// <summary>
        /// 버튼 클릭 시 오름차순으로 알맞은 버튼 클릭 체크 함수 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckCollect(int id, NumberBtn numberBtn, int plusNumber, bool? isChange = null)
        {
            if (answerID == id)
            {
                /*   obj.SetActive(false);*/
                SetEnableBtnRenderer(numberBtn, false);
                answerID++;

                if ((answerID == randomNums.Count))
                {
                    collectCount++;
                    SettingScore("doCollect", plusNumber, true);
                }
                //collectCount가 3이상일 때 Fever Time!
                if (collectCount == Utils.collectCount)
                {
                    GetFeverType(FeverType.COLLECTFEVER);
                }
                Debug.Log(">>UIObjectManager :: CheckCollect : id = " + id + "answerID = " + answerID);
                return true;
            } 
            else if (isChange == true)
            {
               
                SettingScore("doCollect", plusNumber, true);
                return true;
            }
            else
            {
                answerID = 0;
                collectCount = 0; //초기화
                SettingScore("doNonCollect", 10, false);
                GetFeverType(FeverType.NONE);
                Debug.Log(">>UIObjectManager :: CheckCollect : is Not Collect : id = " + id + "answerID = " + answerID);
                return false;
            }
        }

        /// <summary>
        /// 점수 setting 함수 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="num"></param>
        /// <param name="isCollect"></param>
        private void SettingScore(string parameter,int num , bool isCollect)
        {
            answerID = 0;
            RandomNumber();
            SetRandomNumber();
            anim.SetTrigger(parameter);
            //점수 추가 
            mScreenController.CalculateScore(num, isCollect);
            //점수 효과 
            string sign = isCollect ? "+" : "-";
            mDamageText.SetScoreText(sign, num);
        }

        /// <summary>
        /// Time Fever 전환 시 실행 되는 함수 
        /// #1. 어떤 버튼을 클릭해도 150점을 준다. 
        /// #2. 점수 text Setting 해준다 
        /// </summary>
        public void TimeFever()
        {
            mScreenController.CalculateScore(150, true);
            mDamageText.SetScoreText("+", 150);
        }


        /// <summary>
        /// Fever 타임 시작 시 ScreenBtn 에 전달 함수 
        /// </summary>
        /// <param name="_feverType"></param>
        public void GetFeverType(FeverType _feverType)
        {
            FeverSetting(_feverType);
            foreach (var btn in screenBtns)
            {
                btn.SettingFeverType(_feverType);
            }
        }
        
        /// <summary>
        /// Fever time이 되었을 때 Setting 함수 
        /// </summary>
        /// <param name="isFever"></param>
        public void FeverSetting(FeverType _feverType)
        {
            bool isFever = _feverType != FeverType.NONE ? true : false;
            mScreenController.anim.SetBool("isFever", isFever);          
        }
      
        

        /// <summary>
        /// 게임 시작 버튼 
        /// </summary>
        public void ClickStartBtn()
        {
            mMiniGameController.SetGameStart(); //게임 시작 
            StartSetting();
        }

       

        /// <summary>
        /// 게임 종료 사인이 있을 시 실행되는 함수 
        /// </summary>
        /// <param name="isFinish"></param>
        private void GetFinishSign(bool isFinish)
        {
            if (isFinish)
            {
                FinishGame();
            }
        }

        /// <summary>
        /// 외부 world에서 강제 종료 시 작동 함수 
        /// </summary>
        public void FinishForceGame()
        {
            mScreenController.ControlActivePanel(false); //점수 패널 false

        }
    }
}

