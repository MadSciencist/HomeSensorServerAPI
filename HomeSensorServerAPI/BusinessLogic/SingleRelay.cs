namespace HomeSensorServerAPI.BusinessLogic
{
    public class SingleRelay
    {

        public static bool GetNewState(string value)
        {
            bool isOn = false;

            if (value == Relay1AvailableStates.on.ToString())
            {
                isOn = true;
            }
            else if (value == Relay1AvailableStates.off.ToString())
            {
                isOn = false;
            }

            return isOn;
        }
    }

    public enum Relay1AvailableStates
    {
        on,
        off
    }
}
