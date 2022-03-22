#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WakeFW_Server_UI.Models;

namespace WakeFW_Server_UI.Data
{
    public class WakeFW_Server_UIContext : DbContext
    {
        public WakeFW_Server_UIContext (DbContextOptions<WakeFW_Server_UIContext> options)
            : base(options)
        {
        }

        public DbSet<WakeFW_Server_UI.Models.TargetNetworkDevice> TargetNetworkDevice { get; set; }
    }
}
