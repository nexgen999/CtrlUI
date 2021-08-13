﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using static ArnoldVinkCode.AVImage;
using static CtrlUI.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.FocusFunctions;
using static LibraryShared.JsonFunctions;

namespace CtrlUI
{
    partial class WindowMain
    {
        async Task RightClickLauncher(ListBox listboxSender, int listboxSelectedIndex, DataBindApp dataBindApp)
        {
            try
            {
                List<DataBindString> Answers = new List<DataBindString>();

                DataBindString AnswerDownload = new DataBindString();
                AnswerDownload.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Default/Icons/Download.png" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerDownload.Name = "Download game information";
                Answers.Add(AnswerDownload);

                DataBindString AnswerHide = new DataBindString();
                AnswerHide.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Default/Icons/Hide.png" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerHide.Name = "Hide the launcher application";
                Answers.Add(AnswerHide);

                //Get launcher application information
                string launcherInfoString = dataBindApp.PathExe;
                if (!string.IsNullOrWhiteSpace(dataBindApp.Argument))
                {
                    launcherInfoString += " (" + dataBindApp.Argument + ")";
                }

                DataBindString messageResult = await Popup_Show_MessageBox("What would you like to do with " + dataBindApp.Name + "?", launcherInfoString, "", Answers);
                if (messageResult != null)
                {
                    if (messageResult == AnswerDownload)
                    {
                        DownloadInfoGame informationDownloaded = await DownloadInfoGame(dataBindApp.Name, 100, true);
                        if (informationDownloaded != null)
                        {
                            if (informationDownloaded.ImageBitmap != null)
                            {
                                dataBindApp.ImageBitmap = informationDownloaded.ImageBitmap;
                            }
                        }
                    }
                    else if (messageResult == AnswerHide)
                    {
                        await HideLauncherApp(listboxSender, listboxSelectedIndex, dataBindApp);
                    }
                }
            }
            catch { }
        }

        //Hide the launcher app
        async Task HideLauncherApp(ListBox listboxSender, int listboxSelectedIndex, DataBindApp dataBindApp)
        {
            try
            {
                await Notification_Send_Status("Hide", "Hiding launcher " + dataBindApp.Name);
                Debug.WriteLine("Hiding launcher by name: " + dataBindApp.Name);

                //Create new profile shared
                ProfileShared profileShared = new ProfileShared();
                profileShared.String1 = dataBindApp.Name;

                //Add shortcut file to the ignore list
                vCtrlIgnoreLauncherName.Add(profileShared);
                JsonSaveObject(vCtrlIgnoreLauncherName, @"User\CtrlIgnoreLauncherName");

                //Remove application from the list
                await RemoveAppFromList(dataBindApp, false, false, true);

                //Select the previous index
                await ListboxFocusIndex(listboxSender, false, false, listboxSelectedIndex, vProcessCurrent.MainWindowHandle);
            }
            catch (Exception ex)
            {
                await Notification_Send_Status("Hide", "Failed hiding");
                Debug.WriteLine("Failed hiding launcher: " + ex.Message);
            }
        }
    }
}