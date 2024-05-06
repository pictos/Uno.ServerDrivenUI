using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Search;

namespace Uno.ServerDrivenUI.Services;
sealed class StorageService
{
	static readonly Lazy<StorageService> lazy = new(() => new());

	public static StorageService Current => lazy.Value;

	readonly string xamlFolderPath;

	Dictionary<string, string> loadedXamls = [];

	StorageService()
	{
		var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		xamlFolderPath = Path.Combine(localAppDataPath, "xaml");
		if (!Directory.Exists(xamlFolderPath))
		{
			Directory.CreateDirectory(xamlFolderPath);
		}
	}

	public async ValueTask CreateFile(XamlInfo info)
	{
		var fileName = info.XName;

		var filePath = Path.Combine(xamlFolderPath, fileName);

		if (File.Exists(filePath))
		{
			var creationTime = File.GetCreationTime(filePath);

			if (info.CreatedTime < creationTime)
			{
				return;
			}
		}

		using var fileStream = File.CreateText(filePath);
		await fileStream.WriteAsync(info.Xaml).ConfigureAwait(false);
	}

	public async ValueTask<string> GetFileContent(string xName)
	{
		var filePath = Path.Combine(xamlFolderPath, xName);

		if (!File.Exists(filePath))
		{
			return string.Empty;
		}

		string? xaml = string.Empty;

		if (loadedXamls.TryGetValue(xName, out xaml))
		{
			return xaml;
		}

		using var file = File.OpenText(filePath);

		xaml = await file.ReadToEndAsync().ConfigureAwait(false);

		loadedXamls[xName] = xaml;

		return xaml;
	}
}
