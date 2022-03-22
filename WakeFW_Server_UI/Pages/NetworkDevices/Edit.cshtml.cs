#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WakeFW_Server_UI.Data;
using WakeFW_Server_UI.Models;

namespace WakeFW_Server_UI.Pages.NetworkDevices
{
    public class EditModel : PageModel
    {
        private readonly WakeFW_Server_UI.Data.WakeFW_Server_UIContext _context;

        public EditModel(WakeFW_Server_UI.Data.WakeFW_Server_UIContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TargetNetworkDevice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TargetNetworkDeviceExists(TargetNetworkDevice.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TargetNetworkDeviceExists(int id)
        {
            return _context.TargetNetworkDevice.Any(e => e.Id == id);
        }
    }
}
