using GameGomoku.Constants;
using GameGomoku.Interfaces;
using GameGomoku.Models;
using GameGomoku.Services;

namespace GameGomoku.Tests
{
    [TestFixture]
    public sealed class GameServiceTests
    {
        private readonly IGameService _gameService;
        private const int ConnectedFiveRows = 4;

        public GameServiceTests()
        {
            _gameService = new GameService();
        }

        [Test]
        public void ShouldSuccessWhenThereIsWinner()
        {
            _ = _gameService.CreateBoard(15, 15, 1).Result;
            _ = _gameService.CreateStone(1, 1).Result;
            _ = _gameService.CreateStone(9, 7).Result; // player 2's turn
            _ = _gameService.CreateStone(1, 2).Result;
            _ = _gameService.CreateStone(8, 1).Result; // player 2's turn
            _ = _gameService.CreateStone(1, 3).Result;
            _ = _gameService.CreateStone(3, 14).Result; // player 2's turn
            _ = _gameService.CreateStone(1, 4).Result;
            _ = _gameService.CreateStone(8, 14).Result; // player 2's turn
            var response = _gameService.CreateStone(1, 5).Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Ok, Is.EqualTo(true));
            Assert.That(response.Message, Is.EqualTo("Player 1 with Black stone wins"));
            Assert.That(response.Result, Is.EqualTo(GlobalConstants.GameEnded));
        }

        [Test]
        public void ShouldSuccessWhenBoardIsCreated()
        {
            var response = _gameService.CreateBoard(15, 15, 1).Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Ok, Is.EqualTo(true));
            Assert.That(response.Message, Is.EqualTo(GlobalConstants.BoardCreated));
            Assert.That(response.Result, Is.EqualTo(GlobalConstants.Success));
        }

        [Test]
        public void ShouldFailWhenPlacingStoneWithoutCreatedBoard()
        {
            var board = new GameService();
            var response = board.CreateStone(1, 1).Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Ok, Is.EqualTo(false));
            Assert.That(response.Message, Is.EqualTo(GlobalConstants.BoardNotExist));
            Assert.That(response.Result, Is.EqualTo(GlobalConstants.Invalid));
        }

        [Test]
        public void ShouldFailWhenPlacingStoneOutsideTheBoard()
        {
            _ = _gameService.CreateBoard(5, 5, 1).Result;

            var response = _gameService.CreateStone(6, 6).Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Ok, Is.EqualTo(false));
            Assert.That(response.Message, Is.EqualTo(GlobalConstants.StoneOutOfBounds));
            Assert.That(response.Result, Is.EqualTo(GlobalConstants.Invalid));
        }

        [Test]
        [TestCase(1, 1, 1)]
        [TestCase(2, 7, 2)]
        [TestCase(1, 2, 3)]
        [TestCase(2, 4, 8)]
        [TestCase(1, 6, 2)]
        [TestCase(2, 1, 8)]
        public void ShouldSuccessWhenPlacingStoneWithValidMove(int playerId, int rowNumber, int columnNumber)
        {
            _ = _gameService.CreateBoard(15, 15, 1).Result;

            var response = _gameService.CreateStone(rowNumber, columnNumber).Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Ok, Is.EqualTo(true));
            Assert.That(response.Message, Contains.Substring("took his turn and placed a"));
            Assert.That(response.Result, Is.EqualTo(GlobalConstants.NextTurn));
        }

        [Test]
        public void ShouldFailWhenPlacingStoneInExisitingPosition()
        {
            _ = _gameService.CreateBoard(15, 15, 1).Result;

            _ = _gameService.CreateStone(6, 6).Result;
            var response = _gameService.CreateStone(6, 6).Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Ok, Is.EqualTo(false));
            Assert.That(response.Message, Is.EqualTo(GlobalConstants.StoneAlreadyExists));
            Assert.That(response.Result, Is.EqualTo(GlobalConstants.Invalid));
        }

    }
}
