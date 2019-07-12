using Codesanook.Configuration.Models;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Email.Models;
using Orchard.Environment;
using System.Linq;

namespace Codesanook.Log4netSmtpAppender
{
    //https://stackoverflow.com/a/9029457/1872200
    public class ShellEvent : IOrchardShellEvents
    {
        private readonly ITransactionManager transactionManager;
        private readonly IOrchardServices orchardServices;

        public ShellEvent(ITransactionManager transactionManager, IOrchardServices orchardServices)
        {
            this.transactionManager = transactionManager;
            this.orchardServices = orchardServices;
        }

        public void Activated() => AddCloudWatchLogAppender();
        public void Terminating() { }

        private void AddCloudWatchLogAppender()
        {
            var hierarchy = ((Hierarchy)LogManager.GetRepository());
            var rootLogger = hierarchy.Root;
            var appender = CreateCloudWatchLogAppender();
            rootLogger.AddAppender(appender);
        }

        private SmtpAppender CreateCloudWatchLogAppender()
        {
            var patternLayout = new PatternLayout
            {
                ConversionPattern =
                    "%utcdate{yyyy-MM-ddTHH:mm:ss.fffZ} [%-5level] %logger - %message%newline" +
                    "%stacktrace{5}%newline"
            };
            patternLayout.ActivateOptions();

            var siteItem = orchardServices.ContentManager.Query(new[] { "Site" }).List().Single();
            var evaluator = new LevelEvaluator()
            {
                Threshold = Level.Warn
            };

            //SMTP
            var smtpSettingsPart = siteItem.As<SmtpSettingsPart>();
            var moduleSettingPart = siteItem.As<ModuleSettingPart>();
            var appender = new SmtpAppender()
            {
                Subject = "System logging email",
                From = smtpSettingsPart.Address,
                To = moduleSettingPart.ExceptionEmailToAddress,

                //server and authentication
                SmtpHost = smtpSettingsPart.Host,
                Port = smtpSettingsPart.Port,
                Authentication  = SmtpAppender.SmtpAuthentication.Basic,
                EnableSsl = smtpSettingsPart.EnableSsl,
                Username = smtpSettingsPart.UserName,
                Password = smtpSettingsPart.Password,
                BufferSize = 1,

                Lossy = false,
                Layout = patternLayout,
            };

            appender.ActivateOptions();
            return appender;
        }
    }
}
