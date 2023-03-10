﻿using ArnoldVinkCode;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using static ArnoldVinkCode.AVProcess;
using static LibraryShared.Classes;

namespace CtrlUI
{
    partial class WindowMain
    {
        //Close single process Win32 and Win32Store
        async Task<bool> CloseSingleProcessWin32AndWin32Store(DataBindApp dataBindApp, ProcessMulti processMulti, bool resetProcess, bool removeProcess)
        {
            try
            {
                await Notification_Send_Status("AppClose", "Closing " + dataBindApp.Name);
                Debug.WriteLine("Closing Win32 and Win32Store process: " + dataBindApp.Name);

                //Close the process
                bool closedProcess = false;
                if (processMulti.Identifier > 0)
                {
                    closedProcess = AVProcess.Close_ProcessTreeByProcessId(processMulti.Identifier);
                }
                else if (!string.IsNullOrWhiteSpace(dataBindApp.NameExe))
                {
                    closedProcess = AVProcess.Close_ProcessesByName(dataBindApp.NameExe, true);
                }
                else
                {
                    closedProcess = AVProcess.Close_ProcessesByExecutablePath(dataBindApp.PathExe);
                }

                //Check if process closed
                if (closedProcess)
                {
                    await Notification_Send_Status("AppClose", "Closed " + dataBindApp.Name);
                    Debug.WriteLine("Closed Win32 and Win32Store process: " + dataBindApp.Name);

                    //Reset the process running status
                    if (resetProcess)
                    {
                        dataBindApp.StatusRunning = Visibility.Collapsed;
                        dataBindApp.StatusSuspended = Visibility.Collapsed;
                        dataBindApp.RunningProcessCount = string.Empty;
                        dataBindApp.RunningTimeLastUpdate = 0;
                        dataBindApp.ProcessMulti.Clear();
                    }

                    //Remove the process from the list
                    if (removeProcess)
                    {
                        await RemoveAppFromList(dataBindApp, false, false, true);
                    }

                    return true;
                }
                else
                {
                    await Notification_Send_Status("AppClose", "Failed to close application");
                    Debug.WriteLine("Failed to close the application.");
                    return false;
                }
            }
            catch { }
            return false;
        }

        //Close all processes Win32 and Win32Store
        async Task<bool> CloseAllProcessesWin32AndWin32Store(DataBindApp dataBindApp, bool resetProcess, bool removeProcess)
        {
            try
            {
                await Notification_Send_Status("AppClose", "Closing " + dataBindApp.Name);
                Debug.WriteLine("Closing all Win32 and Win32Store processes: " + dataBindApp.Name);

                //Close the processes by id or name
                bool closedProcess = false;
                foreach (ProcessMulti processMulti in dataBindApp.ProcessMulti)
                {
                    if (processMulti.Identifier > 0)
                    {
                        closedProcess = AVProcess.Close_ProcessTreeByProcessId(processMulti.Identifier);
                    }
                    else if (!string.IsNullOrWhiteSpace(dataBindApp.NameExe))
                    {
                        closedProcess = AVProcess.Close_ProcessesByName(dataBindApp.NameExe, true);
                    }
                    else
                    {
                        closedProcess = AVProcess.Close_ProcessesByExecutablePath(dataBindApp.PathExe);
                    }
                }

                //Check if process closed
                if (closedProcess)
                {
                    await Notification_Send_Status("AppClose", "Closed " + dataBindApp.Name);
                    Debug.WriteLine("Closed all Win32 and Win32Store processes: " + dataBindApp.Name);

                    //Reset the process running status
                    if (resetProcess)
                    {
                        dataBindApp.StatusRunning = Visibility.Collapsed;
                        dataBindApp.StatusSuspended = Visibility.Collapsed;
                        dataBindApp.RunningProcessCount = string.Empty;
                        dataBindApp.RunningTimeLastUpdate = 0;
                        dataBindApp.ProcessMulti.Clear();
                    }

                    //Remove the process from the list
                    if (removeProcess)
                    {
                        await RemoveAppFromList(dataBindApp, false, false, true);
                    }

                    return true;
                }
                else
                {
                    await Notification_Send_Status("AppClose", "Failed to close application");
                    Debug.WriteLine("Failed to close the application.");
                    return false;
                }
            }
            catch { }
            return false;
        }
    }
}