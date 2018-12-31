# Stocqres

Stocqres is a stock exchange implementation in ASP.NET Core. I've decided to focus more on using different technologies and new architecture rather than develop business logic. So in the end, the user can only log in or register to the application and buy or sell stock from a stock exchange.

## Technology and Architecture

### Backend
In this project I have used:
1. ASP.NET Core 2.1 WebApi
2. Docker
3. MongoDb
4. Sql Server

Databases and main application are in docker containers. Everything is wrapped in docker-compose, so it is easy to run.

On the architecture side, I've used CQRS pattern with Event Sourcing. I decided to implement it by myself. This solution is only for development purpose, and it is still growing. So please, do not copy it to production, it would be a distaster.

### Frontend
It is the first time when I used React-Native for frontend purpose. I decided to make only a mobile application, but thanks to redux, it could be reused for the web application, but I don't plan it.

The react-native app include:
1. React-native-navigation
2. Redux with redux-persist and redux-thunk
3. NativeBase for components

The mobile app is in folder StocqresMobile.

## How to run it?

To run up the project you must enter to the main folder and use the command `docker-compose up`. That's it.
