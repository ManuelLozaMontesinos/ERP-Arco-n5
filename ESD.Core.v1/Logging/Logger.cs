using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ESD.Core.Logging
{
   internal class LoggerConstant {
      public static readonly string InfoStart = "Start Execution!";
      public static readonly string InfoFinish = "Execution finished!";
   }
   public static class Logger
   {
      private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

      public static void Info(string message)
      {
         settingMappedDiagnosticsLogicalContext();
         _logger.Info(message);
      }
      public static void Info(string message, params object[] args)
      {
         settingMappedDiagnosticsLogicalContext();
         _logger.Info(message, args);
      }
      public static void InfoStart()
      {
         settingMappedDiagnosticsLogicalContext();
         _logger.Info(LoggerConstant.InfoStart);
      }
      public static void InfoFinish()
      {
         settingMappedDiagnosticsLogicalContext();
         _logger.Info(LoggerConstant.InfoFinish);
      }
      public static void Error(string message)
      {
         settingMappedDiagnosticsLogicalContext();
         _logger.Error(message);
      }
      public static void Error(Exception ex)
      {
         settingMappedDiagnosticsLogicalContext();
         _logger.Error(ex.ToString());
      }
      public static bool IsInfoEnabled {
         get { return _logger.IsInfoEnabled; }      
      }
      internal static void settingMappedDiagnosticsLogicalContext() {

         MethodBase methodBase = ((IEnumerable<StackFrame>)new StackTrace(0, true).GetFrames()).ElementAt(2).GetMethod();

         if (methodBase == null)
            return;

         if (methodBase.DeclaringType == null)
            return;

         //MappedDiagnosticsLogicalContext.Set("ClassName", methodBase.DeclaringType.FullName.ToString());
         //MappedDiagnosticsLogicalContext.Set("MethodName", methodBase.Name.ToString());
      }
   }
}
