using CarCustomChecker.Models;

namespace CarCustomChecker.ViewModels;

public class CarItemViewModel : BaseViewModel
{
	private FieldValueViewModel<string> _vin = new(string.Empty);
	private FieldValueViewModel<string> _dateIn = new(string.Empty);
	private FieldValueViewModel<string> _timeIn = new(string.Empty);
	private FieldValueViewModel<string> _ptoCode = new(string.Empty);

	public FieldValueViewModel<string> Vin
	{
		get => _vin;
		set => SetProperty(ref _vin, value);
	}

	public FieldValueViewModel<string> DateIn
	{
		get => _dateIn;
		set => SetProperty(ref _dateIn, value);
	}

	public FieldValueViewModel<string> TimeIn
	{
		get => _timeIn;
		set => SetProperty(ref _timeIn, value);
	}

	public FieldValueViewModel<string> PtoCode
	{
		get => _ptoCode;
		set => SetProperty(ref _ptoCode, value);
	}
}
