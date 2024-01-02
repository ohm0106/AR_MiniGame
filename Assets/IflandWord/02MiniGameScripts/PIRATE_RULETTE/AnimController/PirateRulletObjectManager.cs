using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace ifland.main.minigame
{
    /// <summary>
    /// 해적룰렛 오브젝트 매니저 
    /// </summary>
    public class PirateRulletObjectManager : MonoBehaviour, IGameObjectManager
    {
        private Dictionary<int, GameObject> objs = new Dictionary<int, GameObject>(); //칼 instant 오브젝트 Dic
        private Dictionary<GameObject, int> uiObjs = new Dictionary<GameObject, int>(); //버튼 instant 오브젝트 Dic

        private Dictionary<GameObject, int> uiObjsStorage = new Dictionary<GameObject, int>(); //버튼 instant 오브젝트 저장소

        public GameObject knifePrefabs; // 칼 오브젝트 지정 
        public GameObject buttonPrefabs; // UI 버튼 오브젝트 지정 

        [HideInInspector]
        public TurnAnimEvent mTurnAnimEvent;

        [HideInInspector]
        public ClickBtnEvent mClickEvent;

        [HideInInspector]
        public GameFlowEvent mFlowEvent;

        private GameController mGameManager;

        
        public GameType gameType;


        private int boomNum;


        private MiniGame minigameType;

        private void Awake()
        {
            OnINIT();
        }
        private void OnEnable()
        {

        }
        private void OnDestroy()
        {
            Release();
        }

        #region IGameObjectManager
        /// <summary>
        /// 게임 오브젝트 초기 설정
        /// </summary>
        public void OnINIT()
        {
            Debug.Log(">>ObjectManager :: OnINIT() Start!!! ------------");
            minigameType = GetComponent<MiniGame>();
            if (mGameManager == null)
                mGameManager = new GameController(this, minigameType);

            boomNum = Utils.RandomNumber(10); //랜덤 숫자 생성 

            List<Vector3> objPositions = new List<Vector3>();
            objPositions = Utils.SetObjectVec(10); //UI Object 위치 리스트 설정
            for (int i = 0; i < objPositions.Count; i++)
            {
                // knife 객체 생성
                var obj = Instantiate(knifePrefabs, this.gameObject.transform);
                obj.transform.localPosition = objPositions[i];
                obj.transform.LookAt(this.gameObject.transform.position);

                objs.Add(i, obj);
                obj.SetActive(false);

                //UI Button 객체 생성
                var uiObj = Instantiate(buttonPrefabs, this.gameObject.transform);
                uiObj.transform.localPosition = objPositions[i];
                var uiObjSc = uiObj.GetComponent<ButtonController>();
                uiObjSc.mObjManager = this;

                uiObjs.Add(uiObj, i);
                uiObjsStorage.Add(uiObj, i);
                Debug.Log(">>ObjectManager :: OnINIT() " + i + "Object Instantiate" + obj + "/" + uiObj);
            }
            mClickEvent.AddListener(CheckBtnClick);
            mFlowEvent.AddListener(FinishSetting);
            Debug.Log(">>ObjectManager :: OnINIT() Finish!!! ------------");
        }

        public void StartSetting()
        { 
            Debug.Log(">>ObjectManager :: StartSetting START ------------");

            boomNum = Utils.RandomNumber(10);
            if (uiObjs != null)
                uiObjs.Clear();

            foreach (var uiObj in uiObjsStorage)
            {
                uiObjs.Add(uiObj.Key, uiObj.Value);
                Debug.Log(">> StartSetting :: Add List " + uiObj.Key + "/" + uiObj.Value);
            }
            foreach (var obj in objs)
            {
                obj.Value.SetActive(false); //knife 함수 비활성화 
                Debug.Log(">> StartSetting :: Knife Obj Active False " + obj.Key + "/" + obj.Value);
            }

            OnEnabledControl(true);
            Debug.Log(">>ObjectManager :: StartSettion FINISH ------------");
        }

        /// <summary>
        /// 릴리즈 함수 
        /// </summary>
        public void Release()
        {
            Debug.Log(">>ObjectManager :: Release START ------------");
            if (objs != null)
            {
                Debug.Log(">> Release :: Knife Object Dictionary is not null");
                for (int i = 0; i < objs.Count; i++)
                {
                    Debug.Log(">> Release :: Destroy Knife Object " + objs[i].name);
                    DestroyImmediate(objs[i]);
                }
                objs.Clear();
            }
            if (uiObjs != null)
            {
                Debug.Log(">> Release :: UI Object Dictionary is not null");
                foreach (var arr in uiObjs)
                {
                    Debug.Log(">> Release :: Destroy UI Object" + arr.Key.name);
                    DestroyImmediate(arr.Key);

                }
                uiObjs.Clear();
            }
            Debug.Log(">>ObjectManager :: Release FINISH ------------");
        }


        #endregion
        /// <summary>
        /// 버튼 UI 클릭시 실행되는 함수 
        /// 클릭된 Button 오브젝트의 index와 boomNum가 부합한지 체크 한다.
        /// </summary>
        /// <param name="button"> 클릭된 버튼 GameObject </param>
        private void CheckBtnClick(GameObject button)
        {
            if (!uiObjs.ContainsKey(button))
                return;
            int num = this.uiObjs[button];
            Debug.Log(">>Object Manager :: CheckBtnClick number = " + num);
            if (boomNum == num)
            {
                Debug.Log(">>CheckBtnClick :: boomNum is num ");
                this.mTurnAnimEvent.Invoke(true); // Boom
                this.mGameManager.CheckGameState(AnimationType.LOSE);
            }
            else
            {
                Debug.Log(">>CheckBtnClick :: boomNum is not num ");
                this.mTurnAnimEvent.Invoke(false); // not Boom
                this.uiObjs.Remove(button);
                this.mGameManager.CheckGameState(AnimationType.WIN);
            }
            OnEnabledControl(false);
            this.objs[num].SetActive(true); //knife 오브젝트 Active
        }

        /// <summary>
        /// UI GameObject Enable 제어 
        /// </summary>
        /// <param name="isActive"></param>
        public void OnEnabledControl(bool isActive)
        {
            Debug.Log(">>Object Manager :: OnEnabledControl Start");

            foreach (var arr in this.uiObjs)
            {
                Debug.Log(">>OnEnabledControl :: OnEnabledControl UI Active = " + arr.Key.activeSelf + "Swich Active" + isActive);
                if (arr.Key.activeSelf != isActive)
                    arr.Key.SetActive(isActive);
            }
            Debug.Log(">>Object Manager :: OnEnabledControl End");
        }

        /// <summary>
        /// 게임 종료 셋팅 
        /// </summary>
        /// <param name="gameStatus"></param>
        private void FinishSetting(GameStatus gameStatus)
        {
            if (gameStatus == GameStatus.FINISH)
            {
                FinishGame();
                Debug.Log(">>Object Manager :: FinishSetting " + gameStatus);
                //게임 끝 
            }
        }

        public void FinishGame()
        {
            StartSetting();
        }

        

        public void StopSetting()
        {
            throw new System.NotImplementedException();
        }

        public void FinishForceGame()
        {
            throw new System.NotImplementedException();
        }
    }

}
