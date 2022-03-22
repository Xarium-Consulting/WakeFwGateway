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

namespace WakeFW_Server_UI.Pages.NetworkDevices
{
    public class DeleteModel : PageModel
    {
        private readonly WakeFW_Server_UI.Data.WakeFW_Server_UIContext _context;

        public DeleteModel(WakeFW_Server_UI.Data.WakeFW_Server_UIContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TargetNetworkDevice = await _context.TargetNetworkDevice.FindAsync(id);

            if (TargetNetworkDevice != null)
            {
                _context.TargetNetworkDevice.Remove(TargetNetworkDevice);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
