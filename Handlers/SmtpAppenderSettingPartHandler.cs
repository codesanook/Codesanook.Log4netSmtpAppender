using Codesanook.Common.Models;
using Codesanook.Log4netSmtpAppender.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;

namespace Codesanook.Log4netSmtpAppender.Handlers {
    public class SmtpAppenderSettingPartHandler : ContentHandler {
        private const string groupId = "SMTP appender settings";

        public Localizer T { get; set; }

        public SmtpAppenderSettingPartHandler() {
            T = NullLocalizer.Instance;

            // Attach a part to the content item Site
            Filters.Add(new ActivatingFilter<SmtpAppenderSettingPart>("Site"));

            // Set a view for a content part 
            Filters.Add(new TemplateFilterForPart<SmtpAppenderSettingPart>(
               prefix: "SmtpAppenderSetting",
               templateName: "Parts/SmtpAppenderSetting", // Part in EditorTemplates
               groupId: groupId // Same value as a parameter of GroupInfo but ignore case
            ));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site") {
                return;
            }

            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T(groupId)));
        }
    }
}
