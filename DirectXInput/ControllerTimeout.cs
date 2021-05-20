﻿using System.Diagnostics;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Check if a controller has timed out
        void ControllerTimeout(ControllerStatus Controller)
        {
            try
            {
                //Debug.WriteLine("Checking if controller " + Controller.NumberId + " has timed out for " + Controller.TimeoutSeconds + " seconds.");
                if (Controller.Connected() && Controller.InputReport != null && Controller.LastInputTicks != 0 && Controller.PrevInputTicks != 0)
                {
                    long latencyMs = Controller.LastInputTicks - Controller.PrevInputTicks;
                    if (latencyMs > Controller.MilliSecondsTimeout)
                    {
                        Debug.WriteLine("Controller " + Controller.NumberId + " has timed out, stopping and removing the controller.");
                        StopControllerTask(Controller, "timeout", "Controller " + Controller.NumberId + " has timed out.");
                    }
                }
            }
            catch { }
        }

        //Check for timed out controllers
        void CheckControllersTimeout()
        {
            try
            {
                ControllerTimeout(vController0);
                ControllerTimeout(vController1);
                ControllerTimeout(vController2);
                ControllerTimeout(vController3);
            }
            catch { }
        }
    }
}