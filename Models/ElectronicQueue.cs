namespace CarCustomChecker.Models
{
    public class ElectronicQueue : BaseViewModel
    {
        private string name;
        public string Id { get; set; }
        public string Name { get => name; set => SetProperty(ref name, value); }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int CountAll { get; set; }
        public int CountCar { get; set; }
        public int CountTruck { get; set; }
        public int CountBus { get; set; }
        public int CountMotorcycle { get; set; }
        public int CountLiveQueue { get; set; }
        public int CountBookings { get; set; }
        public int CountPriority { get; set; }
        public int CountPassedSCC { get; set; }
    }

    public class ElectronicQueueResult(
        IReadOnlyList<ElectronicQueue> Result
    )
    {
        public IReadOnlyList<ElectronicQueue> Result { get; } = Result;
    }
}
