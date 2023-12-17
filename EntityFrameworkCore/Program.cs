using System;
using EntityFrameworkCore.Collections;
using EntityFrameworkCore.Collections.Extensions;
using EntityFrameworkCore.ORMapping.Attributes;

namespace EntityFrameworkCore;

public class Program
{
    private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EntityFrameworkCore";

    public static void Main(string[] args)
    {
        SavingData();
        //Querying();
    }

    public static void SavingData()
    {
        using var db = new SchoolDbContext(ConnectionString);
        db.CreateModels();
        db.Students.Add(new Student("A", 1));
        db.Students.Add(new Student("B", 1));
        db.Students.Add(new Student("C", 2));
        db.Students.Add(new Student("D", 2));
        db.Teachers.Add(new Teacher("A", 1));
        db.Teachers.Add(new Teacher("B", 2));
        db.Cars.Add(new Car("A", DateTime.Now));
        db.Cars.Add(new Car("B", DateTime.Now));
        db.SaveChanges();
    }

    public static void Querying()
    {
        using var db = new SchoolDbContext(ConnectionString);
        foreach (var x in db.Students.Where(x => x.Teacher.Car.Name == "A").Select(x => x.Teacher))
        {
            x.Dump();
        }
    }
}

public class SchoolDbContext : DbContext
{
    public DbSet<Car> Cars { get; set; }

    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<Student> Students { get; set; }

    public SchoolDbContext(string connectionString) : base(connectionString) { }
}

public class Student
{
    [PrimaryKey(true)] public int Id { get; set; }

    public string Name { get; set; }

    public int? TeacherId { get; set; }

    [ForeignKey(nameof(TeacherId))] public Teacher Teacher { get; set; }

    public Student(string name, int? teacherId)
    {
        Id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray());
        Name = name;
        TeacherId = teacherId;
    }
}

public class Teacher
{
    [PrimaryKey(true)] public int Id { get; set; }

    public string Name { get; set; }

    public int? CarId { get; set; }

    [ForeignKey(nameof(CarId))] public Car Car { get; set; }

    public Teacher(string name, int? carId)
    {
        Id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray());
        Name = name;
        CarId = carId;
    }
}

public class Car
{
    [PrimaryKey(true)] public int Id { get; set; }

    public DateTime CreateTime { get; set; }

    public string Name { get; set; }

    public Car(string name, DateTime createTime)
    {
        Id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray());
        Name = name;
        CreateTime = createTime;
    }
}