using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

namespace DatSystem.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Panel : View
    {
        protected CanvasGroup panelCanvasGroup;
        public TweenData[] openTween;
        public TweenData[] closeTween;


        protected virtual void Reset()
        {
            panelCanvasGroup = GetComponent<CanvasGroup>();
        }

        public override void OnSetup()
        {
            if (panelCanvasGroup == null)
            {
                panelCanvasGroup = GetComponent<CanvasGroup>();
            }
        }

        public override void Open(UIData uiData)
        {
            this.uiData = uiData;
            gameObject.SetActive(true);
            panelCanvasGroup.interactable = false;
            PlayTweens(openTween, OnOpenCompleted).Forget();
        }

        public override void OnOpenCompleted()
        {
            panelCanvasGroup.interactable = true;
        }

        //TODO: override this method must call OnCloseCompleted() at the end
        public override void Close()
        {
            panelCanvasGroup.interactable = false;
            PlayTweens(closeTween, OnCloseCompleted).Forget();
        }

        protected override void OnCloseCompleted()
        {
            base.OnCloseCompleted();
            if (this.uiData != null && this.uiData.TryGet<Action>(UIDataKey.CallBackOnClose, out var callback))
            {
                callback?.Invoke();
            }
        }

        public override void OnFocus()
        {

        }

        public override void OnFocusLost()
        {
        }

        private async UniTask PlayTweens(TweenData[] tweenDatas, Action callback)
        {
            if (tweenDatas == null)
            {
                callback?.Invoke();
                return;
            }
            float maxTime = 0;
            for (int i = 0; i < tweenDatas.Length; i++)
            {
                UITween.Play(tweenDatas[i]).Forget();
                if (tweenDatas[i].delay + tweenDatas[i].duration > maxTime) maxTime = tweenDatas[i].delay + tweenDatas[i].duration;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(maxTime));
            callback?.Invoke();

        }
    }
}