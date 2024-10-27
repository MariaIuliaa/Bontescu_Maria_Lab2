using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bontescu_Maria_Lab2.Models;

namespace Bontescu_Maria_Lab2.Data
{
    public class Bontescu_Maria_Lab2Context : DbContext
    {
        public Bontescu_Maria_Lab2Context (DbContextOptions<Bontescu_Maria_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Bontescu_Maria_Lab2.Models.Book> Book { get; set; } = default!;

        public DbSet<Bontescu_Maria_Lab2.Models.Publisher>? Publisher { get; set; }

        public DbSet<Bontescu_Maria_Lab2.Models.Author>? Author { get; set; }

        public DbSet<Bontescu_Maria_Lab2.Models.Category>? Category { get; set; }
    }
}
