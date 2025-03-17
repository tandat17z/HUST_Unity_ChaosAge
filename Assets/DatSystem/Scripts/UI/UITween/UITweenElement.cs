using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITweenElement : MonoBehaviour
{
	public TweenData tweenData;
	public bool playOnAwake = true;


	private void OnEnable()
	{
		if (tweenData.target == null) tweenData.target = this.transform;
		if (playOnAwake)
		{
			Play();
		}
	}


	public void Play()
	{
		UITween.Play(tweenData).Forget();
	}

}
