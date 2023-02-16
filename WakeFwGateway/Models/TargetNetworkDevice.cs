using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace WakeFwGateway.Models
{
    public class TargetNetworkDevice
    {      
        public int Id { get; set; }
        
        [Display(Name ="IP address")]
        [Required(ErrorMessage = "IP is required")]
        [MinLength(7, ErrorMessage = "Specified IP is too short")]
        [Remote(action: "ValidateIpAddress", controller: "TargetNetworkDevices")]
        public string Ip { get; set; }
        
        
        [Display(Name = "MAC address")]
        [Required(ErrorMessage = "MAC is required")]
        [MinLength(17, ErrorMessage = "Specified MAC is too short")]
        [MaxLength(17, ErrorMessage = "Specified MAC is too long")]
        [Remote(action: "ValidateMacAddress", controller: "TargetNetworkDevices")]
        public string Mac { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name ="Creation Date")]
        [Required(ErrorMessage = "Creation date is required")]
        public DateTime AddedDate { get; set; }
    }
}
