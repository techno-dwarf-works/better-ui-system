using System;
using System.Threading.Tasks;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;
using UnityEngine;

namespace Better.UISystem.Runtime
{
    [Serializable]
    public abstract class PopupsSequence
    {
        [Min(0)] [SerializeField] protected float _duration;

        public abstract Task DoPlay(IPopup from, IPopup to);
    }
}