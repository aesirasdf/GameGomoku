using Microsoft.AspNetCore.Mvc;
using GameGomoku.Constants;
using GameGomoku.Interfaces;
using GameGomoku.Models;

namespace GameGomoku.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        [Route("CreateBoard")]
        public async Task<IActionResult> Board(int startingPlayer = 1)
        {
            var response = new APIResponse();
            response = await _gameService.CreateBoard(15, 15, startingPlayer);
            switch (response.Result)
            {
                case GlobalConstants.Success:
                    return new OkObjectResult(response);
                case GlobalConstants.Invalid:
                    return new BadRequestObjectResult(response);
                default:
                    return new OkObjectResult(response);
            }
        }


        [HttpPost]
        [Route("CreateStone")]
        public async Task<IActionResult> Stone(int row, int column)
        {
            var response = await _gameService.CreateStone(row, column);
            switch (response.Result)
            {
                case GlobalConstants.GameEnded:
                case GlobalConstants.NextTurn:
                    return new OkObjectResult(response);
                case GlobalConstants.Invalid:
                case GlobalConstants.Exception:
                    return new BadRequestObjectResult(response);
                default:
                    return new OkObjectResult(response);
            }
        }
    }
}
