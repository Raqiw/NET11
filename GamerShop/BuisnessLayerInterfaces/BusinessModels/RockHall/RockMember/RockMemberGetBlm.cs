﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerInterfaces.BusinessModels.RockHall.RockMember
{
    public class RockMemberGetBlm
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Genre { get; set; }
        public int EntryYear { get; set; }
        public int YearOfBirth { get; set; }
        public string CurrentBandName { get; set; }
        public string CreatorName { get; set; }
    }
}
