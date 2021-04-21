using Licenta.Models;
using Licenta.Models.Chat;
using Licenta.Models.Notificari;
using Licenta.Models.Plati;
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
        public DbSet<Salariat> Salariat { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<TipDocument> TipDocument { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<QuestionCategory> QuestionCategory { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Response> Response { get; set; }
        public DbSet<Furnizori> Furnizori { get; set; }
        public DbSet<SolduriCasa> SolduriCasa { get; set; }
        public DbSet<ProfitPierdere> ProfitPierdere { get; set; }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<Mesaj> Mesaje { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<TipPlata> TipPlati { get; set; }
        public DbSet<Plata> Plati { get; set; }

        public DbSet<NotificareUser> NotificareUsers { get; set; }
        public DbSet<Notificare> Notificari { get; set; }


        // Composite Key pentru ChatUsers
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatUser>()
                .HasKey(x => new { x.ChatId, x.ApplicationUserId });

            builder.Entity<NotificareUser>()
                   .HasKey(k => new { k.NotificareId, k.ApplicationUserId });
        }

    }
}
