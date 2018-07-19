namespace HomeSensorServerAPI.Models
{
    //order of these is important!
    //higher number -> more privilledges
    public enum EUserRole : int
    {
        Sensor = 0,
        Viewer,
        Manager,
        Admin
    }
}
