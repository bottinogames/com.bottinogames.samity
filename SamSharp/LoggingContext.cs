using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace SamSharp
{
    public static class LoggingContext
    {
        public static string CurrentContext { get; private set; } = null;

        public static bool HasActiveContext => CurrentContext != null;

        private static StringBuilder stringBuilder = new StringBuilder();

        public static bool OpenContext(string context)
        {
            if (HasActiveContext)
                return false;

            CurrentContext = context;

            stringBuilder.Clear();
            stringBuilder.AppendLine($"SamSharp Logging ({CurrentContext}):");

            return true;
        }

        public static void CloseAndLogContext()
        {
            if (!HasActiveContext)
                return;

            string output = stringBuilder.ToString();
            Debug.Log(output);

            stringBuilder.Clear();

            CurrentContext = null;
        }

        public static void Log(string line)
        {
            if (!HasActiveContext)
                return;

            stringBuilder.AppendLine(line);
        }
    }
}
