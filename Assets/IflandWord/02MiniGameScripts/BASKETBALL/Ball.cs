/********************************************
 * Title    : Ball Object impomation
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ifland.main.minigame.BASKETBALL
{
    /// <summary>
    /// 공 오브젝트 정보 클래스  
    /// </summary>
    public class Ball : MonoBehaviour
    {
        private BallGrade preGrade; //이전 등급
        public BallGrade CurGrade { get; private set; } //현재 등급


        private int count = 0; // 공이 골에 들어간 카운트 갯수 
        public int Count // 공이 골에 들어간 카운트 갯수 
        {
            get { return count; }
            set
            {
                preGrade = CurGrade;

                //count 범위를 지정해서 current Ball Grade 제어
                if (value <= 5)
                {
                    CurGrade = BallGrade.BASIC;
                } 
                else if (value <= 10)
                {
                    CurGrade = BallGrade.SLIVER;
                }
                else if(value <= 20)
                {
                    CurGrade = BallGrade.GOLD;
                }
                else if(value <= 30)
                {
                    CurGrade = BallGrade.DIAMOND;
                }
                else
                {
                    CurGrade = BallGrade.OBSIDIAN;
                }

                //현재 grade가 이전 grade와 다르다면 Material 변경 
                if (CurGrade != preGrade)
                {
                    Controller.ChangeMaterial(this.CurGrade,  this.meshRenderer);
                }
                
                count = value;

            }
        }

        private BasketBallController controller = null; 
        public BasketBallController Controller { get; set; }

        private MeshRenderer meshRenderer;

        private void Awake()
        {
            CurGrade = BallGrade.BASIC; // 등급 초기화 

            if(meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Target")
            {
                Count++; 
            }
               
        }
    }
}