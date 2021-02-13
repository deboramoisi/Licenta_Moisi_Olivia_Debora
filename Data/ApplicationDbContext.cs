using System;
using System.Collections.Generic;
using System.Text;
using Licenta.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Client> Client { get; set; }
        public DbSet<SediuSocial> SediuSocial { get; set; }
        public DbSet<Furnizor> Furnizor { get; set; }
        public DbSet<ClientFurnizor> ClientFurnizor { get; set; }
        public DbSet<Salariat> Salariat { get; set; }
        public DbSet<IstoricSalar> IstoricSalar { get; set; }
    }
}
