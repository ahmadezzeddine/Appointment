namespace App.Schedule.Domains.ViewModel
{
    public class ServiceDataViewModel<T>
    {
        public bool HasError { get; set; }
        public string Error { get; set; }
        public bool HasMore { get; set; }
        public string ErrorDescription { get; set; }
        public T Data { get; set; }
    }
}
