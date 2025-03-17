using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UITweenBase
{
    public event Action OnCompleted;
    public void Play();
    public void Play(float from, float to, float duration);
}
