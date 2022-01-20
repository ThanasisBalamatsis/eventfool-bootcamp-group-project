using EventFool.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;


namespace EventFool.ViewModels
{
    public class PhotoViewModel
    {

        public Guid Id { get; set; }
        public string Image { get; set; }
        
        public bool Checked { get; set; }

    }
}