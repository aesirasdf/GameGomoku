# GameGomoku API

## Overview

This document provides a brief overview of the Gomoku API for evaluation purposes. The API allows users to interact with a simplified Gomoku game. It features two primary endpoints: `/Game/Board` and `/Game/Stone`.

## Endpoints

- **Create Game Board**: 
  - Endpoint: `POST /Game/Board`
  - Description: Create a new Gomoku game board.
- **Place Stone (Make a Move)**:
  - Endpoint: `POST /Game/Stone`
  - Description: Place a stone (make a move) on a Gomoku board.

## Usage

1. Create a new Gomoku game board using the `POST /Game/Board` endpoint.
2. Place stones (make moves) on the board using the `POST /Game/Stone` endpoint.

## Testing

You can test the API by making HTTP requests to the provided endpoints. Ensure that you follow the API's request and response format, as specified in the API documentation.