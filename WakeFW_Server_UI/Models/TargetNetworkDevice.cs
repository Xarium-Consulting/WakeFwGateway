using System.ComponentModel.DataAnnotations;

namespace WakeFW_Server_UI.Models
{
    public class TargetNetworkDevice
    {      
        public int Id { get; set; }
        [Display(Name ="IP address")]
        public string Ip { get; set; }
        [Display(Name = "MAC address")]
        public string Mac { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Creation Date")]
        public DateTime AddedDate { get; set; }
    }
}