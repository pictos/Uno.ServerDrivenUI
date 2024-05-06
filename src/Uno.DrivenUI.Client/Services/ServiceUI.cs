using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;

namespace Uno.ServerDrivenUI.Services;

public sealed class ServiceUI
{
	readonly static Lazy<ServiceUI> serviceUILazy = new(() => new());
	public static ServiceUI Current => serviceUILazy.Value;
	static StorageService StorageService => StorageService.Current;

	ServiceUI() { }

	public ValueTask<string> GetXaml(string xName)
	{
		return StorageService.GetFileContent(xName);
	}

	const string defaultXmlns = "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"";
	const string txtBoxXaml =
$@"
	<TextBox
	{defaultXmlns}
	IsEnabled=""False""
	FontSize=""48""
	Text=""Disabled On DrivenUI""/>
";

	//const string helloWord = "<TextBox xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Text=\"Hello, World!\" />";
	const string helloWord = $"<TextBox {defaultXmlns} Text=\"Hello, World!\" />";


	public async ValueTask ProcessXaml(DrivenUIElement uiElement)
	{
		var x = XamlReader.Load(txtBoxXaml);

		var result = await GetXaml(uiElement.Name);

		var xaml = XamlReader.Load(result);

		if (xaml is null)
		{
			return;
		}

		uiElement.LoadContent(xaml);
	}
}
