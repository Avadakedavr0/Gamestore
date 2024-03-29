using Gamestore.Dtos;
using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new (
        1,
        "Street Fighter",
        "Fighting",
        19.99M,
        new DateOnly (1992, 7, 15)),
    new (
        2,
        "Super Mario",
        "Arcade",
        19.99M,
        new DateOnly (1997, 6, 19)),
    new (
        3,
        "FIFA",
        "Sports",
        25.00M,
        new DateOnly (1992, 7, 15)),
];

// GET /games
app.MapGet("games", () => games);

// Games /games/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GetGameEndpointName);

// POST /games
app.MapPost("games", (CreateGameDto newGame) => {
    GameDto game = new GameDto (
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);
    
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, game);
});

// PUT /games/1
app.MapPut("games/{id}", (int id, UpdateGameDto updateGame) => {
    var index = games.FindIndex(game => game.Id == id);

    games[index] = new GameDto(
        id,
        updateGame.Name,
        updateGame.Genre,
        updateGame.Price,
        updateGame.ReleaseDate
    );

    return Results.NoContent();
});

// DELETE /games/1
app.MapDelete("games/{id}", (int id) => {
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.Run();
