using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarShowRoom.Models;
namespace CarShowRoom.Data
{
    public class ShowRoomContext : DbContext
    {
        public ShowRoomContext(DbContextOptions<ShowRoomContext> options) : base(options)
        { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<AssembledCar> AssembledCars { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Command>().ToTable("Commands");
            modelBuilder.Entity<Car>().ToTable("Cars");
            modelBuilder.Entity<Mechanic>().ToTable("Mechanic");
            modelBuilder.Entity<AssembledCar>().ToTable("AssembledCar");
            modelBuilder.Entity<AssembledCar>()
            .HasKey(c => new { c.CarID, c.MechanicID });//configureaza cheia primara compusa
        }

    }
}