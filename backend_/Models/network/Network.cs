using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_.Models.network
{

    public class UserNetwork
    {
        public string Discription { get; set; }

        public string Name { get; set; }
    }

    public class Network
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pn_id")]
        public int Id { get; set;}

        [MaxLength(100)]
        [Column("pn_discription")]
        public string Discription { get; set;}

        [MaxLength(30)]
        [Column("pn_name")]
        public string Name { get; set;}

        public Network(UserNetwork userNetwork)
        {
            this.Discription=userNetwork.Discription;
            this.Name=userNetwork.Name;
        }
    }
}
