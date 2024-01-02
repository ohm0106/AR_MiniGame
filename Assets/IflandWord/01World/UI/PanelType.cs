/********************************************
 * Title    : Panel Type by Mini Game 
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ifland.main
{
    //미니게임 별 패널 정보 
    public class PanelType : MonoBehaviour
    {
        public GameType gameType;

        private GameObject panelObj;

        public RankingTable rankTable;


        private void Awake()
        {
            panelObj = this.gameObject;
            rankTable = GetComponentInChildren<RankingTable>();
        }
    }
}

