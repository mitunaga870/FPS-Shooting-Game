using System;
using System.Collections;
using System.Collections.Generic;
using InvasionPhase;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;

namespace lib
{
    public static class General
    {
        public static bool IsPointerOverUIObject()
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        /**
         * 遅延処理用コルーチン
         * @param seconds 遅延時間(sec)
         */
        public static IEnumerator DelayCoroutine(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }

        /**
         * 遅延処理用コルーチン作成
         * ゲーム内時間で遅延処理を行う
         */
        public static IEnumerator DelayCoroutineByGameTime(
            InvasionController sceneController,
            int time,
            Action action
        )
        {
            // ゲーム内時間での遅延時間を計算
            var delayTime = sceneController.GameTime + time;
            while (sceneController.GameTime < delayTime) yield return null;

            action?.Invoke();
        }
    }
}