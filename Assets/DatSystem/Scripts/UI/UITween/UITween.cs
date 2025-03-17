using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UITween
{
	public static async UniTask Play(TweenData tweenData)
	{
		switch (tweenData.tweenType)
		{
			case UITweenType.Scale:
				await PlayTweenScale(tweenData);
				break;
			case UITweenType.Fade:
				await PlayTweenFade(tweenData);
				break;
			case UITweenType.Move:
				await PlayTweenMove(tweenData);
				break;
			case UITweenType.LocalMove:
				await PlayTweenLocalMove(tweenData);
				break;
			case UITweenType.RectLocalMove:
				await PlayTweenRectLocalMove(tweenData);
				break;
			case UITweenType.FadeGroup:
				await PlayTweenFadeGroup(tweenData);
				break;
		}
	}

    private static async UniTask PlayTweenScale(TweenData tweenData)
	{
		Transform target = tweenData.target;
		target.localScale = Vector3.one * tweenData.from;
		await target.DOScale(tweenData.to, tweenData.duration).SetEase(tweenData.curve).SetDelay(tweenData.delay).OnComplete(() => { tweenData.OnCompleted?.Invoke(); }).ToUniTask();
	}

	private static async UniTask PlayTweenFade(TweenData tweenData)
	{
		Graphic target = tweenData.target.GetComponent<Graphic>();
		Color color = target.color;
		color.a = tweenData.from;
		target.color = color;
		await target.DOFade(tweenData.to, tweenData.duration).SetEase(tweenData.curve).SetDelay(tweenData.delay).OnComplete(() => { tweenData.OnCompleted?.Invoke(); }).ToUniTask();
	}

	private static async UniTask PlayTweenMove(TweenData tweenData)
	{
		Transform target = tweenData.target;
		target.position = tweenData.mFrom;
		await target.DOMove(tweenData.mTo, tweenData.duration).SetEase(tweenData.curve).SetDelay(tweenData.delay).OnComplete(() => { tweenData.OnCompleted?.Invoke(); }).ToUniTask();
	}

	private static async UniTask PlayTweenLocalMove(TweenData tweenData)
	{
		Transform target = tweenData.target;
		target.localPosition = tweenData.mFrom;
		await target.DOLocalMove(tweenData.mTo, tweenData.duration).SetEase(tweenData.curve).SetDelay(tweenData.delay).OnComplete(() => { tweenData.OnCompleted?.Invoke(); });
	}

	private static async UniTask PlayTweenRectLocalMove(TweenData tweenData)
	{
		RectTransform target = (RectTransform)tweenData.target;
		target.anchoredPosition = tweenData.mFrom;
		await target.DOAnchorPos(tweenData.mTo, tweenData.duration).SetEase(tweenData.curve).SetDelay(tweenData.delay).OnComplete(() => { tweenData.OnCompleted?.Invoke(); });
	}


	private static async UniTask PlayTweenFadeGroup(TweenData tweenData)
	{
		CanvasGroup target = tweenData.target.GetComponent<CanvasGroup>();
		target.alpha = tweenData.from;
		await target.DOFade(tweenData.to, tweenData.duration).SetEase(tweenData.curve).SetDelay(tweenData.delay).OnComplete(() => { tweenData.OnCompleted?.Invoke(); }).ToUniTask();
	}
}


public enum UITweenType
{
	None,
	Scale,
	Fade,
	Move,
	LocalMove,
	RectLocalMove,
	FadeGroup
}
