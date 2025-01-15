using AventStack.ExtentReports;
using Microsoft.Extensions.Configuration;
using TrelloAutomation.Pages;

namespace TrelloAutomation.Tests;

public class TrelloTest : BaseTest
{
    private LoginPage _loginPage;
    private BoardsPage _boardsPage;

    /// <summary>
    /// Initializes page objects before each test, ensuring a fresh instance for test isolation.
    /// </summary>
    [SetUp]
    public void TestSetUp()
    {
        _loginPage = new LoginPage(page);
        _boardsPage = new BoardsPage(page);
    }

    /// <summary>
    /// Verifies that a user can log in with valid credentials and navigate to the boards page.
    /// </summary>
    [Test, Order(1)]
    public async Task LoginWithValidUserCredentialsAsync()
    {
        test = extent.CreateTest("Login with Valid User Credentials");
        try
        {
            // Arrange.
            var expected = "Boards | Trello";
            var username = configuration.GetValue<string>("Trello:Email") ?? throw new ArgumentNullException();
            var password = configuration.GetValue<string>("Trello:Password") ?? throw new ArgumentNullException();

            // Act.
            test.Log(Status.Info, "Performing user loggin.");
            await _loginPage.LoginAsync(username, password);
            

            test.Log(Status.Info, "Waiting for Boards page to load.");
            await page.WaitForURLAsync("**/boards");

            // Assert.
            Assert.That(await _loginPage.PageTitleAsync(), Is.EqualTo(expected));
        }
        catch (Exception ex)
        {
            test.Fail($"Login test failed due to an error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Creates a new Trello board and verifies that the board name matches the expected name.
    /// </summary>
    [Test, Order(2)]
    public async Task CreateTrelloBoardAsync()
    {
        test = extent.CreateTest("Create Trello Board");
        try
        {
            //Arrange.
            var expectedBoard = "Bob's Pool Maintenance";

            //Act.
            test.Log(Status.Info, "Creating new board.");
            await _boardsPage.CreatBoardAsync(expectedBoard);

            //Assert.
            Assert.That(await _boardsPage.GetBoardNameAsync(), Is.EqualTo(expectedBoard));
        }
        catch (Exception ex)
        {
            test.Fail($"Create board test failed due to an error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Adds a list to the current board with the specified name and 
    /// verifies that the list name matches the expected name.
    /// </summary>
    /// <param name="listName">Selected List name</param>
    [Test, Order(3)]
    [TestCase("Prospects")]
    [TestCase("In Progress")]
    public async Task AddAListOnBoardAsync(string listName)
    {
        try
        {
            test = extent.CreateTest($"Add List on Board: {listName}");
            //Arrange.
            var expectedListName = listName;

            //Await.
            test.Log(Status.Info, "Adding a new list to the board.");
            await _boardsPage.AddListToBoard(listName);

            // Assert.
            Assert.That(await _boardsPage.GetListNameAsync(listName), Is.EqualTo(expectedListName));
        }
        catch (Exception ex)
        {
            test.Fail($"Add list test failed due to an error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Adds a card with the specified name to the specified list on the board and
    /// verifies that the card name matches with the expected name.
    /// </summary>
    /// <param name="listName">Selected list name</param>
    /// <param name="cardName">Selected card name</param>
    [Test, Order(4)]
    [TestCase("Prospects", "Public Pool Amsterdam Renovation")]
    [TestCase("In Progress", "New Public Pool - Delft")]
    public async Task AddACardToTheListAsync(string listName, string cardName)
    {
        try
        {
            test = extent.CreateTest($"Add Card '{cardName}' to List '{listName}'");
            //Arrange.
            var expectedCardName = cardName;

            //Act.
            test.Log(Status.Info, "Adding a new card to the list.");
            await _boardsPage.AddCardToList(listName, expectedCardName);

            //Assert.
            Assert.That(await _boardsPage.GetCardNameAsync(cardName), Is.EqualTo(expectedCardName));
        }
        catch (Exception ex)
        {
            test.Fail($"Add card test failed due to an error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Adds a start and remove the Due date to a card on the board and verifies the displayed date.
    /// </summary>
    [Test, Order(5)]
    public async Task AddADateToTheCardAsync()
    {
        test = extent.CreateTest($"Add date to the card");
        
        try
        {
            //Arrange.
            var cardName = "Public Pool Amsterdam Renovation";
            var expectedDateInList = DateTime.Today.ToString("MMM dd");

            //Act.
            test.Log(Status.Info, "Adding a date to the card.");
            await _boardsPage.AddDateToCardAsync(cardName);

            test.Log(Status.Info, "Getting start date.");
            var actualDate = await _boardsPage.GetStartedDateInListAsync();

            //Assert.
            Assert.That(actualDate, Does.Contain($"{expectedDateInList}"));
        }
        catch (Exception ex)
        {
            test.Fail($"Add date test failed due to an error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Drags and drops a card between the selected lists and verifies its position.
    /// </summary>
    [Test, Order(6)]
    public async Task DragAndDropCardBetweenListsAsync()
    {
        test = extent.CreateTest($"Drag a card between lists");

        try
        {
            // Arrange.
            var cardName = "Public Pool Amsterdam Renovation";
            var dragDestinationList = "In Progress";

            // Act.
            test.Log(Status.Info, "Drag a card between lists.");
            await _boardsPage.DragCardBetweenListsAsync(cardName, dragDestinationList);

            test.Log(Status.Info, "Take current position of the selected card.");
            var currentCardPosition = await _boardsPage.GetCardIndexInListAsync(cardName, dragDestinationList);

            test.Log(Status.Info, "Get total number of cards in the list.");
            var numberOfCards = await _boardsPage.GetNumberOfCardsInListAsync(dragDestinationList);

            // Assert.
            Assert.Multiple(() =>
            {
                Assert.That(currentCardPosition, Is.EqualTo(1));
                Assert.That(numberOfCards, Is.EqualTo(2));
            });
        }
        catch (Exception ex)
        {
            test.Fail($"Drag and drop between lists test failed: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Drag and move a card within the selected list.
    /// </summary>
    [Test, Order(7)]
    public async Task DragAndDropCardWithinListAsync()
    {
        test = extent.CreateTest($"Move a selected card within a list");

        try
        {
            // Arrange.
            var cardName = "Public Pool Amsterdam Renovation";
            var listName = "In Progress";

            // Act.
            test.Log(Status.Info, "Move a card within a selected list.");
            await _boardsPage.DragCardWithinListAsync(cardName, listName);

            test.Log(Status.Info, "Take current position of the selected card.");
            var currentCardPosition = await _boardsPage.GetCardIndexInListAsync(cardName, listName);

            test.Log(Status.Info, "Get total number of cards in the list.");
            var numberOfCards = await _boardsPage.GetNumberOfCardsInListAsync(listName);
            
            // Assert.
            Assert.Multiple(() =>
            {
                Assert.That(currentCardPosition, Is.EqualTo(0));
                Assert.That(numberOfCards, Is.EqualTo(2));
            });
        }
        catch (Exception ex)
        {
            test.Fail($"Move card test failed: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Archives a card in a list and verifies it is no longer visible in the list.
    /// </summary>
    [Test, Order(8)]
    public async Task ArchiveCardInListAsync()
    {
        test = extent.CreateTest($"Move a selected card within a list");

        try
        {
            // Arrange.
            var cardName = "New Public Pool - Delft";
            var listName = "In Progress";

            //Act.
            test.Log(Status.Info, "Archive a card from the list.");
            await _boardsPage.ArchiveCardInListAsync(cardName);

            test.Log(Status.Info, "Get total number of cards in the list.");
            var numberOfCards = await _boardsPage.GetNumberOfCardsInListAsync(listName);
            
            // Assert.
            Assert.Multiple(async () =>
            {
                Assert.That(numberOfCards, Is.EqualTo(1));
                Assert.That(await _boardsPage.IsCardVisibleInListAsync(cardName), Is.False);
            });
        }
        catch (Exception ex)
        {
            test.Fail($"Archive card test failed: {ex.Message}");
            throw;
        }
    }
}
