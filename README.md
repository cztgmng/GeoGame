# GeoGame

GeoGame is a simple browser-based game I created for myself and my friends. The game loads random Google Street View locations, and players must guess where they are by marking the spot on the map. After each guess, the game calculates the distance between the actual location and the player's guess, awarding points accordingly.

## Features

- **Google Street View Integration**: Random Street View locations are loaded for players to guess the location on a map.
- **Login System**: A very simple login system is implemented. Users are manually created in the code.
- **Points System**: Players earn points based on the accuracy of their guesses.
- **Rate Limiter**: To avoid exceeding the free tier limits of the Google Maps API, a basic rate limiter is in place.

## Requirements

- **Google Maps API Key**: You'll need to have your own Google Maps API key to make the game work. Insert the key into the `Game.cshtml` file.
- **Manual User Creation**: There is no user registration system. You need to manually add users in the `Login.cshtml.cs` file.

## Setup Instructions

1. Clone this repository.
2. Obtain a Google Maps API key from the [Google Cloud Console](https://console.cloud.google.com/).
3. Add your Google Maps API key to the `Game.cshtml` file at the specified location.
4. To create users, manually edit the `Login.cshtml.cs` file and define your users there.
5. Deploy the project locally or to a server and start playing!

## Rate Limiting

The game includes a rate limiter to prevent the Google Maps API from exceeding its free usage limits. Be mindful of the Google Maps API quotas when using the game.

---

I hope you and your friends enjoy playing GeoGame!
