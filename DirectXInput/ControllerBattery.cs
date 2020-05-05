﻿using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.SoundPlayer;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Read controller battery level
        void ControllerUpdateBatteryLevel(ControllerStatus Controller)
        {
            try
            {
                //Check which controller is connected
                IEnumerable<ControllerSupported> TargetController = vDirectControllersSupported.Where(x => x.ProductIDs.Any(z => z.ToLower() == Controller.Details.Profile.ProductID.ToLower() && x.VendorID.ToLower() == Controller.Details.Profile.VendorID.ToLower()));
                if (TargetController.Any(x => x.CodeName == "SonyDualShock4") && Controller.Details.Wireless)
                {
                    //Bluetooth - DualShock 4
                    int BatteryOffset = 30 + Controller.InputHeaderByteOffset + Controller.InputButtonByteOffset;
                    byte BatteryReport = Controller.InputReport[BatteryOffset];

                    bool BatteryCharging = TranslateByte_0x10(0, BatteryReport) != 0;
                    if (BatteryCharging)
                    {
                        Controller.BatteryPercentageCurrent = -2;
                    }
                    else
                    {
                        int RawBattery = TranslateByte_0x0F(0, BatteryReport) * 10 + 10;
                        if (RawBattery > 100) { RawBattery = 100; }
                        Controller.BatteryPercentageCurrent = RawBattery;
                    }
                }
                else if (TargetController.Any(x => x.CodeName == "SonyDualShock4") && !Controller.Details.Wireless)
                {
                    //Wired USB - DualShock 4
                    Controller.BatteryPercentageCurrent = -2;
                }
                else
                {
                    //Incompatible controllers
                    Controller.BatteryPercentageCurrent = -1;
                }
            }
            catch
            {
                Controller.BatteryPercentageCurrent = -1;
            }
        }

        //Check controller for low battery level
        void ControllerLowBattery(ControllerStatus Controller)
        {
            try
            {
                //Debug.WriteLine("Checking if controller " + Controller.NumberId + " has a low battery level " + Controller.BatteryPercentageCurrent + "/" + Controller.BatteryPercentagePrevious);
                string controllerNumberDisplay = (Controller.NumberId + 1).ToString();

                //Check the controller id
                TextBlock targetControllerTextblock = null;
                StackPanel targetControllerStackpanel = null;
                if (Controller.NumberId == 0)
                {
                    targetControllerTextblock = App.vWindowOverlay.textblock_Battery_Warning_Controller1;
                    targetControllerStackpanel = App.vWindowOverlay.stackpanel_Battery_Warning_Controller1;
                }
                else if (Controller.NumberId == 1)
                {
                    targetControllerTextblock = App.vWindowOverlay.textblock_Battery_Warning_Controller2;
                    targetControllerStackpanel = App.vWindowOverlay.stackpanel_Battery_Warning_Controller2;
                }
                else if (Controller.NumberId == 2)
                {
                    targetControllerTextblock = App.vWindowOverlay.textblock_Battery_Warning_Controller3;
                    targetControllerStackpanel = App.vWindowOverlay.stackpanel_Battery_Warning_Controller3;
                }
                else if (Controller.NumberId == 3)
                {
                    targetControllerTextblock = App.vWindowOverlay.textblock_Battery_Warning_Controller4;
                    targetControllerStackpanel = App.vWindowOverlay.stackpanel_Battery_Warning_Controller4;
                }

                //Check if controller is connected
                if (!Controller.Connected() && Controller.InputReport == null)
                {
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        targetControllerStackpanel.Visibility = Visibility.Collapsed;
                    });
                    return;
                }

                //Check controller battery level overlay
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["BatteryShowIconLow"]) && Controller.BatteryPercentageCurrent <= 20 && Controller.BatteryPercentageCurrent >= 0)
                {
                    Debug.WriteLine("Controller " + Controller.NumberId + " has a low battery level, showing overlay.");
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        App.vWindowOverlay.UpdateBatteryPosition();
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["BatteryShowPercentageLow"]))
                        {
                            targetControllerTextblock.Text = Controller.BatteryPercentageCurrent + "%";
                            targetControllerTextblock.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            targetControllerTextblock.Visibility = Visibility.Collapsed;
                        }
                        targetControllerStackpanel.Visibility = Visibility.Visible;
                    });
                }
                else
                {
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        targetControllerStackpanel.Visibility = Visibility.Collapsed;
                    });
                }

                //Check controller battery level sound and notification
                if (Controller.BatteryPercentageCurrent > 0)
                {
                    if (Controller.BatteryPercentageCurrent <= 10 && (Controller.BatteryPercentagePrevious > 10 || Controller.BatteryPercentagePrevious == -1))
                    {
                        Debug.WriteLine("Controller " + Controller.NumberId + " has a low battery level 10%");

                        NotificationDetails notificationDetails = new NotificationDetails();
                        notificationDetails.Icon = "Battery/BatteryVerDis10";
                        notificationDetails.Text = "Controller (" + controllerNumberDisplay + ") battery " + Controller.BatteryPercentageCurrent + "%";
                        App.vWindowOverlay.Notification_Show_Status(notificationDetails);

                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["BatteryPlaySoundLow"]))
                        {
                            PlayInterfaceSound("BatteryLow", true);
                        }
                    }
                    else if (Controller.BatteryPercentageCurrent <= 20 && (Controller.BatteryPercentagePrevious > 20 || Controller.BatteryPercentagePrevious == -1))
                    {
                        Debug.WriteLine("Controller " + Controller.NumberId + " has a low battery level 20%");

                        NotificationDetails notificationDetails = new NotificationDetails();
                        notificationDetails.Icon = "Battery/BatteryVerDis20";
                        notificationDetails.Text = "Controller (" + controllerNumberDisplay + ") battery " + Controller.BatteryPercentageCurrent + "%";
                        App.vWindowOverlay.Notification_Show_Status(notificationDetails);

                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["BatteryPlaySoundLow"]))
                        {
                            PlayInterfaceSound("BatteryLow", true);
                        }
                    }

                    Controller.BatteryPercentagePrevious = Controller.BatteryPercentageCurrent;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed checking for low battery level: " + ex.Message);
            }
        }

        //Check all controllers for low battery level
        void CheckAllControllersLowBattery()
        {
            try
            {
                ControllerLowBattery(vController0);
                ControllerLowBattery(vController1);
                ControllerLowBattery(vController2);
                ControllerLowBattery(vController3);
            }
            catch { }
        }
    }
}