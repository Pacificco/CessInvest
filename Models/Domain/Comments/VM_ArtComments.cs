using CessInvest.Models.Domain.Articles;
using CessInvest.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CessInvest.Models.Domain.Comments
{
    public class VM_ArtComments
    {
        public VM_Article Art { get; set; }
        public List<VM_Comment> Items { get; set; }
        public VM_PagingInfo PagingInfo { get; set; }

        public VM_ArtComments()
        {
            Art = new VM_Article();
            Items = new List<VM_Comment>();
            PagingInfo = new VM_PagingInfo();
        }
    }
}