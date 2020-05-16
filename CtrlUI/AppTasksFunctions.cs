﻿using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;
using static CtrlUI.AppVariables;

namespace CtrlUI
{
    public partial class WindowMain
    {
        Task vTaskLoop_UpdateClock()
        {
            try
            {
                while (!vTask_UpdateClock.TaskStopRequest)
                {
                    UpdateClockTime();

                    //Delay the loop task
                    TaskDelayLoop(5000, vTask_UpdateClock);
                }
            }
            catch { }
            return Task.FromResult(0);
        }

        async Task vTaskLoop_UpdateWindowStatus()
        {
            try
            {
                while (!vTask_UpdateWindowStatus.TaskStopRequest)
                {
                    await UpdateWindowStatus();

                    //Delay the loop task
                    TaskDelayLoop(500, vTask_UpdateWindowStatus);
                }
            }
            catch { }
        }

        async Task vTaskLoop_ControllerConnected()
        {
            try
            {
                while (!vTask_ControllerConnected.TaskStopRequest)
                {
                    await UpdateControllerConnected();

                    //Delay the loop task
                    TaskDelayLoop(2000, vTask_ControllerConnected);
                }
            }
            catch { }
        }

        Task vTaskLoop_UpdateAppRunningTime()
        {
            try
            {
                while (!vTask_UpdateAppRunningTime.TaskStopRequest)
                {
                    UpdateAppRunningTime();

                    //Delay the loop task
                    TaskDelayLoop(60000, vTask_UpdateAppRunningTime);
                }
            }
            catch { }
            return Task.FromResult(0);
        }

        async Task vTaskLoop_UpdateMediaInformation()
        {
            try
            {
                while (!vTask_UpdateMediaInformation.TaskStopRequest)
                {
                    await UpdateCurrentMediaInformation();

                    //Delay the loop task
                    TaskDelayLoop(1000, vTask_UpdateMediaInformation);
                }
            }
            catch { }
        }

        async Task vTaskLoop_UpdateProcesses()
        {
            try
            {
                while (!vTask_UpdateProcesses.TaskStopRequest)
                {
                    if (vAppActivated)
                    {
                        await RefreshListProcessesWithWait(false);

                        //Delay the loop task
                        TaskDelayLoop(3000, vTask_UpdateProcesses);
                    }
                    else
                    {
                        //Delay the loop task
                        TaskDelayLoop(500, vTask_UpdateProcesses);
                    }
                }
            }
            catch { }
        }

        async Task vTaskLoop_UpdateShortcuts()
        {
            try
            {
                while (!vTask_UpdateShortcuts.TaskStopRequest)
                {
                    if (vAppActivated)
                    {
                        await RefreshListShortcuts(false);

                        //Delay the loop task
                        TaskDelayLoop(6000, vTask_UpdateShortcuts);
                    }
                    else
                    {
                        //Delay the loop task
                        TaskDelayLoop(500, vTask_UpdateShortcuts);
                    }
                }
            }
            catch { }
        }

        Task vTaskLoop_UpdateListStatus()
        {
            try
            {
                while (!vTask_UpdateListStatus.TaskStopRequest)
                {
                    if (vAppActivated)
                    {
                        RefreshListStatus();

                        //Delay the loop task
                        TaskDelayLoop(2000, vTask_UpdateListStatus);
                    }
                    else
                    {
                        //Delay the loop task
                        TaskDelayLoop(500, vTask_UpdateListStatus);
                    }
                }
            }
            catch { }
            return Task.FromResult(0);
        }

        async Task vTaskLoop_ShowHideMouse()
        {
            try
            {
                while (!vTask_ShowHideMouse.TaskStopRequest)
                {
                    await MouseCursorCheckMovement();

                    //Delay the loop task
                    TaskDelayLoop(3000, vTask_ShowHideMouse);
                }
            }
            catch { }
        }
    }
}