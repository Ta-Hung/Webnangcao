//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MaverikStudio.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class product_details
    {
        public int product_id { get; set; }
        public int size_id { get; set; }
        public int quantity { get; set; }
        public int quantity_ready { get; set; }
        public double price { get; set; }
        public double price_origin { get; set; }
        public double sale { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
    
        public virtual product product { get; set; }
        public virtual size size { get; set; }
    }
}