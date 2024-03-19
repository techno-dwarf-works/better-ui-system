using System.Threading.Tasks;
using Better.Extensions.Runtime;
using UnityEngine;

namespace Samples.TestSamples.ScreensSystem.Scripts
{
    public static class ScreenTesterUtility
    {
        public static Task GetRandomWaitTask()
        {
            var frameCount = Random.Range(1, 50);
            return TaskUtility.WaitFrame(frameCount);
        }
    }
}