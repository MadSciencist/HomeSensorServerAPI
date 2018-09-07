namespace HomeSensorServerAPI.Models
{
    public interface ILoginCredentials
    {
        string Password { get; set; }
        string Username { get; set; }
    }
}