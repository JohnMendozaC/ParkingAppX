namespace ParkingAppX.Droid.ViewModels.Response
{
    public class ResourceData<T>
    {
        public int _status { get; set; }
        public T _data { get; set; }
        public int? _code { get; set; }
        public string _message { get; set; }
    }
}
