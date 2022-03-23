namespace Shared.Dtos.Requests
{
    public class UserRequest
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public string Smartphone { get; set; }
        public string AndroidVersion { get; set; }
    }
}
