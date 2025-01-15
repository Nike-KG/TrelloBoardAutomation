using Microsoft.Playwright;

namespace TrelloAutomation.Pages;

public class BoardsPage : BasePage
{   
    //Locators to find elements.
    private ILocator HomeLink => _page.Locator("//span[text()='Home']");
    private ILocator CreateButton => _page.GetByTestId("AddIcon");
    private ILocator CreateBoardButton => _page.Locator("//span[text()='Create board']");
    private ILocator CreateBoardTitleInput => _page.GetByTestId("create-board-title-input");
    private ILocator SubmitCreateBoardButton => _page.GetByTestId("create-board-submit-button");
    private ILocator BoardName => _page.GetByTestId("board-name-display");
    private ILocator AddListButton => _page.GetByTestId("list-composer-button");
    private ILocator AddListTextArea => _page.GetByPlaceholder("Enter list name…");
    private ILocator CreateListButton => _page.GetByTestId("list-composer-add-list-button");
    private ILocator AddCardTextArea => _page.GetByTestId("list-card-composer-textarea");
    private ILocator AddCardToListButton => _page.GetByTestId("list-card-composer-add-card-button");
    private ILocator DatesButton => _page.GetByTestId("card-back-due-date-button");
    private ILocator StartDateCheckBox =>
        _page.Locator("xpath=//label[text()='Start date']/following-sibling::label[@data-testid='clickable-checkbox']");
    private ILocator DueDateCheckBox =>
        _page.Locator("xpath=//label[text()='Due date']/following-sibling::label[@data-testid='clickable-checkbox']");
    private ILocator SaveDateButton => _page.GetByTestId("save-date-button");
    private ILocator CloseCardDetailsButton => _page.GetByLabel("Close dialog");
    private ILocator StartDateInList => _page.Locator("xpath=//li[@data-testid='list-card']//span[@data-testid='badge-due-date-not-completed']/span[last()]");
    private ILocator ArchiveButton => _page.GetByTestId("quick-card-editor-archive");

    //Additional Locators.
    private ILocator ListName(string listName) => _page.Locator($"//h2[@data-testid='list-name' and text()='{listName}']");
    private ILocator AddCardButton(string listName) => _page.Locator("li").Filter(
        new()
        {
            HasText = $"{listName}"
        }).GetByTestId("list-add-card-button");

    private ILocator CardName(string cardName) => _page.Locator($"//*[@data-testid='card-name'and text()='{cardName}']");
    private ILocator AllCardsInList(string listName) =>
        _page.Locator($"xpath=//h2[@data-testid='list-name' and text()='{listName}']/ancestor::div[@data-testid='list']/ol[@data-testid='list-cards']/li");
    
    public BoardsPage(IPage page) : base(page) { }

    // Click Actions
    /// <summary>
    /// Click 'Create' button on the navigation bar.
    /// </summary>
    public async Task ClickCreateButtonAsync() => await CreateButton.ClickAsync();

    /// <summary>
    /// Click 'Create board' option in the dropdown list.
    /// </summary>
    public async Task ClickCreateBoardButtonAsync() => await CreateBoardButton.ClickAsync();

    /// <summary>
    /// Click 'Create' button to submit a new board.
    /// </summary>
    public async Task ClickSubmitCreateBoardButtonAsync() => await SubmitCreateBoardButton.ClickAsync();

    /// <summary>
    /// Click 'Add list' button to submit a new list.
    /// </summary>
    public async Task ClickCreateListButtonAsync() => await CreateListButton.ClickAsync();

    /// <summary>
    /// Click 'Add another list' button to create a new list.
    /// </summary>
    public async Task ClickAddListButtonAsync() => await AddListButton.ClickAsync();

    /// <summary>
    /// Click 'Add a card' button to enter a new card to the selected list.
    /// </summary>
    /// <param name="listName">Selected list name</param>
    public async Task ClickAddCardButtonAsync(string listName) => await AddCardButton(listName).ClickAsync();

    /// <summary>
    /// Click 'Add card' button to add the new card to the selected list.
    /// </summary>
    public async Task ClickCreateCardButtonAsync() => await AddCardToListButton.ClickAsync();

    /// <summary>
    /// Click on the selected card name to open card details window.
    /// </summary>
    /// <param name="cardName">Selected card name</param>
    public async Task ClickCardNameAsync(string cardName) => await CardName(cardName).ClickAsync();

    /// <summary>
    /// Click 'Dates' button on the card details popup window.
    /// </summary>
    public async Task ClickDatesButtonAsync() => await DatesButton.ClickAsync();

    /// <summary>
    /// Click on 'Start date' check box.
    /// </summary>
    public async Task CheckStartDateCheckBoxAsync() => await StartDateCheckBox.ClickAsync();

    /// <summary>
    /// Click on 'Due date' check box. 
    /// </summary>
    public async Task CheckDueDateCheckBoxAsync() => await DueDateCheckBox.ClickAsync();

    /// <summary>
    /// Click on 'Save' button in card details window to save selected date.
    /// </summary>
    public async Task ClickDateSaveButtonAsync() => await SaveDateButton.ClickAsync();

    /// <summary>
    /// Click close button to close card details window.
    /// </summary>
    public async Task ClickCardDetailsCloseButtonAsync() => await CloseCardDetailsButton.ClickAsync();

