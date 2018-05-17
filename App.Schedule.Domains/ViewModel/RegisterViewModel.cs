
namespace App.Schedule.Domains.ViewModel
{
    /// <summary>
    /// Class is used to hold register information.
    /// </summary>
    public class RegisterViewModel
    {
        public BusinessViewModel Business { get; set; }
        public BusinessEmployeeViewModel Employee { get; set; }
    }

    public class RegisterCustomerViewModel
    {
        public BusinessCustomerViewMdoel Customer { get; set; }
        public ServiceLocationViewModel ServiceLocation { get; set; }
        public BusinessViewModel Business { get; set; }
    }
}
