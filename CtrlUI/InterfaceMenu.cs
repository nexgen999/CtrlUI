﻿using ArnoldVinkCode;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static CtrlUI.AppVariables;

namespace CtrlUI
{
    partial class WindowMain
    {
        //Handle main menu keyboard/controller tapped
        async void ListBox_Menu_KeyPressUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Space) { await Listbox_Menu_SingleTap(); }
            }
            catch { }
        }

        //Handle main menu mouse/touch tapped
        async void ListBox_Menu_MousePressUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //Check if an actual ListBoxItem is clicked
                if (!AVFunctions.ListBoxItemClickCheck((DependencyObject)e.OriginalSource)) { return; }

                //Check which mouse button is pressed
                if (e.ClickCount == 1)
                {
                    vSingleTappedEvent = true;
                    await Task.Delay(500);
                    if (vSingleTappedEvent) { await Listbox_Menu_SingleTap(); }
                }
            }
            catch { }
        }

        //Handle main menu single tap
        async Task Listbox_Menu_SingleTap()
        {
            try
            {
                if (listbox_MainMenu.SelectedIndex >= 0)
                {
                    StackPanel SelStackPanel = (StackPanel)listbox_MainMenu.SelectedItem;
                    if (SelStackPanel.Name == "menuButtonFullScreen") { await AppSwitchScreenMode(false, false); }
                    else if (SelStackPanel.Name == "menuButtonMoveMonitor") { await AppMoveMonitor(); }
                    else if (SelStackPanel.Name == "menuButtonSwitchMonitor") { await SwitchDisplayMonitor(); }
                    else if (SelStackPanel.Name == "menuButtonWindowsStart") { ShowWindowStartMenu(); }
                    else if (SelStackPanel.Name == "menuButtonSearch") { await Popup_ShowHide_Search(true); }
                    else if (SelStackPanel.Name == "menuButtonSorting") { SortAppLists(false, false); }
                    else if (SelStackPanel.Name == "menuButtonMediaControl") { await Popup_Show(grid_Popup_Media, grid_Popup_Media_PlayPause); }
                    else if (SelStackPanel.Name == "menuButtonAudioDevice") { await SwitchAudioDevice(); }
                    else if (SelStackPanel.Name == "menuButtonRunExe") { await RunExecutableFile(); }
                    else if (SelStackPanel.Name == "menuButtonRunApp") { await RunUwpApplication(); }
                    else if (SelStackPanel.Name == "menuButtonAddExe") { await Popup_Show_AddExe(); }
                    else if (SelStackPanel.Name == "menuButtonAddApp") { await Popup_Show_AddStoreApp(); }
                    else if (SelStackPanel.Name == "menuButtonFps") { await CloseShowFpsOverlayer(); }
                    else if (SelStackPanel.Name == "menuButtonSettings") { await ShowLoadSettingsPopup(); }
                    else if (SelStackPanel.Name == "menuButtonHelp") { await Popup_Show(grid_Popup_Help, btn_Help_Focus); }
                    else if (SelStackPanel.Name == "menuButtonCloseLaunchers") { await CloseLaunchers(false); }
                    else if (SelStackPanel.Name == "menuButtonDisconnect") { await CloseStreamers(); }
                    else if (SelStackPanel.Name == "menuButtonShutdown") { await Application_Exit_Prompt(); }
                    else if (SelStackPanel.Name == "menuButtonShowFileManager") { await ShowFileManager(); }
                    else if (SelStackPanel.Name == "menuButtonProfileManager") { await Popup_Show_ProfileManager(); }
                    else if (SelStackPanel.Name == "menuButtonEmptyRecycleBin") { await RecycleBin_Empty(); }
                }
            }
            catch { }
        }
    }
}