    /// <summary>
    /// Click 'Archive' button to remove selected cards from the list.
    /// </summary>
    public async Task ClickArchiveButtonAsync() => await ArchiveButton.ClickAsync();

    // Right Click

    /// <summary>
    /// Right-click on the selected card.
    /// </summary>
    /// <param name="cardName">Selected card name</param>
    public async Task RightClickCardNameAsync(string cardName) => await CardName(cardName)
        .ClickAsync(new LocatorClickOptions { Button = MouseButton.Right });

    // Fill Actions

    /// <summary>
    /// Enter the title of the new board.
    /// </summary>
    /// <param name="title">Selected Board name</param>
    public async Task FillBoardTitleAsync(string title) => await CreateBoardTitleInput.FillAsync(title);

    /// <summary>
    /// Enter the name of the list.
    /// </summary>
    /// <param name="title">Name of the list</param>
    public async Task FillAddListTextAreaAsync(string title) => await AddListTextArea.FillAsync(title);

    /// <summary>
    /// Enter the name of the card.
    /// </summary>
    /// <param name="cardName">Name of the card</param>
    public async Task FillAddCardtextAreaAsync(string cardName) => await AddCardTextArea.FillAsync(cardName);

    // Get Texts

    /// <summary>
    /// Get the name of the board.
    /// </summary>
    public async Task<string> GetBoardNameAsync() => await BoardName.InnerTextAsync();

    /// <summary>
    /// Get selected list name.
    /// </summary>
    /// <param name="title">Selected list name</param>
    public async Task<string> GetListNameAsync(string title) => await ListName(title).InnerTextAsync(); 

    /// <summary>
    /// Get selected card name.
    /// </summary>
    /// <param name="cardName">Selected card name</param>
    public async Task<string> GetCardNameAsync(string cardName) => await CardName(cardName).InnerTextAsync();

    /// <summary>
    /// Get the value of start date display in the list.
    /// </summary>
    public async Task<string> GetStartedDateInListAsync() => await StartDateInList.InnerTextAsync();

    // Is Visible

    /// <summary>
    /// Check whether the selected card is visible in the list.
    /// </summary>
    /// <param name="cardName">Selected card name</param>
    public async Task<bool> IsCardVisibleInListAsync(string cardName) => await CardName(cardName).IsVisibleAsync();
    

    // Functionality

    /// <summary>
    /// Add list to the selected board.
    /// </summary>
    /// <param name="listName">Selected list name</param>
    public async Task AddListToBoard(string listName)
    {
        if (await AddListButton.IsVisibleAsync())
        {
            await ClickAddListButtonAsync();
        }

        await FillAddListTextAreaAsync(listName);
        await ClickCreateListButtonAsync();
    }

    /// <summary>
    /// Add cards to the list.
    /// </summary>
    /// <param name="listName">Selected list</param>
    /// <param name="cardName">Card name</param>
    public async Task AddCardToList(string listName, string cardName)
    {
        await ClickAddCardButtonAsync(listName);
        await FillAddCardtextAreaAsync(cardName);
        await ClickCreateCardButtonAsync();
    }

    /// <summary>
    /// Drag and drop cards between lists.
    /// </summary>
    /// <param name="cardName">Selected card name</param>
    /// <param name="dragDestinationListName">Selected destination list</param>
    public async Task DragCardBetweenListsAsync(string cardName, string dragDestinationListName)
        => await CardName(cardName).DragToAsync(AddCardButton(dragDestinationListName));

    /// <summary>
    /// Drag card within the selected list.
    /// </summary>
    /// <param name="cardName">Selected card name</param>
    /// <param name="listName">Selected list name</param>
    public async Task DragCardWithinListAsync(string cardName, string listName)
    {
        var allCards = await AllCardsInList(listName).AllAsync();

        var firstCard = allCards[0];

        await CardName(cardName).DragToAsync(firstCard);
    }

    /// <summary>
    /// Get the position of the selected card in the list.
    /// </summary>
    /// <param name="cardName">Selected card name</param>
    /// <param name="listName">Selected list name</param>
    public async Task<int> GetCardIndexInListAsync(string cardName, string listName)
    {
        var val = await AllCardsInList(listName).AllInnerTextsAsync();
        return val.ToList().FindIndex(x => x.Contains(cardName));
    }

    /// <summary>
    /// Get the count of the cards in the selected list.
    /// </summary>
    /// <param name="listName">Selected list name</param>
    public async Task<int> GetNumberOfCardsInListAsync(string listName)
        => await AllCardsInList(listName).CountAsync();


    public async Task CreatBoardAsync(string boardName)
    {
        await ClickCreateButtonAsync();
        await ClickCreateBoardButtonAsync();
        await FillBoardTitleAsync(boardName);
        await ClickSubmitCreateBoardButtonAsync();
    }


    public async Task AddDateToCardAsync(string cardName)
    {
        await ClickCardNameAsync(cardName);
        await ClickDatesButtonAsync();
        await CheckStartDateCheckBoxAsync();
        await CheckDueDateCheckBoxAsync();
        await ClickDateSaveButtonAsync();
        await ClickCardDetailsCloseButtonAsync();
    }

    public async Task ArchiveCardInListAsync(string cardName)
    {
        await RightClickCardNameAsync(cardName);
        await ClickArchiveButtonAsync();
    }
}
