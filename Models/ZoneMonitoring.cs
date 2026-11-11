using Newtonsoft.Json;

namespace CarCustomChecker.Models;

/// <summary>Live electronic queue of one waiting zone, from belarusborder.by/info/monitoring-new.</summary>
public class ZoneMonitoring
{
	[JsonProperty("info")] public ZoneInfo? Info { get; set; }

	[JsonProperty("carLiveQueue")] public List<ZoneQueueEntry> CarLiveQueue { get; set; } = [];
	[JsonProperty("carPriority")] public List<ZoneQueueEntry> CarPriority { get; set; } = [];

	[JsonProperty("truckLiveQueue")] public List<ZoneQueueEntry> TruckLiveQueue { get; set; } = [];
	[JsonProperty("truckPriority")] public List<ZoneQueueEntry> TruckPriority { get; set; } = [];
	[JsonProperty("truckGpk")] public List<ZoneQueueEntry> TruckGpk { get; set; } = [];

	[JsonProperty("busLiveQueue")] public List<ZoneQueueEntry> BusLiveQueue { get; set; } = [];
	[JsonProperty("busPriority")] public List<ZoneQueueEntry> BusPriority { get; set; } = [];
	[JsonProperty("busRegular")] public List<ZoneQueueEntry> BusRegular { get; set; } = [];

	[JsonProperty("motorcycleLiveQueue")] public List<ZoneQueueEntry> MotorcycleLiveQueue { get; set; } = [];
	[JsonProperty("motorcyclePriority")] public List<ZoneQueueEntry> MotorcyclePriority { get; set; } = [];

	/// <summary>All queues for one vehicle type: priority first, then the others.</summary>
	public IEnumerable<ZoneQueueEntry> GetQueue(ZoneVehicleType type) => type switch
	{
		ZoneVehicleType.Car => CarPriority.Concat(CarLiveQueue),
		ZoneVehicleType.Truck => TruckPriority.Concat(TruckLiveQueue).Concat(TruckGpk),
		ZoneVehicleType.Bus => BusPriority.Concat(BusLiveQueue).Concat(BusRegular),
		ZoneVehicleType.Motorcycle => MotorcyclePriority.Concat(MotorcycleLiveQueue),
		_ => []
	};
}

public enum ZoneVehicleType
{
	Car,
	Truck,
	Bus,
	Motorcycle
}

public record ZoneVehicleTypeOption(string Name, ZoneVehicleType Type);

public class ZoneInfo
{
	[JsonProperty("id")] public string? Id { get; set; }
	[JsonProperty("name")] public string? Name { get; set; }
	[JsonProperty("address")] public string? Address { get; set; }
	[JsonProperty("phone")] public string? Phone { get; set; }
}

/// <summary>One vehicle in a waiting-zone queue. Labels match mon.declarant.by/#/zone.</summary>
public class ZoneQueueEntry
{
	[JsonProperty("regnum")] public string? RegNum { get; set; }
	[JsonProperty("status")] public int Status { get; set; }
	[JsonProperty("order_id")] public int? OrderId { get; set; }
	[JsonProperty("type_queue")] public int TypeQueue { get; set; }
	[JsonProperty("registration_date")] public string? RegistrationDate { get; set; }
	[JsonProperty("changed_date")] public string? ChangedDate { get; set; }

	public string StatusText => Status switch
	{
		2 => "Прибыл в ЗО",
		3 => "Вызван в ПП",
		9 => "Аннулирован",
		12 => "Прибыл рейсовый автобус",
		_ => string.Empty
	};

	public Color StatusColor => Status switch
	{
		2 or 12 => Colors.Green,
		3 => Colors.Orange,
		9 => Colors.Red,
		_ => Colors.Transparent
	};

	public string QueueTypeText => TypeQueue switch
	{
		1 => "Приоритет",
		2 => "Бронь",
		3 => "Живая очередь",
		4 => "Рейсовый автобус",
		_ => string.Empty
	};
}
