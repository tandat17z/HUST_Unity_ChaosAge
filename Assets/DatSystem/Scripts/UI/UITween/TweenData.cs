
using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class TweenData
{
    public UITweenType tweenType;
    public Transform target;
    [ShowIf("@(this.tweenType & UITweenType.Fade) == UITweenType.Fade" +
        "|| (this.tweenType & UITweenType.FadeGroup) == UITweenType.FadeGroup" +
        "|| (this.tweenType & UITweenType.Scale) == UITweenType.Scale")]
    public float from;
    [ShowIf("@(this.tweenType & UITweenType.Fade) == UITweenType.Fade" +
        "|| (this.tweenType & UITweenType.FadeGroup) == UITweenType.FadeGroup" +
        "|| (this.tweenType & UITweenType.Scale) == UITweenType.Scale")]
    public float to = 1;
    [ShowIf("@(this.tweenType & UITweenType.Move) == UITweenType.Move" +
        "|| (this.tweenType & UITweenType.RectLocalMove) == UITweenType.RectLocalMove" +
        "|| (this.tweenType & UITweenType.LocalMove) == UITweenType.LocalMove")]
    public Vector3 mFrom;
    [ShowIf("@(this.tweenType & UITweenType.Move) == UITweenType.Move" +
        "|| (this.tweenType & UITweenType.RectLocalMove) == UITweenType.RectLocalMove" +
        "|| (this.tweenType & UITweenType.LocalMove) == UITweenType.LocalMove")]
    public Vector3 mTo;
    public float duration;
    public float delay;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    public Action OnCompleted;
}
