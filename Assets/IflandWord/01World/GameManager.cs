/********************************************
 * Title    : 3D World Game Manager
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using ifland.main.minigame;

namespace ifland.main
{

    /// <summary>
    /// 미니게임 상태를 3d world Camera, Player, UI 객체 한테 정보 전달하는 클래스 
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        //미니게임 임시 프리펩
        public GameObject miniGame;
        public GameObject UIminiGame;
        public GameObject basketBallMiniGame;

        //이벤트 멤버
        public Action<GameController, MiniGameName, AnimationType> GameInteractiveEvent; //미니게임 상호작용 이벤트 
        public Action<GameStatus,MiniGame, GameController> GameStatusEvent; //미니게임 상태 전달 이벤트 

        public PlayerController mPlayerController;
        private IAnimationController animationHandler;


        private UIManager mUiManager = null;

        private CameraController mCameraController = null;


        private MiniGame curMinigame;
        private GameController miniGameHandler;

        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// 초기화 함수 
        /// </summary>
        private void Init()
        {
            //미니게임 오브젝트 생성 -----
            /*   var obj = Instantiate(miniGame);
            obj.transform.eulerAngles = new Vector3(0, 180, 0);

            var uiObj = Instantiate(UIminiGame);
            uiObj.transform.position = new Vector3(-3.64f, 0, 0);
            uiObj.transform.eulerAngles = Vector3.zero;*/
            //--------------------------

            //이벤트 구독
            GameInteractiveEvent += InteractiveGameObject;
            GameStatusEvent += CheckStatusMiniGame;

            if (mPlayerController != null)
                animationHandler = mPlayerController.GetComponent<IAnimationController>();

            if (mCameraController == null)
                mCameraController = Camera.main.GetComponent<CameraController>();

            Debug.Log("minigame controller  camera null? " + mCameraController == null);
            mUiManager = FindObjectOfType<UIManager>();

        }

        private void OnDestroy()
        {
            //이벤트 구독 취소 
            GameInteractiveEvent -= InteractiveGameObject;
            GameStatusEvent -= CheckStatusMiniGame;
        }

        /// <summary>
        /// MiniGame 오브젝트의 Game Win or Lose 상태를 전달받아 오는 함수
        /// </summary>
        /// <param name="_gameController"></param>
        /// <param name="_miniGameName"></param>
        /// <param name="_aniType"></param>
        private void InteractiveGameObject(GameController _gameController, MiniGameName _miniGameName, AnimationType _aniType)
        {

            Debug.Log(">> GameManager :: InteractiveGameObject " + _gameController + "/" + _miniGameName.ToString() + "/" + _aniType.ToString());
            switch (_miniGameName)
            {
                case MiniGameName.PIRATE_RULETTE:
                    if (_aniType == AnimationType.WIN)
                    {
                        Debug.Log(">> InteractiveGameObject :: is Win " + _aniType);
                        // #1.플레이어 Win 애니메이션
                        animationHandler?.PlayTriggerAnimation(AnimationType.WIN, "doWin");

                        // #2.Camera Particle 재생 
                    }
                    else if (_aniType == AnimationType.LOSE)
                    {
                        Debug.Log(">> InteractiveGameObject :: is Lose " + _aniType);
                        // #1.플레이어 Lose 애니메이션
                        animationHandler?.PlayTriggerAnimation(AnimationType.LOSE, "doLose");

                        // #2.Camera Particle 재생 
                        var particleObj = Utils.MainCam.GetComponentInChildren<ParticleSystem>(); //Particle object 접근 (수정 예정)
                        particleObj.Play();

                    }
                    break;
            }
        }

        /// <summary>
        /// 미니게임 시작, 게임 중, 종료 에 대한 상태를 전달 받아 오는 함수 
        /// </summary>
        /// <param name="_gameStatus">미니게임 상태</param>
        /// <param name="_minigame">미니게임 이름</param>
        /// <param name="gameController">실행 중인 미니게임 Game Controller 정보 </param>
        private void CheckStatusMiniGame(GameStatus _gameStatus, MiniGame _minigame = null, GameController gameController = null)
        {
            Debug.Log(">>GameManager :: CheckStatusMiniGame  GameStatus" + _gameStatus + "MiniGeme : " + _minigame + " GemaController : " + gameController);
            //게임 상태에 따른 상호작용
            switch (_gameStatus)
            {
                    //게임 재 시작 시 
                case GameStatus.RESTART:
                    miniGameHandler?.mObjectHandler?.StopSetting();
                    miniGameHandler?.mObjectHandler?.StartSetting();
                    break;
                    //게임 종료 시 
                case GameStatus.FINISH:
                    mUiManager.ControlUIActive(GameType.NONETYPE); // Main UI Canvas Setting
                    mCameraController.CheckMiniGameStartEvent.Invoke(false, null);
                    miniGameHandler?.mObjectHandler?.StopSetting();
                    break;
                    //미니게임 강제 종료 시 
                case GameStatus.FORCEFINISH:
                    mUiManager.ControlUIActive(GameType.NONETYPE); // Main UI Canvas Setting
                    mCameraController.CheckMiniGameStartEvent.Invoke(false, null);
                    miniGameHandler?.mObjectHandler?.StopSetting();
                    miniGameHandler?.mObjectHandler?.FinishForceGame();
                    break;

                    //미니 게임 시작 시 
                case GameStatus.START:
                    curMinigame = _minigame;
                    if (gameController != null)
                    {
                        miniGameHandler = gameController;
                    }

                    if (_minigame.type == GameType.TWO_DIMENSIONAL)
                    {
                        mUiManager.ControlUIActive(_minigame.type, _minigame); // Main UI Canvas Setting
                        mCameraController.CheckMiniGameStartEvent.Invoke(true, _minigame);
                    }
                    else if (_minigame.type == GameType.THERE_DIMENSIONAL)
                    {
                        mUiManager.ControlUIActive(_minigame.type, _minigame); // Main UI Canvas Setting
                        mCameraController.CheckMiniGameStartEvent.Invoke(true, _minigame);
                    }

                    Debug.Log(">>GameManager :: CheckStatusMiniGame / Game Status " + _gameStatus + "/MiniGame Name " +
                        _minigame.name + "/Game Type " + _minigame.type);

                    break;
                case GameStatus.FEVER:
                case GameStatus.WAITING:
                    mUiManager.CreateRankTable();
                    break;

            }
        }
      





    }

}
