﻿using System;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVInputOutputClass;
using static ArnoldVinkCode.AVInputOutputKeyboard;
using static DirectXInput.AppVariables;
using static DirectXInput.WindowMain;
using static LibraryShared.Classes;
using static LibraryShared.Settings;
using static LibraryShared.SoundPlayer;

namespace DirectXInput.MediaCode
{
    partial class WindowMedia
    {
        //Process controller input for mouse
        public void ControllerInteractionMouse(ControllerInput ControllerInput)
        {
            bool ControllerDelayMicro = false;
            bool ControllerDelayShort = false;
            try
            {
                if (GetSystemTicksMs() >= vControllerDelay_Mouse)
                {
                    //Get the mouse move amount
                    int moveSensitivity = Convert.ToInt32(Setting_Load(vConfigurationDirectXInput, "KeyboardMouseMoveSensitivity"));
                    GetMouseMovementAmountFromThumb(moveSensitivity, ControllerInput.ThumbLeftX, ControllerInput.ThumbLeftY, true, out int moveHorizontalLeft, out int moveVerticalLeft);
                    GetMouseMovementAmountFromThumb(moveSensitivity, ControllerInput.ThumbRightX, ControllerInput.ThumbRightY, true, out int moveHorizontalRight, out int moveVerticalRight);

                    //Move the mouse cursor
                    vVirtualHidDevice.movRel(moveHorizontalLeft, moveVerticalLeft);

                    //Move the media window
                    MoveMediaWindow(moveHorizontalRight, moveVerticalRight);

                    //Emulate mouse click left
                    if (ControllerInput.ButtonShoulderLeft.PressedRaw)
                    {
                        if (!vMouseLeftDownStatus)
                        {
                            vMouseLeftDownStatus = true;
                            vVirtualHidDevice.btn(1); //Press left button

                            ControllerDelayMicro = true;
                        }
                    }
                    else if (vMouseLeftDownStatus)
                    {
                        vMouseLeftDownStatus = false;
                        vVirtualHidDevice.btn(2); //Release left button

                        ControllerDelayMicro = true;
                    }

                    //Emulate mouse click right
                    if (ControllerInput.ButtonShoulderRight.PressedRaw)
                    {
                        if (!vMouseRightDownStatus)
                        {
                            vMouseRightDownStatus = true;
                            vVirtualHidDevice.btn(4); //Press right button

                            ControllerDelayMicro = true;
                        }
                    }
                    else if (vMouseRightDownStatus)
                    {
                        vMouseRightDownStatus = false;
                        vVirtualHidDevice.btn(8); //Release right button

                        ControllerDelayMicro = true;
                    }

                    //Delay input to prevent repeat
                    if (ControllerDelayMicro)
                    {
                        vControllerDelay_Mouse = GetSystemTicksMs() + vControllerDelayMicroTicks;
                    }
                    else if (ControllerDelayShort)
                    {
                        vControllerDelay_Mouse = GetSystemTicksMs() + vControllerDelayShortTicks;
                    }
                    else
                    {
                        vControllerDelay_Mouse = GetSystemTicksMs() + vControllerDelayNanoTicks;
                    }
                }
            }
            catch { }
        }

