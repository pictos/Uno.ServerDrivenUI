using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.ServerDrivenUI;
using Uno.ServerDrivenUI.Services;

namespace UnitTests;

[TestClass]
public class Tests
{
	public void Setup()
	{
	}

	[TestMethod]
	public async Task Test1()
	{
		var sut = new DrivenUIElement()
		{
			Name = "batata"
		};

		await ServiceUI.Current.GetAndProcessXaml();

		Assert.IsTrue(true);
	}
}