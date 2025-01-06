using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Xml;

namespace FotoQuestApi.Web.Diagnostics;

public class XmlHealthCheckOptions : HealthCheckOptions
{
    public XmlHealthCheckOptions() => ResponseWriter = CustomResponseWriter;
    private Task CustomResponseWriter(HttpContext httpContext, HealthReport healthReport)
    {
        var xml = new XmlDocument();
        xml.CreateXmlDeclaration("1.0", "UTF-8", "yes");

        XmlElement root = xml.CreateElement("health");
        root.SetAttribute("host", System.Net.Dns.GetHostName());
        root.SetAttribute("username", Environment.UserDomainName + "\\" + Environment.UserName);
        xml.AppendChild(root);

        XmlElement overallHealth = xml.CreateElement("overall");
        overallHealth.InnerText = healthReport.Status.ToString();
        root.AppendChild(overallHealth);

        XmlElement details = xml.CreateElement("details");
        root.AppendChild(details);

        foreach (var healthReportEntry in healthReport.Entries)
        {
            XmlElement component = xml.CreateElement("component");
            component.SetAttribute("name", healthReportEntry.Key);

            if (!string.IsNullOrEmpty(healthReportEntry.Value.Description))
            {
                component.InnerXml = healthReportEntry.Value.Description;
            }

            details.AppendChild(component);
        }

        httpContext.Response.ContentType = MediaTypeNames.Application.Xml;
        return httpContext.Response.WriteAsync(string.Join("", xml.OuterXml));
    }
}