        //Process controller input for keyboard
        public void ControllerInteractionKeyboard(ControllerInput ControllerInput)
        {
            bool ControllerDelayShort = false;
            bool ControllerDelayMedium = false;
            bool ControllerDelayLonger = false;
            try
            {
                if (GetSystemTicksMs() >= vControllerDelay_Media)
                {
                    //Send internal arrow left key
                    if (ControllerInput.DPadLeft.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Move", false);
                        KeySendSingle(KeysVirtual.Left, vInteropWindowHandle);

                        ControllerDelayShort = true;
                    }
                    //Send internal arrow right key
                    else if (ControllerInput.DPadRight.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Move", false);
                        KeySendSingle(KeysVirtual.Right, vInteropWindowHandle);

                        ControllerDelayShort = true;
                    }
                    //Send internal arrow up key
                    else if (ControllerInput.DPadUp.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Move", false);
                        KeySendSingle(KeysVirtual.Up, vInteropWindowHandle);

                        ControllerDelayShort = true;
                    }
                    //Send internal arrow down key
                    else if (ControllerInput.DPadDown.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Move", false);
                        KeySendSingle(KeysVirtual.Down, vInteropWindowHandle);

                        ControllerDelayShort = true;
                    }

                    //Send internal space key
                    else if (ControllerInput.ButtonA.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Click", false);
                        KeySendSingle(KeysVirtual.Space, vInteropWindowHandle);

                        ControllerDelayMedium = true;
                    }
                    //Send external media next key
                    else if (ControllerInput.ButtonB.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Click", false);
                        App.vWindowOverlay.Notification_Show_Status("MediaNext", "Going to next media item");
                        KeyPressSingleAuto(KeysVirtual.MediaNextTrack);

                        ControllerDelayMedium = true;
                    }
                    //Send external media playpause key
                    else if (ControllerInput.ButtonY.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Click", false);
                        App.vWindowOverlay.Notification_Show_Status("MediaPlayPause", "Resuming or pausing media");
                        KeyPressSingleAuto(KeysVirtual.MediaPlayPause);

                        ControllerDelayMedium = true;
                    }
                    //Send external media previous key
                    else if (ControllerInput.ButtonX.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Click", false);
                        App.vWindowOverlay.Notification_Show_Status("MediaPrevious", "Going to previous media item");
                        KeyPressSingleAuto(KeysVirtual.MediaPreviousTrack);

                        ControllerDelayMedium = true;
                    }

                    //Send external arrow keys
                    else if (ControllerInput.ButtonThumbLeft.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Click", false);
                        App.vWindowOverlay.Notification_Show_Status("ArrowLeft", "Moving left");
                        KeyPressSingleAuto(KeysVirtual.Left);

                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.ButtonThumbRight.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Click", false);
                        App.vWindowOverlay.Notification_Show_Status("ArrowRight", "Moving right");
                        KeyPressSingleAuto(KeysVirtual.Right);

                        ControllerDelayShort = true;
                    }

                    //Send external alt+enter keys
                    else if (ControllerInput.ButtonBack.PressedRaw)
                    {
                        PlayInterfaceSound(vConfigurationCtrlUI, "Click", false);
                        App.vWindowOverlay.Notification_Show_Status("MediaFullscreen", "Toggling fullscreen");
                        KeyPressComboAuto(KeysVirtual.Alt, KeysVirtual.Enter);

                        ControllerDelayMedium = true;
                    }

                    //Change the system volume
                    else if (ControllerInput.TriggerLeft > 0 && ControllerInput.TriggerRight > 0)
                    {
                        App.vWindowOverlay.Notification_Show_Status("VolumeMute", "Toggling output mute");
                        KeyPressSingleAuto(KeysVirtual.VolumeMute);

                        ControllerDelayLonger = true;
                    }
                    else if (ControllerInput.ButtonStart.PressedRaw)
                    {
                        App.vWindowOverlay.Notification_Show_Status("VolumeMute", "Toggling output mute");
                        KeyPressSingleAuto(KeysVirtual.VolumeMute);

                        ControllerDelayMedium = true;
                    }
                    else if (ControllerInput.TriggerLeft > 0)
                    {
                        App.vWindowOverlay.Notification_Show_Status("VolumeDown", "Decreasing volume");
                        KeyPressSingleAuto(KeysVirtual.VolumeDown);

                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.TriggerRight > 0)
                    {
                        App.vWindowOverlay.Notification_Show_Status("VolumeUp", "Increasing volume");
                        KeyPressSingleAuto(KeysVirtual.VolumeUp);

                        ControllerDelayShort = true;
                    }

                    //Delay input to prevent repeat
                    if (ControllerDelayShort)
                    {
                        vControllerDelay_Media = GetSystemTicksMs() + vControllerDelayShortTicks;
                    }
                    else if (ControllerDelayMedium)
                    {
                        vControllerDelay_Media = GetSystemTicksMs() + vControllerDelayMediumTicks;
                    }
                    else if (ControllerDelayLonger)
                    {
                        vControllerDelay_Media = GetSystemTicksMs() + vControllerDelayLongerTicks;
                    }

                    //Update the window style (focus workaround)
                    if (ControllerDelayShort || ControllerDelayMedium || ControllerDelayLonger)
                    {
                        UpdateWindowStyleVisible();
                    }
                }
            }
            catch { }
        }
    }
}