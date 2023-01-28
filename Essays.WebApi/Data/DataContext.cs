using Essays.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Essays.WebApi.Data
{
    public sealed class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<CountriesOfAuthors> CountriesOfAuthors { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Essay> Essays { get; set; }
        public DbSet<EssaysAboutSubjects> EssaysAboutSubjects { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectCategory> SubjectCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountriesOfAuthors>()
                    .HasKey(coa => new { coa.AuthorId, coa.CountryAbbreviation });
            modelBuilder.Entity<CountriesOfAuthors>()
                    .HasOne(coa => coa.Author)
                    .WithMany(a => a.CountriesOfAuthors)
                    .HasForeignKey(coa => coa.AuthorId);
            modelBuilder.Entity<CountriesOfAuthors>()
                    .HasOne(coa => coa.Country)
                    .WithMany(c => c.CountriesOfAuthors)
                    .HasForeignKey(coa => coa.CountryAbbreviation);

            modelBuilder.Entity<EssaysAboutSubjects>()
                    .HasKey(eas => new { eas.EssayId, eas.SubjectId });
            modelBuilder.Entity<EssaysAboutSubjects>()
                    .HasOne(eas => eas.Essay)
                    .WithMany(e => e.EssaysAboutSubjects)
                    .HasForeignKey(eas => eas.EssayId);
            modelBuilder.Entity<EssaysAboutSubjects>()
                    .HasOne(eas => eas.Subject)
                    .WithMany(s => s.EssaysAboutSubjects)
                    .HasForeignKey(eas => eas.SubjectId);
        }
    }
}