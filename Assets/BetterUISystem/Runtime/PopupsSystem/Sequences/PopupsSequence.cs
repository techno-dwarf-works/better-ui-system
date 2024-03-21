using System;
using System.Threading.Tasks;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;

namespace Better.UISystem.Runtime
{
    [Serializable]
    public abstract class PopupsSequence
    {
        public abstract Task DoPlay(IPopup from, IPopup to);
    }
}