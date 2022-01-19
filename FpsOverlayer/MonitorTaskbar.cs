﻿using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVDisplayMonitor;
using static FpsOverlayer.AppTasks;
using static FpsOverlayer.AppVariables;
using static LibraryShared.Settings;

namespace FpsOverlayer
{
    public partial class WindowMain
    {
        void StartMonitorTaskbar()
        {
            try
            {
                AVActions.TaskStartLoop(LoopMonitorTaskbar, vTask_MonitorTaskbar);
                Debug.WriteLine("Started monitoring taskbar.");
            }
            catch { }
        }

        async Task LoopMonitorTaskbar()
        {
            try
            {
                while (!vTask_MonitorTaskbar.TaskStopRequest)
                {
                    try
                    {
                        //Check taskbar visibility
                        TaskbarInformation taskbarInfo = new TaskbarInformation();

                        //Check if auto hide is enabled
                        if (taskbarInfo.IsAutoHide && taskbarInfo.IsVisible)
                        {
                            //Get the current active screen
                            int monitorNumber = Convert.ToInt32(Setting_Load(vConfigurationCtrlUI, "DisplayMonitor"));
                            DisplayMonitor displayMonitorSettings = GetSingleMonitorEnumDisplay(monitorNumber);

                            //Get the current taskbar size
                            int taskbarSize = (int)(taskbarInfo.Bounds.Height / displayMonitorSettings.DpiScaleVertical);

                            //Check the taskbar margin
                            if (vTaskBarAdjustMargin != taskbarSize)
                            {
                                vTaskBarAdjustMargin = taskbarSize;

                                //Update the fps overlay position
                                await UpdateFpsOverlayPosition(vTargetProcess.Name);
                            }
                        }
                        else
                        {
                            //Check the taskbar margin
                            if (vTaskBarAdjustMargin != 0)
                            {
                                vTaskBarAdjustMargin = 0;

                                //Update the fps overlay position
                                await UpdateFpsOverlayPosition(vTargetProcess.Name);
                            }
                        }
                    }
                    catch { }
                    finally
                    {
                        //Delay the loop task
                        await TaskDelayLoop(1000, vTask_MonitorTaskbar);
                    }
                }
            }
            catch { }
        }
    }
}