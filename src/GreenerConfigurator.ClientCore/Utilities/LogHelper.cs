using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenerConfigurator.ClientCore.Utilities
{
    public static class LogHelper
    {
        public static void Log(string message)
        {
            Debug.WriteLine(message);
        }

        public static void LogError(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
