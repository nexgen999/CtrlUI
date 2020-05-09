﻿using System;
using static ArnoldVinkCode.AVDisplayMonitor;
using static ArnoldVinkCode.AVFunctions;
using static ArnoldVinkCode.AVInteropDll;
using static KeyboardController.AppVariables;

namespace KeyboardController
{
    partial class WindowMain
    {
        void MoveKeyboardWindow(int thumbHorizontal, int thumbVertical)
        {
            try
            {
                //Check the thumb movement
                int smallOffset = 2500;

                thumbVertical = -thumbVertical;
                int absHorizontal = Math.Abs(thumbHorizontal);
                int absVertical = Math.Abs(thumbVertical);

                if (absHorizontal > smallOffset || absVertical > smallOffset)
                {
                    double mouseSensitivity = 0.0007;
                    int mouseHorizontal = Convert.ToInt32(thumbHorizontal * mouseSensitivity);
                    int mouseVertical = Convert.ToInt32(thumbVertical * mouseSensitivity);

                    //Get the current window position
                    WindowRectangle positionRect = new WindowRectangle();
                    GetWindowRect(vInteropWindowHandle, ref positionRect);
                    int moveLeft = positionRect.Left + mouseHorizontal;
                    int moveTop = positionRect.Top + mouseVertical;
                    int moveRight = positionRect.Right + mouseHorizontal;
                    int moveBottom = positionRect.Bottom + mouseVertical;

                    //Get the current active screen
                    int monitorNumber = Convert.ToInt32(vConfigurationCtrlUI.AppSettings.Settings["DisplayMonitor"].Value);
                    DisplayMonitorSettings displayMonitorSettings = GetScreenSettings(monitorNumber);

                    //Get the current window size
                    int windowWidth = (int)(this.ActualWidth * displayMonitorSettings.DpiScaleHorizontal);
                    int windowHeight = (int)(this.ActualHeight * displayMonitorSettings.DpiScaleVertical);

                    //Check if window leaves screen
                    double screenEdgeLeft = moveLeft + windowWidth;
                    double screenLimitLeft = displayMonitorSettings.BoundsLeft + 20;
                    double screenEdgeTop = moveTop + windowHeight;
                    double screenLimitTop = displayMonitorSettings.BoundsTop + 20;
                    double screenEdgeRight = moveRight - windowWidth;
                    double screenLimitRight = displayMonitorSettings.BoundsRight - 20;
                    double screenEdgeBottom = moveBottom - windowHeight;
                    double screenLimitBottom = displayMonitorSettings.BoundsBottom - 20;
                    if (screenEdgeLeft > screenLimitLeft && screenEdgeTop > screenLimitTop && screenEdgeRight < screenLimitRight && screenEdgeBottom < screenLimitBottom)
                    {
                        WindowMove(vInteropWindowHandle, moveLeft, moveTop);
                    }
                }
            }
            catch { }
        }
    }
}