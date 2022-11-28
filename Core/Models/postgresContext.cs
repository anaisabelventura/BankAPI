using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;



namespace BankAPI.Core.Models
{
    public partial class postgresContext : DbContext
    {
        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; } = null!;
        public virtual DbSet<Transfer> Transfer { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Token> Token { get; set; } = null!;
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                          .AddJsonFile("appsettings.json")
                          .Build();
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("DbConnection"));
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnName("id");
                    //.HasDefaultValueSql("nextval('accounts_id_seq'::regclass)");

                entity.Property(e => e.Balance)
                     .HasColumnName("balance");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Currency)
                    .HasColumnType("character varying")
                    .HasColumnName("currency");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accounts_fkey");
            });

            modelBuilder.Entity<Transfer>(entity =>
            {

                entity.ToTable("Transfer");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                    //.HasDefaultValueSql("nextval('transfers_id_seq'::regclass)");

                entity.Property(e => e.Amount)
                .HasColumnName("amount");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SenderAccountId)
                    .HasColumnName("senderaccountid");

                entity.Property(e => e.DestinationAccountId)
                    .HasColumnName("destinationaccountid");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "users_email_key")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "users_username")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                    //.HasDefaultValueSql("nextval('users_id_seq'::regclass)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Email)
                    .HasColumnType("character varying")
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasColumnType("character varying")
                    .HasColumnName("full_name");

                entity.Property(e => e.Password)
                    .HasColumnType("character varying")
                    .HasColumnName("password");

                entity.Property(e => e.UserName)
                    .HasColumnType("character varying")
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");
                    //.HasDefaultValueSql("nextval('tokens_id_seq'::regclass)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Refresh_Token)
                   .HasColumnName("refresh_token");

                entity.Property(e => e.RefreshToken_expireAt)
                   .HasColumnName("refresh_token_expire_at");

                entity.Property(e => e.UserID)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tokens)
                    .HasForeignKey(d => d.UserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tokens_fkey");

            });

           

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}


