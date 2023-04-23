#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WakeFwGateway.Models;

namespace WakeFwGateway.Data
{
    public class WakeFwGatewayContext : DbContext
    {
        public WakeFwGatewayContext (DbContextOptions<WakeFwGatewayContext> options)
            : base(options)
        {
        }

        public DbSet<WakeFwGateway.Models.TargetNetworkDevice> TargetNetworkDevice { get; set; }
    }
}
