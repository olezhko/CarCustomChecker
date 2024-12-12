namespace CarCustomChecker.Models;
internal interface IDataService
{
	Task<Root> GetCarItems(int page, int perPage, string ptoCode, string search);

	Task<ElectronicQueueResult> GetElectronicQueueResult();
}