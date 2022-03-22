#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WakeFW_Server_UI.Data;
using WakeFW_Server_UI.Models;
using System.Net;
using System.Net.NetworkInformation;


namespace WakeFW_Server_UI.Pages.NetworkDevices
{
    public class DetailsModel : PageModel
    {
        private readonly WakeFW_Server_UI.Data.WakeFW_Server_UIContext _context;

        public DetailsModel(WakeFW_Server_UI.Data.WakeFW_Server_UIContext context)
        {
            _context = context;
        }

        public TargetNetworkDevice TargetNetworkDevice { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TargetNetworkDevice = await _context.TargetNetworkDevice.FirstOrDefaultAsync(m => m.Id == id);

            if (TargetNetworkDevice == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetWakeClickAsync(int? id)
        {
            TargetNetworkDevice = await _context.TargetNetworkDevice.FirstOrDefaultAsync(m => m.Id == id);

            if (TargetNetworkDevice == null)
            {
                return NotFound();
            }

            IPAddress targetIp = IPAddress.Parse(TargetNetworkDevice.Ip);
            PhysicalAddress targetMac = PhysicalAddress.Parse(TargetNetworkDevice.Mac);

            byte[] payload = WakeTarget.GeneratePayload(targetMac);
            WakeTarget.WakeDevice(targetIp, payload, null);
            return RedirectToPage("./Index");
        }
    }
}
