using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ifland.main
{
    public static class Utils
    {
        public static readonly float circleR = 0.9f; //원통 반지름 
        public static readonly int objectCount = 10;
        public static Quaternion QI = Quaternion.identity;
        public static Camera MainCam = Camera.main;
        public static Vector3 cameraOffset = new Vector3(0, 2.14f, -3.59f); //기본 offset 
        public static Vector3 cameraMiniGameOffset = new Vector3(0, 0.3f, -2f); //미니게임 offset 
        public static int Score; // 미니게임 점수 
        public static readonly int gameTime = 60;
        public static readonly int delay = 3;
        public static readonly int collectCount = 3;
        public static bool isExecuteMinigame;

        /// <summary>
        /// 점수 리셋
        /// </summary>
        public static void RestScore()
        {
            Score = 0;
        }

        /// <summary>
        /// 위치 리스트 반환 함수 
        /// </summary>
        /// <returns></returns>
        public static List<Vector3> SetObjectVec(int count)
        {
            Debug.Log(">>Utils :: SetObjectVec count : " + count );
            List<Vector3> objectPositions = new List<Vector3>();
            for (int i = 1; i <= count; i++)
            {
                int angle = i * 72;
                bool isOutOfRange = true;
                if (angle > 360)
                {
                    angle = (angle - 360) + 35;
                    isOutOfRange = false;
                }
                Vector3 vec = CalculateVec3(angle, isOutOfRange);
                objectPositions.Add(vec);
                Debug.Log(">>Utils :: SetObjectVec List index : " + i + " Position : " +vec);
            }
            return objectPositions;
        }

        /// <summary>
        /// 원 둘레 위치 계산 함수 
        /// </summary>
        /// <param name="angle">각도</param>
        /// <param name="isOutOfRange">true = 범위 밖 false = 범위 안</param>
        /// <returns></returns>
        public static Vector3 CalculateVec3(float angle, bool isOutOfRange)
        { 
            var rad = Mathf.Deg2Rad * angle;
            Debug.Log(rad);
            var x = circleR * Mathf.Sin(rad);
            var z = circleR * Mathf.Cos(rad);
            var y = isOutOfRange == true ? 0.0f : 0.7f;
            var vec = new Vector3(x, y, z);
            Debug.Log("Utils Set ObjectVector" + angle + "/" + vec);
            return vec;
        }

        /// <summary>
        /// 두 Vector2 사이의 각도 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static float GetAngle(Vector2 start, Vector2 end)
        {
            Vector2 v2 = end - start;
            return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
        }


        /// <summary>
        /// 레버 및 카메라 forWard의 방향 체크 
        /// </summary>
        /// <param name="leverVec"></param>
        /// <returns></returns>
        public static LeverCondition CheckLeverVector(Vector2 leverVec)
        {
            LeverCondition lever;
       
            if(Mathf.Abs(leverVec.x) > Mathf.Abs(leverVec.y))
            {
                if (leverVec.x > 0)
                    lever = LeverCondition.RIGHT;
                else
                    lever = LeverCondition.LEFT;
            }
            else
            {
                if (leverVec.y > 0)
                    lever = LeverCondition.UP;
                else
                    lever = LeverCondition.DOWN;
            }
            return lever;

        }


        /// <summary>
        /// 컴포넌트 복사 함수 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        static public T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy as T;
        }

        /// <summary>
        /// 0 ~ max 범위 랜덤 함수 
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomNumber(int max)
        {
            int boomNum = Random.Range(0, max);
            return boomNum;
        }


        /// <summary>
        /// min ~ max 범위의 중복 되지 않는 count 개의 숫자 리스트 반환 함수 
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<int> NonDuplicationRandom(int max,int min, int count)
        {
            List<int> nums = new List<int>();
            while (nums.Count < count)
            {
                int number = Random.Range(min, max);
                if (!nums.Contains(number))
                    nums.Add(number);
            }
            return nums;
        }


    }
}