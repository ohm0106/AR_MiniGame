/********************************************
 * Title    : MiniGame Ranking Table 
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ifland.main
{
    /// <summary>
    /// 미니게임 랭킹 표 생성 및 제거 클래스 
    /// </summary>
    public class RankingTable : MonoBehaviour
    {
        private List<RankingType> rankingTypes = new List<RankingType>();
        private List<int> scores = new List<int>();
        public GameObject rankObjPrefab;
        public GameObject thisObj;
        public MiniGameName name;
        private void Awake()
        {
            if(thisObj == null)
                thisObj = this.gameObject;
        }

        private void OnEnable()
        {
            
        }

        private void OnDestroy()
        {
            if(rankingTypes != null)
            {
                Debug.Log(">>RankingTable :: OnDestroy Ranking Table " + rankingTypes.Count);
                foreach ( var rankingType in rankingTypes)
                {
                    Destroy(rankingType.gameObject);
                    Debug.Log(">>RankingTable :: OnDestroy Ranking Table GameObject Destroy" + rankingType.gameObject == null);
                }
                rankingTypes = null;
            }
        }

        /// <summary>
        /// 랭크 score 표 추가 함수 
        /// </summary>
        public void CreateRank()
        {
            //랭크 score 오브젝트 생성 후 리스트 추가 
            GameObject rankObj = Instantiate(rankObjPrefab,this.gameObject.transform);
            RankingType ranktype = rankObj.GetComponent<RankingType>();
            rankingTypes.Add(ranktype);
        }

        /// <summary>
        /// Ranking 표 setting 함수 
        /// </summary>
        public void SetRanking()
        {
            scores.Add(Utils.Score);
            // 내림차순 정렬
            scores.Sort(); //오름차순
            scores.Reverse(); //역순

            if(scores.Count < 6)
            {
                CreateRank(); 
            }

            // 내림차순 정렬된 score에 따라 랭킹 정보 변경 
            for (int i = 0; i < rankingTypes.Count; i++)
            {
                RankingType ranking = rankingTypes[i];
                RankType ranktype;
                if (i == 0)
                {
                    ranktype = RankType.FIRST_PLACE; //1등
                }
                else
                {
                    ranktype = RankType.NON_FIRST_PLACE; //~1등
                }
                ranking.SetRankingUI(ranktype, i+1, scores[i]);


                }
        }
    }
}
