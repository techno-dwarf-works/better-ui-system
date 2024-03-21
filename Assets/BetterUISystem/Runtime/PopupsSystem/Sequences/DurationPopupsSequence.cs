using UnityEngine;

namespace Better.UISystem.Runtime
{
    public abstract class DurationPopupsSequence : PopupsSequence
    {
        [Min(0)] [SerializeField] protected float _duration;
    }
}