using System;
using System.Collections.Generic;
using System.Text;
using Licenta.Models;
using Licenta.Models.QandA;
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
        public DbSet<Salariat> Salariat { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<TipDocument> TipDocument { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<QuestionCategory> QuestionCategory { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Response> Response { get; set; }
        public DbSet<Furnizori> Furnizori { get; set; }
        public DbSet<SolduriCasa> SolduriCasa { get; set; }

    }
}
