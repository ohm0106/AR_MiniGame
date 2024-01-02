using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ifland.main;


namespace ifland.main.minigame
{
    //미니 게임 진행 상태 
    public enum GameStatus
    {
        WAITING,
        RESTART,
        START,
        FEVER,
        FINISH,
        FORCEFINISH
    }

    public class GameController
    {
  
        private GameStatus status;
        public GameStatus Status { get; private set; }

        public GameFlowEvent mGameFlowEvent;

        public readonly IGameObjectManager mObjectHandler = null;

        private readonly GameManager mGameManager = null;

        private MiniGame miniGameType;

        /// <summary>
        /// 게임 매니저 생성자 함수 
        /// </summary>
        public GameController(IGameObjectManager callback, MiniGame _minigameType) 
        {
            this.mObjectHandler = callback;
            this.miniGameType = _minigameType;

            if (this.mGameManager == null)
                this.mGameManager = GameObject.FindObjectOfType<GameManager>();
            Debug.Log(">>GameController ::  Game Status = " + Status);
        }

        /// <summary>
        /// 미니 게임 종료 후 Wating 상태 지정 함수 [GameStatus.WAITITNG = 게임 재시작 가능] 
        /// </summary>
        public void SetGameFinish()
        {
            
            Debug.Log(">>GameController :: CheckGameFinish");

            Status = GameStatus.WAITING;
            mGameManager.GameStatusEvent.Invoke(Status, miniGameType, this); // Minigame 이름과 게임 상태 전달 
            
        }

        /// <summary>
        /// 게임 시작 함수 
        /// </summary>
        public void SetGameStart()
        {
            Status = GameStatus.START;
            mGameManager.GameStatusEvent.Invoke(Status, miniGameType, this); // Minigame 이름과 게임 상태 전달 
        }

        /// <summary>
        /// 게임 상태에 따른 Player Animation 타입 전달 함수  
        /// </summary>
        /// <param name="animationType"></param>
        public void CheckGameState(AnimationType animationType)
        {
            this.mGameManager.GameInteractiveEvent(this, miniGameType.name, animationType);
        }
    }
}