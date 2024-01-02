/********************************************
 * Title    : MiniGame Ranking Type 
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace ifland.main
{
    /// <summary>
    /// 랭킹 순위 enum
    /// </summary>
    public enum RankType :int
    {
        FIRST_PLACE = 0,
        NON_FIRST_PLACE = 1
    }

    /// <summary>
    /// 랭킹 오브젝트 객체 
    /// </summary>
    public class RankingType : MonoBehaviour
    {
        public Sprite[] sprites;

        public int rankId;
        public RankType type;
        public TMP_Text rank;
        public TMP_Text score;
        public TMP_Text playerName;

        private Image img;

        private void Awake()
        {
            img = GetComponent<Image>();
        }

        /// <summary>
        /// 랭킹 오브젝트 UI 설정 
        /// </summary>
        /// <param name="_type">랭킹 타입 </param>
        /// <param name="_rank">랭킹 순위 </param>
        /// <param name="_score">랭킹 점수 </param>
        public void SetRankingUI(RankType _type,int _rank, int _score)
        {
            
            type = _type;
            img.sprite = sprites[(int)type];
            rank.text = _rank.ToString();
            score.text = string.Format("{0:#,###}", _score);
        }

    }
}