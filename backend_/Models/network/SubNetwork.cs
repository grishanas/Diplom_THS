using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend_.Models.network
{
    public class UserSubNetwork
    {

    }
    public class SubNetwork
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("n_id")]
        public int Id { get; set; }

        [ForeignKey("pn_id")]
        public Network Network { get; set; }

        [Required]
        [Column("n_prefix")]
        public byte Prefix { get; set; }

        [Required]
        [Column("n_address")]
        public int Address { get; set; }

        [Column("n_name")]
        public string Name { get; set; }

        [Column("n_discription")]
        public string Discription { get; set; }
    }
}
