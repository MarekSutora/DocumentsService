﻿using DocumentsService.API.Common.JsonConverters;
using DocumentsService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsService.Storage.Implementations.InMemoryDb
{
    public class DocumentsDbContext : DbContext
    {
        public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options) { }

        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new DictionaryToJsonConverter();

            modelBuilder.Entity<Document>()
                .Property(e => e.Data)
                .HasConversion(converter);

            base.OnModelCreating(modelBuilder);
        }
    }
}
