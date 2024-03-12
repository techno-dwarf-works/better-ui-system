using System;
using UnityEngine;

namespace Better.UISystem.Runtime.ScreensSystem.Sequences
{
    [Serializable]
    public abstract class DurationScreensSequence : ScreensSequence
    {
        [Min(0)] [SerializeField] protected float _duration;
    }
}