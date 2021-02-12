using Bikes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bikes.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		public DbSet<Make> Makes { get; set; }
		public DbSet<Model> Models { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<Bike> Bikes { get; set; }
	}
	
}
