using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entity
{
    [Table("MesBot")]
    public class MesBotEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string ReplyText { get; set; }

        [StringLength(200)]
        public string Exception { get; set; }

        /// <summary>
        /// 1. Dự báo thời tiết
        /// 2. chào và tạm biệt
        /// </summary>
        public int Type { get; set; }
    }
}