using FluentAssertions;
using ModeloValidador.Abstracciones;
using SmartHome.BusinessLogic.AssemblyManagement;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Tests.ServicesTests;

[TestClass]
public class ModelValidatorServiceTest
{
    private readonly string _destinationPath =
        AppDomain.CurrentDomain.BaseDirectory + Constant.ValidatorsPathAddedToCurrent;

    private readonly string _sourcePath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "ServicesTests", "Validators");

    private ModelValidatorService _service = null!;

    [TestInitialize]
    public void Initialize()
    {
        MoveValidator(_sourcePath, _destinationPath);

        _service = new ModelValidatorService();
    }

    [TestCleanup]
    public void Cleanup()
    {
        MoveValidator(_destinationPath, _sourcePath);
    }

    private static void MoveValidator(string sourcePath, string destinationPath)
    {
        if (Directory.Exists(sourcePath))
        {
            Directory.Move(sourcePath, destinationPath);
        }
    }

    private static Guid GetValidatorId(string validatorName)
    {
        var testValidatorsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Validators");
        var loadAssembly = new LoadAssembly<IModeloValidador>(testValidatorsPath);
        loadAssembly.GetImplementations();
        return loadAssembly.GetImplementationIdByName(validatorName);
    }

    #region IsValidModel

    [TestMethod]
    public void IsValidModel_WhenValidatorReturnsTrue_ShouldReturnTrue()
    {
        Guid trueValidatorId = GetValidatorId("TrueValidatorTest");

        var result = _service.IsValidModel(trueValidatorId, "test");

        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsValidModel_WhenValidatorReturnsFalse_ShouldReturnFalse()
    {
        Guid falseValidatorId = GetValidatorId("FalseValidatorTest");

        var result = _service.IsValidModel(falseValidatorId, "test");

        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsValidModel_WhenThrowsException_ShouldReturnFalse()
    {
        var validatorId = Guid.NewGuid();

        var result = _service.IsValidModel(validatorId, "test");

        result.Should().BeFalse();
    }

    #endregion

    #region ValidatorIdIsValid

    [TestMethod]
    public void ValidatorIdIsValid_WhenValidatorIdIsValid_ShouldReturnTrue()
    {
        Guid trueValidatorId = GetValidatorId("TrueValidatorTest");

        var result = _service.ValidatorIdIsValid(trueValidatorId);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ValidatorIdIsValid_WhenValidatorIdIsNotValid_ShouldReturnFalse()
    {
        var validatorId = Guid.NewGuid();

        var result = _service.ValidatorIdIsValid(validatorId);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ValidatorIdIsValid_WhenThrowsException_ShouldReturnFalse()
    {
        var validatorId = Guid.NewGuid();

        var result = _service.ValidatorIdIsValid(validatorId);

        result.Should().BeFalse();
    }

    #endregion

    #region GetImplementations

    #region Error

    [TestMethod]
    public void GetImplementations_WhenThrowsException_ShouldThrowAssemblyException()
    {
        MoveValidator(_destinationPath, _sourcePath);

        try
        {
            Action act = () => _service.GetImplementations();
            act.Should().Throw<AssemblyException>();
        }
        finally
        {
            MoveValidator(_sourcePath, _destinationPath);
        }
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetImplementations_WhenNoErrors_ShouldReturnListOfShowModelValidatorsDtoWithCorrectIds()
    {
        List<ShowModelValidatorsDto> result = _service.GetImplementations();

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().HaveCount(2);
        result.Should()
            .ContainSingle(x => x.Name == "TrueValidatorTest" && x.Id == GetValidatorId("TrueValidatorTest"));
        result.Should()
            .ContainSingle(x => x.Name == "FalseValidatorTest" && x.Id == GetValidatorId("FalseValidatorTest"));
    }

    #endregion

    #endregion
}
