﻿using System;
using System.Diagnostics;

namespace MovistarPlus.Common.Interface
{
    public interface IComunicate
    {
        void Debug(string message, StackTrace stackTrace);
        void Info(string message, string title);
        void Info(string message, StackTrace stackTrace);
        void Warn(string message, string title);
        void Warn(string message, StackTrace stackTrace);
        void Error(string message, StackTrace stackTrace);
        void Error(string message, Exception ex);
        void Fatal(string message, StackTrace stackTrace);
        void Fatal(string message, Exception ex);
        void Warn(string v, Exception e);
    }
}