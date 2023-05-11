using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_.Models.controllerGroup;

namespace backend_.Models.controller
{
    [Table("mco_id")]
    public class ControllerOutput
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("mc_address")]
        public UInt32 controllerAddress { get; set; }
        public Controller? controller { get; set; }

        [Column("mco_name")]
        public string name { get; set; }

        [Column("mco_description")] 
        public string description { get; set; }
        
        [Column("mcov_range_id")]
        public int? RangeId { get; set; }
        public OutputRange? range { get; set; }

        [Column("mco_q_id")]
        public int? QueryId { get; set; }
        public ControllerQuery? Query { get; set; }

        [Column("mco_s_id")]
        public int? outputStateId { get; set; }
        public OutputState? outputState { get; set; }


        public List<ControllerOutputGroup>? outputGroups { get; set; } = new List<ControllerOutputGroup>();
        public List<m2mControllerOutputGroup>? m2mOutputGroups { get; set; } = new List<m2mControllerOutputGroup>();

        public List<OutputValue>? outputValues { get; set; } = new List<OutputValue>();
    }

    [Table("mcov_range")]
    public class OutputRange
    {
        [Column("mcov_range_id")]
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RangeId { get; set; }
        [Column("mcov_range_start")]
        public float RangeStart { get; set; }
        [Column("mcov_range_min_value")]
        public float RangeMin { get; set; }
        [Column("mcov_range_max_value")]
        public float RangeMax { get; set; }

        public List<ControllerOutput>? controllers { get; set; } = new List<ControllerOutput>();
    }
    [Table("mcov_state")]
    public class OutputValueState
    {
        [Column("mcov_s_id")]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public UInt32 id { get; set; }
        [Column("mcov_description")]
        public string description { get; set; }

        public List<OutputValue> outputValues { get; set; } = new List<OutputValue>();

    }


    [Table("microcontroller_output_state")]
    public class OutputState
    {
        [Column("mco_s_id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Column("mco_s_description")]
        public string description { get; set; }

        public List<ControllerOutput>? controllers { get; set; } = new List<ControllerOutput>();

    }

    [Table("mco_v")]
    public class OutputValue
    {
        [Column("mco_v_value")]
        public byte[] value { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("mcov_id")]
        public UInt32 controllerAddress { get; set; }
        [Column("mco_id")] 
        public int controllerOutputId { get; set; }
        public ControllerOutput? controllerOutput { get; set; }
        [Column("mco_v_time")]
        public DateTime DateTime { get; set; }




    }

}
