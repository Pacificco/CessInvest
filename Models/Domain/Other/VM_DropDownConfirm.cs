﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CessInvest.Models.Domain.Other
{
    public class VM_DropDownConfirm    
    {
        public EnumFirstDropDownItem FirstItem { get; set; }        
        public EnumBoolType SelectedId { get; set; }
        public string Name { get; set; }

        public VM_DropDownConfirm()
        {
            FirstItem = EnumFirstDropDownItem.None;            
            SelectedId = EnumBoolType.None;
            Name = "";
        }
    }
}