//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_10.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Contact
    {
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]

        public string Email { get; set; }

        public string Message { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Invalid Phone Number.")]
        public string Phone { get; set; }
    }
}
