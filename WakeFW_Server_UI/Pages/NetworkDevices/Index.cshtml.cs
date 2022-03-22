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
    public class IndexModel : PageModel
    {
        private readonly WakeFW_Server_UI.Data.WakeFW_Server_UIContext _context;

        public IndexModel(WakeFW_Server_UI.Data.WakeFW_Server_UIContext context)
        {
            _context = context;
        }

        public IList<TargetNetworkDevice> TargetNetworkDevice { get;set; }

        public async Task OnGetAsync()
        {
            TargetNetworkDevice = await _context.TargetNetworkDevice.ToListAsync();
        }
    }
}
