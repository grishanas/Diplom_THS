using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_.Models.controller
{
    [Table("mco_id")]
    public class ControllerOutput
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("mc_address")]
        public int controllerAddress { get; set; }

        [Column("mco_name")]
        public string? name { get; set; }

        [Column("mco_description")] 
        public string? description { get; set; }
        
        [Column("mcov_range_id")]
        public int RangeId { get; set; }

        [Column("mco_q_id")]
        public int QueryId { get; set; }

        [Column("mco_s_id")]
        public int OutputState { get; set; }
    }

    [Table("mcov_range")]
    public class OutputRange
    {
        [Column("mcov_range_id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RangeId { get; set; }
        [Column("mcov_range_start")]
        public Single RangeStart { get; set; }
        [Column("mcov_range_min_value")]
        public Single RangeMin { get; set; }
        [Column("mcov_range_max_value")]
        public Single RangeMax { get; set; }
    }
    [Table("mcov_state")]
    public class OutputValueState
    {
        [Column("mcov_s_id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public UInt32 id { get; set; }
        [Column("mcov_description")]
        public string description { get; set; }
    }


    [Table("microcontroller_output_state")]
    public class OutputState
    {
        [Column("mco_s_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Column("mco_s_description")]
        public string description { get; set; }

    }

    [Table("mco_v")]
    public class OutputValue
    {
        [Column("mco_v_value")]
        public byte[] value { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("mcov_id")]
        public int stateId { get; set; }
        [Column("mc_addres")]
        public int controllerAddress { get; set; }
        [Column("mco_id")] 
        public int controllerOutputId { get; set; }
        [Column("mco_v_time")]
        public DateTime DateTime { get; set; }
    }

}
