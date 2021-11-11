﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GenshinCompanion.Modules.BannersModule.Models
{
    public class EndTime
    {
        public EndTime(DateTime? time, int resin)
        {
            Time = time;
            Resin = resin;
        }

        public DateTime? Time { get; set; }
        public int Resin { get; set; }
    }
}
