# EntityFrameworkCore

Entity Framework (EF) Core 是轻量级、可扩展、跨平台的数据访问技术，EF Core 可用作对象关系映射程序 (O/RM)，这可以实现以下两点：

- 使 .NET 开发人员能够使用 .NET 对象处理数据库
- 无需再像通常那样编写大部分数据访问代码

## The model

使用 EF Core 时，数据访问是使用模型执行的。模型由实体类和表示与数据库的会话的上下文对象组成。上下文对象允许查询和保存数据。

```C#
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
```

## **Saving data**

使用实体类的实例在数据库中创建、删除和修改数据。

```C#
using (var db = new SchoolDbContext(ConnectionString))
{
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
```

![img](http://www.kdocs.cn/api/v3/office/copy/Yy9vaDFIOUg4Mk9aSS84NXpCaVc1Sm1QcUZtT0tkVnVKTkVpdFI5WmJWUmdpOVV3UnJ2ZFlmREJ6dDRaYVE2ODc4azNUN0dObm4rYit4MC96UDlrdWVUMlFub3I2V0p3eVVoYjRLVk5sdE13NXV5aUJnMk5VcnJROGwweFFJZUljbzVsd2FGQnFLaXNJQ2UyU051b2FmMGhUaUc0K3M0YlE0VE42NWtua215ZVVmTHlEaDc0TlhjZG4rQ0NJanhNUVp5bERqTXR2Qk11ODk2UUYvaTNkRWRUZG5wVGoweGY1aWh1OG9zTjJQc1B2Tk90dTIyS1g3YTZqUkpSV2hFYWpoZHFTM21vS1ZRPQ==/attach/object/FW7YGBIANY?)

## **Querying**

使用语言集成查询 （LINQ） 从数据库中检索实体类的实例。

```C#
using var db = new SchoolDbContext(ConnectionString);
foreach (var x in db.Students.Where(x => x.Teacher.Car.Name == "A").Select(x => x.Teacher))
{
    x.Dump();
}
```

![img](http://www.kdocs.cn/api/v3/office/copy/Yy9vaDFIOUg4Mk9aSS84NXpCaVc1Sm1QcUZtT0tkVnVKTkVpdFI5WmJWUmdpOVV3UnJ2ZFlmREJ6dDRaYVE2ODc4azNUN0dObm4rYit4MC96UDlrdWVUMlFub3I2V0p3eVVoYjRLVk5sdE13NXV5aUJnMk5VcnJROGwweFFJZUljbzVsd2FGQnFLaXNJQ2UyU051b2FmMGhUaUc0K3M0YlE0VE42NWtua215ZVVmTHlEaDc0TlhjZG4rQ0NJanhNUVp5bERqTXR2Qk11ODk2UUYvaTNkRWRUZG5wVGoweGY1aWh1OG9zTjJQc1B2Tk90dTIyS1g3YTZqUkpSV2hFYWpoZHFTM21vS1ZRPQ==/attach/object/7S7IGBIAPI?)
