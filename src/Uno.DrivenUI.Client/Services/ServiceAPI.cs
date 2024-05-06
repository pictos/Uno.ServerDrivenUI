using System.Text;
using Windows.System.RemoteSystems;

namespace Uno.ServerDrivenUI.Services;
public sealed class ServiceAPI
{
	static readonly Lazy<ServiceAPI> serviceAPI = new(() => new());

	public static ServiceAPI Current => serviceAPI.Value;

	StorageService StorageService => StorageService.Current;

	HttpClient httpClient = new();

	ServiceAPI() { }

	public void SetHttpClient(HttpClient httpClient)
	{
		this.httpClient = httpClient;
	}

	public void ConfigureHttpClient(Action<HttpClient> action)
	{
		ArgumentNullException.ThrowIfNull(action);
		action(httpClient);
	}

	public async Task PopulateXamlFromApi(string url)
	{
		var response = await httpClient.GetAsync(url).ConfigureAwait(false);
		response.EnsureSuccessStatusCode();
		response.Content.Headers.ContentType!.CharSet = "utf-8";
		var xName = response.Headers.GetValues("xName").First();
		var createdTime = response.Headers.GetValues("CreateDateTime").First();
		var createdTimeTicks = long.Parse(createdTime);

		var xaml = await response.Content.ReadAsStringAsync();

		XamlInfo info = new(new(createdTimeTicks), xName, xaml);

		await StorageService.CreateFile(info);
	}

	public async Task PopulateXamlFromApi(params string[] urls)
	{
		var tasks = new List<Task>(urls.Length);
		foreach (var url in urls)
		{
			tasks.Add(PopulateXamlFromApi(url));
		}

		await Task.WhenAll(tasks).ConfigureAwait(false);
	}

}
