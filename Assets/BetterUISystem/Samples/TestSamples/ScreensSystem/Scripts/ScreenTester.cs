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
            ScreenUtility.Open<TestScreen, TestModel>(new TestModel());
        }

        [EditorButton(captureGroup: 1)]
        private void OpenScreenA()
        {
            ScreenUtility.Open<TestScreenA, TestModelA>(new TestModelA());
        }

        [EditorButton(captureGroup: 1)]
        private void OpenScreenB()
        {
            ScreenUtility.Open<TestScreenB, TestModelB>(new TestModelB());
        }

        [EditorButton(captureGroup: 2)]
        private void OpenHistoryScreen()
        {
            var historyDepth = Random.Range(1, 6);

            ScreenUtility.OpenHistory(historyDepth);
        }

        [EditorButton(captureGroup: 2)]
        private void OpenHistoryScreenFromCount()
        {
            ScreenUtility.OpenHistory(_historyDepth);
        }
    }
}