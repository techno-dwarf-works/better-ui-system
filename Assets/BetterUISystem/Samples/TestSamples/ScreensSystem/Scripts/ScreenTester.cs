using System;
using Better.Attributes.Runtime;
using Better.UISystem.Runtime.ScreensSystem.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Samples.TestSamples.ScreensSystem.Scripts
{
    public class ScreenTester : MonoBehaviour
    {
        [Min(1)] [SerializeField] private int _historyDepth;

        private void Awake()
        {
            OpenScreen();
        }

        [EditorButton(captureGroup: 1)]
        private void OpenScreen()
        {
            //ScreenSystemExtensions.Open<TestScreen, TestModel>(new TestModel());
        }

        [EditorButton(captureGroup: 1)]
        private void OpenScreenA()
        {
            //ScreenSystemExtensions.Open<TestScreenA, TestModelA>(new TestModelA());
        }

        [EditorButton(captureGroup: 1)]
        private void OpenScreenB()
        {
            //ScreenSystemExtensions.Open<TestScreenB, TestModelB>(new TestModelB());
        }

        [EditorButton(captureGroup: 2)]
        private void OpenHistoryScreen()
        {
            var historyDepth = Random.Range(1, 6);

           // ScreenSystemExtensions.OpenHistory(historyDepth);
        }

        [EditorButton(captureGroup: 2)]
        private void OpenHistoryScreenFromCount()
        {
           // ScreenSystemExtensions.OpenHistory(_historyDepth);
        }
    }
}