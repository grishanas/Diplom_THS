using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_.Models.network;

namespace backend_.Models.controller
{
    [Table("mco_query")]
    public class ControllerQuery
    {
        [Column("mco_q_id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Column("mco_query_request")]
        public string query { get; set; }
    }

}
