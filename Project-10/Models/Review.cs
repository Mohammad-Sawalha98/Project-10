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
    
    public partial class Review
    {
        public int ReviewId { get; set; }
        public string Id { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string Comment { get; set; }
        public Nullable<int> Rating { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Product Product { get; set; }
    }
}
