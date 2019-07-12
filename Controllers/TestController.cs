using Codesanook.Common.Modules;
using Orchard.Logging;
using System;
using System.Web.Mvc;

namespace Codesanook.Log4netSmtpAppender.Controllers
{
    public class TestController : Controller
    {
        public ILogger Logger { get; set; }
        public TestController() => Logger = NullLogger.Instance;

        public ActionResult Index()
        {
            try
            {
                throw new InvalidOperationException("test exception");
            }
            catch (Exception ex)
            {
                var moduleName = ModuleHelper.GetModuleName<TestController>();
                Logger.Error(ex, $"Error from {moduleName} {ex.StackTrace}");
                return Content("log message sent to CloudWatch Logs");
            }
        }
    }
}
