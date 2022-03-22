#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WakeFW_Server_UI.Data;
using WakeFW_Server_UI.Models;
using System.Net;
using System.Net.NetworkInformation;

namespace WakeFW_Server_UI.Pages.NetworkDevices
{
    public class CreateModel : PageModel
    {
        private readonly WakeFW_Server_UI.Data.WakeFW_Server_UIContext _context;

        public CreateModel(WakeFW_Server_UI.Data.WakeFW_Server_UIContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TargetNetworkDevice TargetNetworkDevice { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TargetNetworkDevice.AddedDate = DateTime.Now.Date;
            IPAddress ParsedIp = null;
            if (!IPAddress.TryParse(TargetNetworkDevice.Ip, out ParsedIp))
            {
                return RedirectToPage("/Error");
            }

            PhysicalAddress ParsedMac = null;
            if(!PhysicalAddress.TryParse(TargetNetworkDevice.Mac, out ParsedMac))
            {
                return RedirectToPage("/Error");
            }
            
            _context.TargetNetworkDevice.Add(TargetNetworkDevice);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
