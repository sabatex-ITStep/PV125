﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfEFCoreDemo.Models;

namespace WpfEFCoreDemo.Data
{
    internal class DemoDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentGroup> StudenGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite(@"filename=c:\databases\PV125Demo.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Student>().HasKey(s=>s.BirthDay);
            //modelBuilder.Entity<StudentGroup>().ToTable("SG");
            modelBuilder.Entity<StudentGroup>().ToView("SG");

        }

    }
}

