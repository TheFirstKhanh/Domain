using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleCode.Controllers;


namespace SampleCode.Api.Tests;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();
    private readonly UsersController _usersController;

    public UsersControllerTests()
    {
        _setupMockMediator(new GetUserSummaryQueryResponse());

        _usersController = new UsersController(_mockMediator.Object);
    }


    [Fact]
    public async Task GetData_DataNotNull_Return200()
    {

        // Act

        ActionResult<GetUserSummaryQueryResponse> testResponse =
                Assert.IsType<ActionResult<GetUserSummaryQueryResponse>>
                (await this._usersController.GetData());

        OkObjectResult returnValue = Assert.IsType<OkObjectResult>(testResponse.Result);

        // Assert

        Assert.Equal(StatusCodes.Status200OK, returnValue.StatusCode);
        GetUserSummaryQueryResponse getUserSummaryQueryResponse = Assert.IsType<GetUserSummaryQueryResponse>(returnValue.Value);

    }

    [Fact]
    public async Task GetData_DataIsNull_Return404()
    {
        // Assign
        _setupMockMediator(null);

        // Act

        ActionResult<GetUserSummaryQueryResponse> testResponse =
                Assert.IsType<ActionResult<GetUserSummaryQueryResponse>>
                (await this._usersController.GetData());

        StatusCodeResult returnValue = Assert.IsType<StatusCodeResult>(testResponse.Result);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, returnValue.StatusCode);

    }

    [Fact]
    public async Task GetData_ThrowExcaption_Return500()
    {
        // Assign

        _mockMediator.Setup(x => x.Send(It.IsAny<GetUserSummaryQuery>(), CancellationToken.None))
                        .Throws(new SystemException());

        // Act

        ActionResult<GetUserSummaryQueryResponse> testResponse =
                Assert.IsType<ActionResult<GetUserSummaryQueryResponse>>
                (await this._usersController.GetData());

        ObjectResult returnValue = Assert.IsType<ObjectResult>(testResponse.Result);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, returnValue.StatusCode);

    }

    [Fact]
    public async Task GetData_DataNotNull_ReturnValidResponse()
    {
        // Assign
        _setupMockMediator(QueryResponseFactory.CreateGetUserSummaryQueryResponse());

        // Act

        ActionResult<GetUserSummaryQueryResponse> testResponse =
                Assert.IsType<ActionResult<GetUserSummaryQueryResponse>>
                (await this._usersController.GetData());

        OkObjectResult returnValue = Assert.IsType<OkObjectResult>(testResponse.Result);
        GetUserSummaryQueryResponse getUserSummaryQueryResponse = Assert.IsType<GetUserSummaryQueryResponse>(returnValue.Value);

        // Assert

        Assert.Equal("Test FirstName", getUserSummaryQueryResponse.FirstName);
        Assert.True(QueryResponseFactory.CreateListUserFullName.SequenceEqual(getUserSummaryQueryResponse.UserFullName));
        Assert.True(QueryResponseFactory.CreateListGenderPerAge.SequenceEqual(getUserSummaryQueryResponse.GenderPerAges));

    }

    private void _setupMockMediator(GetUserSummaryQueryResponse? getUserSummaryQueryResponse)
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<GetUserSummaryQuery>(), CancellationToken.None))
                            .ReturnsAsync(getUserSummaryQueryResponse);
    }
}