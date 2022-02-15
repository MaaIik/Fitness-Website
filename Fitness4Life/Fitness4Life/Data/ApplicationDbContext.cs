using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fitness4Life.Models;

namespace Fitness4Life.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Fitness4Life.Models.Forum> Forums { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Fitness4Life.Models.WorkoutProgramme> WorkoutProgrammes { get; set; }
    }
}
