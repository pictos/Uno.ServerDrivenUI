using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Xml;
using static System.Net.WebRequestMethods;

namespace SampleAPI;

public static class XamlApis
{
	public static void MapXamlEndpoints(this IEndpointRouteBuilder app)
	{
		var endpoints = app.MapGroup("/xaml");

		endpoints.MapGet("/string", GetXamlFromString);
	}
	const string defaultXmlns = "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"";
	const string txtBoxXaml =
$@"
	<TextBox
	{defaultXmlns}
	IsEnabled=""False""
	FontSize=""48""
	Text=""Disabled On DrivenUI! Final Test""/>
";

	static IResult GetXamlFromString(HttpContext context)
	{
		context.Response.Headers.Append("CreateDateTime", DateTime.Now.Ticks.ToString());
		context.Response.Headers.Append("xName", "mainContent");
		var content = new StringContent(txtBoxXaml, Encoding.ASCII, "application/xml");

		return Results.Content(txtBoxXaml, "text/plain", Encoding.UTF8);
	}
}
