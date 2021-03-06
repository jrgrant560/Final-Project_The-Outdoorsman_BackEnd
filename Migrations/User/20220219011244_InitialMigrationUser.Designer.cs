// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OutdoorsmanBackend.Models;

namespace OutdoorsmanBackend.Migrations.User
{
    [DbContext(typeof(UserContext))]
    [Migration("20220219011244_InitialMigrationUser")]
    partial class InitialMigrationUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("city")
                        .HasColumnType("TEXT");

                    b.Property<string>("emailAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("firstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("lastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("password")
                        .HasColumnType("TEXT");

                    b.Property<string>("phoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("state")
                        .HasColumnType("TEXT");

                    b.Property<string>("streetAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("zipCode")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
