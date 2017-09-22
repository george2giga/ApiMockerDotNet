﻿using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiMockerDotNet.UnitTests.Repositories
{
    public static class LoggerUtils
    {
        public static Mock<ILogger<T>> LoggerMock<T>() where T : class
        {
            return new Mock<ILogger<T>>();
        }

        /// <summary>
        /// Returns an <pre>ILogger<T></pre> as used by the Microsoft.Logging framework.
        /// You can use this for constructors that require an ILogger parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ILogger<T> Logger<T>() where T : class
        {
            return LoggerMock<T>().Object;
        }

        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, Func<string, bool> filteredMessage, string failMessage = null)
        {
            loggerMock.VerifyLog(level, filteredMessage, Times.Once(), failMessage);
        }
        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, Func<string, bool> filteredMessage, Times times, string failMessage = null)
        {
            loggerMock.Verify(l => l.Log<Object>(level, It.IsAny<EventId>(), It.Is<Object>(o =>  filteredMessage(o.ToString())), null, It.IsAny<Func<Object, Exception, String>>()), times, failMessage);
        }

    }
}