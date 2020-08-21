
using MetroLog;
using MetroLog.Targets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{ 
    [TestClass]
    public class UnitTest
    {
        [AssemblyInitialize()]
        public static void Initialization(TestContext testContext)
        {
            LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget());
            GlobalCrashHandler.Configure();
        }
    }
}
