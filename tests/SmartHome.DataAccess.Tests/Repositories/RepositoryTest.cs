using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Tests.Repositories;

[TestClass]
public class RepositoryTest
{
    private readonly DbContext _context = DbContextBuilder.BuildRepositoryTestDbContext();
    private readonly Repository<EntityTest> _repository;

    #region Initialize And Finalize

    public RepositoryTest()
    {
        _repository = new Repository<EntityTest>(_context);
    }

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #endregion

    #region Add

    #region Error

    [TestMethod]
    public void Add_WhenError_ShouldThrowDataAccessException()
    {
        var entity = new EntityTest("some name");

        _context.Database.EnsureDeleted();

        Action act = () => _repository.Add(entity);

        act.Should().Throw<DataAccessException>().WithMessage("Error adding entity to the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Add_WhenCalled_ShouldAddedToDataBase()
    {
        var entity = new EntityTest("some name");

        _repository.Add(entity);

        using TestDbContext otherContext = DbContextBuilder.BuildRepositoryTestDbContext();

        var entitiesSaved = otherContext.EntitiesTest.ToList();

        entitiesSaved.Count.Should().Be(1);

        EntityTest entitySaved = entitiesSaved[0];
        entitySaved.Id.Should().Be(entity.Id);
        entitySaved.Name.Should().Be(entity.Name);
    }

    #endregion

    #endregion

    #region Update

    #region Error

    [TestMethod]
    public void Update_WhenError_ShouldThrowDataAccessException()
    {
        var entity = new EntityTest("some name");

        _repository.Add(entity);

        _context.Database.EnsureDeleted();

        Action act = () => _repository.Update(entity);

        act.Should().Throw<DataAccessException>().WithMessage("Error updating entity in the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Update_WhenCalled_ShouldUpdatedInDataBase()
    {
        var entity = new EntityTest("some name");

        _repository.Add(entity);

        entity.Name = "new name";

        _repository.Update(entity);

        using TestDbContext otherContext = DbContextBuilder.BuildRepositoryTestDbContext();

        var entitiesSaved = otherContext.EntitiesTest.ToList();

        entitiesSaved.Count.Should().Be(1);

        EntityTest entitySaved = entitiesSaved[0];
        entitySaved.Id.Should().Be(entity.Id);
        entitySaved.Name.Should().Be(entity.Name);
    }

    #endregion

    #endregion

    #region Delete

    #region Error

    [TestMethod]
    public void Delete_WhenError_ShouldThrowDataAccessException()
    {
        var entity = new EntityTest("some name");

        _repository.Add(entity);

        _context.Database.EnsureDeleted();

        Action act = () => _repository.Delete(entity);

        act.Should().Throw<DataAccessException>().WithMessage("Error deleting entity from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Delete_WhenCalled_ShouldDeletedFromDataBase()
    {
        var entity = new EntityTest("some name");

        _repository.Add(entity);

        _repository.Delete(entity);

        using TestDbContext otherContext = DbContextBuilder.BuildRepositoryTestDbContext();

        var entitiesSaved = otherContext.EntitiesTest.ToList();

        entitiesSaved.Count.Should().Be(0);
    }

    #endregion

    #endregion

    #region Exists

    #region Error

    [TestMethod]
    public void Exists_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Func<bool> act = () => _repository.Exists(e => e.Id == "some id");

        act.Should().Throw<DataAccessException>().WithMessage("Error checking if entity exists in the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Exists_WhenExists_ShouldReturnTrue()
    {
        var entity = new EntityTest("some name");

        _repository.Add(entity);

        _repository.Exists(e => e.Id == entity.Id).Should().BeTrue();
    }

    [TestMethod]
    public void Exists_WhenNotExists_ShouldReturnFalse()
    {
        var entity = new EntityTest("some name");

        _repository.Exists(e => e.Id == entity.Id).Should().BeFalse();
    }

    #endregion

    #endregion

    #region Get

    #region Error

    [TestMethod]
    public void Get_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Func<EntityTest?> act = () => _repository.Get(e => e.Id == "some id");

        act.Should().Throw<DataAccessException>().WithMessage("Error getting entity from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Get_WhenNotExists_ReturnsNull()
    {
        EntityTest? entity = _repository.Get(e => e.Id == "not exists");

        entity.Should().BeNull();
    }

    [TestMethod]
    public void Get_WhenExists_ShouldReturnEntity()
    {
        var expectedEntity = new EntityTest { Name = "dummy" };

        _repository.Add(expectedEntity);
        _context.SaveChanges();

        EntityTest? entitySaved = _repository.Get(e => e.Id == expectedEntity.Id);

        entitySaved.Should().NotBeNull();
        entitySaved.Id.Should().Be(expectedEntity.Id);
        entitySaved.Name.Should().Be(expectedEntity.Name);
    }

    #endregion

    #endregion

    #region GetAll

    #region Error

    [TestMethod]
    public void GetAll_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Func<List<EntityTest>> act = () => _repository.GetAll();

        act.Should().Throw<DataAccessException>().WithMessage("Error getting entities from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        var expectedEntity = new EntityTest { Name = "dummy" };

        _repository.Add(expectedEntity);
        _context.SaveChanges();

        List<EntityTest> entitiesSaved = _repository.GetAll();

        entitiesSaved.Count.Should().Be(1);

        EntityTest entitySaved = entitiesSaved[0];
        entitySaved.Id.Should().Be(expectedEntity.Id);
        entitySaved.Name.Should().Be(expectedEntity.Name);
    }

    [TestMethod]
    public void GetAll_WhenExistMultiple_ShouldReturnAll()
    {
        var expectedEntity1 = new EntityTest { Name = "dummy1" };
        var expectedEntity2 = new EntityTest { Name = "dummy2" };

        _repository.Add(expectedEntity1);
        _repository.Add(expectedEntity2);
        _context.SaveChanges();

        List<EntityTest> entitiesSaved = _repository.GetAll();

        entitiesSaved.Count.Should().Be(2);

        EntityTest entitySaved1 = entitiesSaved[0];
        entitySaved1.Id.Should().Be(expectedEntity1.Id);
        entitySaved1.Name.Should().Be(expectedEntity1.Name);

        EntityTest entitySaved2 = entitiesSaved[1];
        entitySaved2.Id.Should().Be(expectedEntity2.Id);
        entitySaved2.Name.Should().Be(expectedEntity2.Name);
    }

    [TestMethod]
    public void GetAll_WithPredicate_ReturnsFilterEntities()
    {
        var expectedEntity1 = new EntityTest { Name = "dummy1" };
        var expectedEntity2 = new EntityTest { Name = "dummy2" };

        _repository.Add(expectedEntity1);
        _repository.Add(expectedEntity2);
        _context.SaveChanges();

        List<EntityTest> entitiesSaved = _repository.GetAll(e => e.Name == "dummy1");

        entitiesSaved.Count.Should().Be(1);

        EntityTest entitySaved = entitiesSaved[0];
        entitySaved.Id.Should().Be(expectedEntity1.Id);
        entitySaved.Name.Should().Be(expectedEntity1.Name);
    }

    [TestMethod]
    public void GetAll_WhenNoEntity_ShouldReturnEmptyList()
    {
        List<EntityTest> entitiesSaved = _repository.GetAll();

        entitiesSaved.Count.Should().Be(0);
    }

    #endregion

    #endregion

    #region GetAllPaged

    #region Error

    [TestMethod]
    public void GetAllPaged_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Func<List<EntityTest>> act = () => _repository.GetAll(null, 0, 10);

        act.Should().Throw<DataAccessException>().WithMessage("Error getting entities from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetAllPaged_WhenLimitIsOne_ShouldReturnOne()
    {
        var expectedEntity1 = new EntityTest { Name = "dummy1" };
        var expectedEntity2 = new EntityTest { Name = "dummy2" };

        _repository.Add(expectedEntity1);
        _repository.Add(expectedEntity2);
        _context.SaveChanges();

        List<EntityTest> entitiesSaved = _repository.GetAll(null, 0, 1);

        entitiesSaved.Count.Should().Be(1);

        EntityTest entitySaved = entitiesSaved[0];
        entitySaved.Id.Should().Be(expectedEntity1.Id);
        entitySaved.Name.Should().Be(expectedEntity1.Name);
    }

    [TestMethod]
    public void GetAllPaged_WhenOffsetIsOne_ShouldReturnSecond()
    {
        var expectedEntity1 = new EntityTest { Name = "dummy1" };
        var expectedEntity2 = new EntityTest { Name = "dummy2" };

        _repository.Add(expectedEntity1);
        _repository.Add(expectedEntity2);
        _context.SaveChanges();

        List<EntityTest> entitiesSaved = _repository.GetAll(null, 1, 10);

        entitiesSaved.Count.Should().Be(1);

        EntityTest entitySaved = entitiesSaved[0];
        entitySaved.Id.Should().Be(expectedEntity2.Id);
        entitySaved.Name.Should().Be(expectedEntity2.Name);
    }

    #endregion

    #endregion

    #region Dummy Entity

    internal sealed class TestDbContext(DbContextOptions options)
        : DbContext(options)
    {
        public DbSet<EntityTest> EntitiesTest { get; set; }
    }

    internal sealed record EntityTest()
    {
        public EntityTest(string name)
            : this()
        {
            Name = name;
        }

        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = null!;
    }

    #endregion
}
