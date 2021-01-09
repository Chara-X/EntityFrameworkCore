using System;
using System.Reflection;
using EntityFrameworkCore.Collections;
using EntityFrameworkCore.Collections.Extensions;
using EntityFrameworkCore.ORMapping.Attributes;

namespace EntityFrameworkCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //const string connectionString = "Data Source=DESKTOP-7DHP15Q;Initial Catalog=Airlines;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            const string connectionString = "Data Source=DESKTOP-7DHP15Q;Initial Catalog=Family;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var db = new FamilyDbContext(connectionString);
            db.Proxy.Create(typeof(Baby), typeof(Father));
            db.Babies.Add(new Baby(1, "a", "bb"));
            db.SaveChanged();
            //foreach (var i in db.Students.Select(i => i.Teacher).Where(i => i.Name != "TeacherB").OrderByDescending(i => i.CarId))
            //{
            //    Console.WriteLine(i.Name);
            //}

            //foreach (var i in db.Students.Select(i => new { i.Id, i.Name, Teacher_Name = i.Teacher.Name, Teacher_Car_Name = i.Teacher.Car.Name }).Where(i => i.Teacher_Car_Name != "CarC"))
            //{
            //    Console.WriteLine(i.Teacher_Car_Name);
            //}

            //var res = db.Students.Select(i => i.Teacher).First(i => i.CarId == db.Teachers.Max(j => j.CarId));
            //Console.WriteLine(res.Car.Name);


        }
    }

    public class FamilyDbContext : DbContext
    {
        public DbSet<Baby> Babies { get; set; }

        public DbSet<Father> Fathers { get; set; }

        public FamilyDbContext(string connectionString) : base(connectionString) { }
    }

    public class People
    {
        [PrimaryKey] public int Id { get; set; }

        [Required] public string Name { get; set; }

        public People(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class Baby : People
    {
        public string Book { get; set; }

        public Baby(int id, string name, string book) : base(id, name) => Book = book;
    }

    public class Father : People
    {
        public string Work { get; set; }

        public int? BabyId { get; set; }

        [ForeignKey(nameof(BabyId))] public Baby Baby { get; set; }

        public Father(int id, string name, string work) : base(id, name) => Work = work;
    }

    public class AirlineDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Student> Students { get; set; }

        public AirlineDbContext(string connectionString) : base(connectionString) { }
    }

    public class Student
    {
        [PrimaryKey] public int Id { get; set; }

        public string Name { get; set; }

        public int? TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]public Teacher Teacher { get; set; }

        public Student(int id, string name, int? teacherId)
        {
            Id = id;
            Name = name;
            TeacherId = teacherId;
        }
    }

    public class Teacher
    {
        [PrimaryKey] public int Id { get; set; }

        public string Name { get; set; }

        public int? CarId { get; set; }

        [ForeignKey(nameof(CarId))]public Car Car { get; set; }

        public Teacher(int id, string name, int? carId)
        {
            Id = id;
            Name = name;
            CarId = carId;
        }
    }

    public class Car
    {
        [PrimaryKey] public int Id { get; set; }

        public string Name { get; set; }

        public Car(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
