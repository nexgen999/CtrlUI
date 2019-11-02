﻿using System;

namespace LibraryShared
{
    public partial class Classes
    {
        public class ShortcutDetails
        {
            public string Name { get; set; }
            public string TargetPath { get; set; }
            public string WorkingPath { get; set; }
            public string IconPath { get; set; }
            public string ShortcutPath { get; set; }
            public string Type { get; set; }
            public string Argument { get; set; }
            public string Comment { get; set; }
            public DateTime TimeModify { get; set; }
        }
    }
}