/********************************************
 * Title    : InOrder Mini Game Btn Manager
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace ifland.main.minigame.NUMBERCHOOSE
{
    /// <summary>
    /// 미니게임 Inorder button object 
    /// </summary>
    public class NumberBtn : MonoBehaviour
    {
        #region # Property
        public int ID { get; set; } 

        public int Number { get; set; }

        public GameObject Obj { get; set; }

        public Button Btn { get; private set; }
        #endregion

        
        [HideInInspector]
        public TMP_Text txt;

        public Image img;

        public FeverType feverType; 

        public NumberChooseObjectManager objManager;
        public RectTransform trans;
    


        private void Start()
        {
            //컴포넌트 참조
            this.txt = Obj.GetComponentInChildren<TMP_Text>();
            this.img = Obj.GetComponentInChildren<Image>();
            this.trans = Obj.GetComponent<RectTransform>();

            //버튼 이벤트 구독 
            Btn = GetComponent<Button>();
            Btn.onClick.AddListener(Clickbtn);

            Setting(ID,Number.ToString());
            Debug.Log(">> ScreenBtn :: Create ScreenBtn " + ID + " / " + Number);
            
            //버튼 renderer false 설정 
            img.enabled = false;
            txt.enabled = false;
        }



       /// <summary>
       /// btn ui 설정 함수 
       /// </summary>
       /// <param name="number"></param>
       /// <param name="id"></param>
        public void Setting(int id,string number)
        {
            ID = id;
            txt.text = number;
            Debug.Log(">>InorderBtn :: Setting id : " + id + " number : " + number);
        }

        /// <summary>
        /// 버튼 클릭시 실행되는 함수 
        /// 클릭 시 fevertime 인지 체크하여 점수 제어 
        /// </summary>
        public void Clickbtn()
        {
            switch (feverType)
            {
                case FeverType.NONE:
                    bool iscollect = objManager.CheckCollect(ID, this, 100);
                    break;
                case FeverType.COLLECTFEVER:
                    bool isFever = objManager.CheckCollect(ID, this, 200);
                    break;

                case FeverType.TIMEFEVER:
                    objManager.TimeFever();
                    break;
            }
        }


        /// <summary>
        /// Fever Type 을 전달받는 함수 
        /// Fever Type에 알맞게 버튼 setting 
        /// </summary>
        /// <param name="_feverType"></param>
        public void SettingFeverType(FeverType _feverType)
        {
            if(feverType != _feverType)
            {

                switch (_feverType)
                {
                    case FeverType.NONE:
                    case FeverType.COLLECTFEVER:
                        if(feverType == FeverType.TIMEFEVER)
                        {
                            objManager.CheckCollect(ID, this, 100 ,true);
                        }
                        break;
                    case FeverType.TIMEFEVER:
                        Setting(0, "('-')");
                        img.enabled = true;
                        txt.enabled = true;
                        break;
                }
                feverType = _feverType;

            }


        }
    }
}
