using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uno.ServerDrivenUI.Services;

namespace Uno.ServerDrivenUI;

public partial class DrivenUIElement : ContentControl
{
	public Status LoadStatus
	{
		get => (Status)GetValue(LoadStatusProperty);
		set => SetValue(LoadStatusProperty, value);
	}

	public static readonly DependencyProperty LoadStatusProperty =
		DependencyProperty.Register(nameof(LoadStatus), typeof(Status), typeof(DrivenUIElement), new PropertyMetadata(Status.Default));

	public DataTemplate DefaultTemplate
	{
		get => (DataTemplate)GetValue(LoadingTemplateProperty);
		set => SetValue(LoadingTemplateProperty, value);
	}

	// Using a DependencyProperty as the backing store for LoadingTemplate.  This enables animation, styling, binding, etc...
	public static readonly DependencyProperty LoadingTemplateProperty =
		DependencyProperty.Register(nameof(DefaultTemplate), typeof(DataTemplate), typeof(DrivenUIElement), new PropertyMetadata(null, OnTemplateChanged));

	static void OnTemplateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
	{
		var dp = (ContentControl)dependencyObject;
		dp.ContentTemplate = (DataTemplate)args.NewValue;
	}

	public DrivenUIElement()
	{
		this.Content = DefaultTemplate;

		Loaded += static async (s, e) =>
		{
			var dui = (DrivenUIElement)s;
			await ServiceUI.Current.ProcessXaml(dui);
		};
	}

	internal void LoadContent(object xaml)
	{
		this.ContentTemplate = null;
		this.Content = xaml;
	}
}

public enum Status
{
	Default,
	Success,
	Error
}
