using System.Globalization;

namespace CarCustomChecker.Converters;

/// <summary>True when the bound value is not null.</summary>
public class IsNotNullConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value != null;

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		throw new NotSupportedException();
}

/// <summary>True when the bound value is a non-blank string.</summary>
public class IsNotEmptyConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		!string.IsNullOrWhiteSpace(value?.ToString());

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		throw new NotSupportedException();
}
