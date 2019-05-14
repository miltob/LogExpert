﻿using LogExpert.Classes.Columnizer;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace LogExpert.Tests
{
    /// <summary>
    /// Summary description for AutoColumnizerTest
    /// </summary>
    [TestFixture]
    public class ColumnizerManagerTest
    {
        [TestCase("Square Bracket Columnizer", "30/08/2018 08:51:42.712 [TRACE]    [a] hello", "30/08/2018 08:51:42.712 [DATAIO]   [b] world", null, null, null)]
        [TestCase("Square Bracket Columnizer", "30/08/2018 08:51:42.712 [TRACE]     hello", "30/08/2018 08:51:42.712 [DATAIO][]    world", null, null, null)]
        [TestCase("Square Bracket Columnizer", "", "30/08/2018 08:51:42.712 [TRACE]    hello", "30/08/2018 08:51:42.712 [TRACE]    hello", "[DATAIO][b][c] world", null)]
        [TestCase("Timestamp Columnizer", "30/08/2018 08:51:42.712 no bracket 1", "30/08/2018 08:51:42.712 no bracket 2", "30/08/2018 08:51:42.712 [TRACE]    with bracket 1", "30/08/2018 08:51:42.712 [TRACE]    with bracket 2", "no bracket 3")]
        public void FindColumnizer_ReturnCorrectColumnizer(string expectedColumnizerName, string line0, string line1, string line2, string line3, string line4)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test");

            Mock<IAutoLogLineColumnizerCallback> autoLogLineColumnizerCallbackMock = new Mock<IAutoLogLineColumnizerCallback>();

            autoLogLineColumnizerCallbackMock.Setup(a => a.GetLogLine(0)).Returns(new TestLogLine()
            {
                FullLine = line0,
                LineNumber = 0
            });

            autoLogLineColumnizerCallbackMock.Setup(a => a.GetLogLine(1)).Returns(new TestLogLine()
            {
                FullLine = line1,
                LineNumber = 1
            });

            autoLogLineColumnizerCallbackMock.Setup(a => a.GetLogLine(2)).Returns(new TestLogLine()
            {
                FullLine = line2,
                LineNumber = 2
            });

            autoLogLineColumnizerCallbackMock.Setup(a => a.GetLogLine(3)).Returns(new TestLogLine()
            {
                FullLine = line3,
                LineNumber = 3
            });
            autoLogLineColumnizerCallbackMock.Setup(a => a.GetLogLine(4)).Returns(new TestLogLine()
            {
                FullLine = line4,
                LineNumber = 4
            });

            var result = ColumnizerManager.FindColumnizer(path, autoLogLineColumnizerCallbackMock.Object);

            Assert.AreEqual(expectedColumnizerName, result.GetName());
        }

        private class TestLogLine : ILogLine
        {
            public string Text => FullLine;
            public string FullLine { get; set; }
            public int LineNumber { get; set; }
        }
    }
}