namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Request")]
    public partial class Request
    {
        public long RequestId { get; set; }

        [StringLength(50)]
        public string Requestor { get; set; }

        public long? AddressId { get; set; }

        public DateTime? RequestDateTime { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int? Status { get; set; }

        public int? Type { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public long? UserID { get; set; }
    }
}
