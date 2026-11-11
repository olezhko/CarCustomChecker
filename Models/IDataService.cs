namespace CarCustomChecker.Models;
internal interface IDataService
{
	Task<Root> GetCarItems(int page, int perPage, string ptoCode, string search);

	Task<ElectronicQueueResult> GetElectronicQueueResult();

	Task<ZoneMonitoring> GetZoneMonitoring(string checkpointId);

	Task<IReadOnlyList<CustomsOffice>> GetCustomsOffices(int placeKind);

	Task<CustomsPlace> GetCustomsPlace(int id);

	Task<PagedResult<VptoDeclaration>> SearchVpto(int placeId, string regNumber, int page);

	Task<PagedResult<SvhEntry>> SearchSvh(int placeId, string regNum, string transport, string numItem, int page);

	Task<IReadOnlyList<RptoOffice>> GetRptoOffices();

	Task<RptoPlace> GetRptoPlace(int id);

	Task<PagedResult<RptoVehicle>> GetRptoVehicles(int id, string search, int page);
}