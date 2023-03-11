﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using static ArnoldVinkCode.AVImage;
using static ArnoldVinkCode.AVProcess;
using static CtrlUI.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.Enums;
using static LibraryShared.FocusFunctions;

namespace CtrlUI
{
    partial class WindowMain
    {
        async Task RightClickApplication(ListBox listboxSender, int listboxSelectedIndex, DataBindApp dataBindApp)
        {
            try
            {
                Debug.WriteLine("Right clicked application: " + dataBindApp.Name + " from: " + listboxSender.Name);

                //Show the messagebox popup with options
                List<DataBindString> Answers = new List<DataBindString>();

                DataBindString AnswerHowLongToBeat = new DataBindString();
                if (dataBindApp.Category == AppCategory.Game)
                {
                    AnswerHowLongToBeat.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Default/Icons/Timer.png" }, null, vImageBackupSource, IntPtr.Zero, -1, 0);
                    AnswerHowLongToBeat.Name = "How long to beat information";
                    Answers.Add(AnswerHowLongToBeat);
                }

                DataBindString AnswerEdit = new DataBindString();
                AnswerEdit.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Default/Icons/Edit.png" }, null, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerEdit.Name = "Edit this application details";
                Answers.Add(AnswerEdit);

                DataBindString AnswerMove = new DataBindString();
                AnswerMove.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Default/Icons/Move.png" }, null, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerMove.Name = "Move application position in list";
                Answers.Add(AnswerMove);

                DataBindString AnswerRemove = new DataBindString();
                AnswerRemove.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Default/Icons/Remove.png" }, null, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerRemove.Name = "Remove application from list";
                Answers.Add(AnswerRemove);

                DataBindString AnswerAddExe = new DataBindString();
                if (dataBindApp.Category == AppCategory.App || dataBindApp.Category == AppCategory.Game || dataBindApp.Category == AppCategory.Emulator)
                {
                    AnswerAddExe.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Default/Icons/AppAddExe.png" }, null, vImageBackupSource, IntPtr.Zero, -1, 0);
                    AnswerAddExe.Name = "Add executable application to list";
                    Answers.Add(AnswerAddExe);
                }

                DataBindString AnswerAddStore = new DataBindString();
                if (dataBindApp.Category == AppCategory.App || dataBindApp.Category == AppCategory.Game || dataBindApp.Category == AppCategory.Emulator)
                {
                    AnswerAddStore.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Default/Icons/AppAddStore.png" }, null, vImageBackupSource, IntPtr.Zero, -1, 0);
                    AnswerAddStore.Name = "Add Windows store application to list";
                    Answers.Add(AnswerAddStore);
                }

                //Get launch information
                string launchInformation = string.Empty;
                if (dataBindApp.Type == ProcessType.UWP || dataBindApp.Type == ProcessType.Win32Store)
                {
                    launchInformation = dataBindApp.AppUserModelId;
                }
                else
                {
                    launchInformation = dataBindApp.PathExe;
                }

                //Add launch argument
                if (!string.IsNullOrWhiteSpace(dataBindApp.Argument))
                {
                    launchInformation += "\nLaunch argument: " + dataBindApp.Argument;
                }

                //Get process running time
                string processRunningTimeString = ApplicationRunningTimeString(dataBindApp.RunningTime, "Application");
                if (!string.IsNullOrWhiteSpace(processRunningTimeString))
                {
                    launchInformation += "\n" + processRunningTimeString;
                }

                //Get process last launch time
                string lastLaunchTimeString = ApplicationLastLaunchTimeString(dataBindApp.LastLaunch, "Application");
                if (!string.IsNullOrWhiteSpace(lastLaunchTimeString))
                {
                    launchInformation += "\n" + lastLaunchTimeString;
                }

                DataBindString messageResult = await Popup_Show_MessageBox("What would you like to do with " + dataBindApp.Name + "?", launchInformation, "", Answers);
                if (messageResult != null)
                {
                    if (messageResult == AnswerHowLongToBeat)
                    {
                        await Popup_Show_HowLongToBeat(dataBindApp.Name);
                    }
                    else if (messageResult == AnswerEdit)
                    {
                        //Show application edit popup
                        await Popup_Show_AppEdit(dataBindApp);
                    }
                    else if (messageResult == AnswerRemove)
                    {
                        //Remove application from the list
                        await RemoveAppFromList(dataBindApp, true, true, false);

                        //Select the previous index
                        await ListboxFocusIndex(listboxSender, false, false, listboxSelectedIndex, vProcessCurrent.WindowHandleMain);
                    }
                    else if (messageResult == AnswerMove)
                    {
                        //Show application move popup
                        await Popup_Show_AppMove(dataBindApp);
                    }
                    else if (messageResult == AnswerAddExe)
                    {
                        //Show application add popup
                        await Popup_Show_AddExe();
                    }
                    else if (messageResult == AnswerAddStore)
                    {
                        //Show application add popup
                        await Popup_Show_AddStore();
                    }
                }
            }
            catch { }
        }
    }
}