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
        [Route("Board")]
        public async Task<IActionResult> CreateBoard(int startingPlayer = 1)
        {
            var response = new APIResponse();
            if (startingPlayer.Equals(1) || startingPlayer.Equals(2))
            {
                response = await _gameService.CreateBoard(15, 15, startingPlayer);
                return new OkObjectResult(response);
            }
            response.Message = GlobalConstants.InvalidStartingPlayer;
            response.Result = GlobalConstants.Invalid;
            return new BadRequestObjectResult(response);
        }


        [HttpPost]
        [Route("Stone")]
        public async Task<IActionResult> Stone(int column, int row)
        {
            var response = _gameService.CreateStone(column, row).Result;
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
