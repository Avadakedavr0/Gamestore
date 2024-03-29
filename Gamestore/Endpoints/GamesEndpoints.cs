using Gamestore.Dtos;
using GameStore.Dtos;

namespace GameStore.Endpoints;

public static class GamesEndpoints 
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
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

    public static WebApplication MapGamesEndpoints(this WebApplication app)
    {
        // GET /games
        app.MapGet("games", () => games);

        // Games /games/1
        app.MapGet("games/{id}", (int id) => 
        {
            GameDto? game = games.Find(game => game.Id == id);
            
            return game is null ? Results.NotFound() : Results.Ok(game);
        })

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

            if (index == -1) 
            {
                return Results.NotFound();
            }

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

        return app;
    }
}