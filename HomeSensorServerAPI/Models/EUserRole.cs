﻿namespace HomeSensorServerAPI.Models
{
    //order of these is important!
    public enum EUserRole : int
    {
        Sensor = 0,
        Viewer = 1,
        Manager = 2,
        Admin = 3
    }
}
