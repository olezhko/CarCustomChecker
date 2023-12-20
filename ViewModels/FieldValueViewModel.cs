using CarCustomChecker.Models;

namespace CarCustomChecker.ViewModels;

public class FieldValueViewModel<T> : BaseViewModel
{
	private T _value;
	private bool _isChanged;

	public T Value
	{
		get => _value;
		set => IsChanged = SetProperty(ref _value, value);
	}

	public bool IsChanged
	{
		get => _isChanged;
		set => SetProperty(ref _isChanged, value);
	}

	public FieldValueViewModel(T value)
	{
		Value = value;
	}
}