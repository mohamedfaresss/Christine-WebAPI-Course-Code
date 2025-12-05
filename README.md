# .NET Web API Playlist Code (Rewritten & Organized)
  
  Rewritten and organized code from Eng. Christine's .NET Web API YouTube playlist.
  This repo is meant to provide clear, clean, study-friendly code for anyone following the playlist.
  
  Playlist link:
  https://youtube.com/playlist?list=PLNFDrRZdysFwWljltjUvYN6O6jajgOJME
  
  This repository is NOT an original project.
  It is a rewritten and organized version of the example code shown in the playlist.
  The purpose is to help learners study, take notes, and understand Web API concepts more clearly.
  
  Technologies used:
  ASP.NET Core Web API, Entity Framework Core, SQL Server, LINQ, Dependency Injection, JWT Authentication, RESTful API structure, Visual Studio 2022.
  
## 🚨 Important Before Running
  
  You MUST update the following in appsettings.json:
  
### 🔹 SQL Server Connection String
Replace with your own database connection:
  
  ```json
"ConnectionStrings": {
  "DefaultConnection": "Your SQL Server connection here"
}
```
  
### 🔹 JWT Options
  You must set your own JWT secret and values:
 ```json
"Jwt": {
  "Key": "YOUR_SECRET_KEY",
  "Issuer": "YOUR_ISSUER",
  "Audience": "YOUR_AUDIENCE",
  "DurationInDays": 30
}
```
  
  The values inside the repo are placeholders only for learning.
  
 ## 🚀 How to Run
  
  Clone the repository:
  
  git clone https://github.com/mohamedfaresss/Christine-WebAPI-Course-Code.git
  
  Open the solution in Visual Studio.
  
  Update:
  
  Connection String
  
  JWT Options
  
  Apply database migrations:
  
  dotnet ef database update
  
  Run the project and open Swagger:
  
  https://localhost
  :<port>/swagger
  
  This repo exists only for educational purposes.
  Full credit for the teaching videos belongs to Eng. Christine.
  All code here was rewritten manually for clearer studying.
  
  Author: Mohamed Fares
