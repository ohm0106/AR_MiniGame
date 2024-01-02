/********************************************
 * Title    : Canvas UI Manager
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ifland.main
{
    /// <summary>
    /// Main Canvas 에 있는 Panel 및 UI 관리 클래스 
    /// </summary>
    public class UIManager : MonoBehaviour
    {


        private PanelType[] panelTypes;
        private RankingTable[] mRankingTables;

        private GameManager mGameManager;

        private MiniGame curMiniGame; //현재 실행 중인 미니 게임 



        private void Start()
        {
            Init();

        }

        private void Init()
        {
            panelTypes = GetComponentsInChildren<PanelType>();
            mRankingTables = GetComponentsInChildren<RankingTable>();

            mGameManager = FindObjectOfType<GameManager>();

            ControlUIActive(GameType.NONETYPE);
        }

        /// <summary>
        /// 미니게임 타입에 맞는 UI Active 제어 
        /// </summary>
        /// <param name="_miniGameType"></param>
        public void ControlUIActive(GameType gameType, MiniGame miniGame = null)
        {
            Debug.Log(">>UIManger :: ControlUIActive Gametype : " + gameType + "MiniGame" + miniGame);
            curMiniGame = miniGame;
            foreach (var panel in panelTypes)
            {
                
                if (panel.gameType == gameType && panel.gameObject.activeSelf != true)
                {
                    panel.gameObject.SetActive(true);
                }
                else
                {
                    panel.gameObject.SetActive(false);
                }

                Debug.Log(">>UIManger :: ControlUIActive Gametype : " + gameType + "Panel GameType : " + panel.gameType + " Active is " + panel.gameObject.activeSelf);
            }
        }

        /// <summary>
        /// miniGame 종료 Btn Event 함수 
        /// </summary>
        /// <param name="id"></param>
        public void ClickMiniGameFinish(int id)
        {

            mGameManager.GameStatusEvent.Invoke(minigame.GameStatus.FORCEFINISH, curMiniGame, null);

        }

        /// <summary>
        /// miniGame 재시작 Btn Event 함수 
        /// </summary>
        /// <param name="id"></param>
        public void ClickMiniGameRestart(int id)
        {

            mGameManager.GameStatusEvent.Invoke(minigame.GameStatus.RESTART, curMiniGame, null);

        }

        /// <summary>
        /// 랭킹 표 생성 함수 
        /// </summary>
        public void CreateRankTable()
        {
            if (curMiniGame == null)
                return;

            for (int index = 0; index < mRankingTables.Length; index++)
            {
                if (mRankingTables[index].name == curMiniGame.name)
                {
                    mRankingTables[index].SetRanking();
                }
            }
        }

    }
}
