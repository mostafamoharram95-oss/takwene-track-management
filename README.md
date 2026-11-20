# Takwene Track Management

A full-stack track management application built as part of the Takwene Full-Stack Software Engineer technical assignment.

The application manages artists, music tracks, and distribution to Digital Service Providers (DSPs). It provides a REST API for creating and managing tracks, plus an Angular frontend for browsing tracks and viewing distribution statuses.

## Technology Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core Web API (.NET 10, C#) |
| Frontend | Angular 22, TypeScript |
| Database | PostgreSQL 17 |
| ORM | Entity Framework Core 10 |
| Database environment | Docker Compose |
| Authentication | JWT Bearer |
| Architecture | Clean Architecture |

## Project Structure

```text
takwene-track-management/
├── backend/
│   └── src/
│       ├── Takwene.TrackManagement.Domain/
│       ├── Takwene.TrackManagement.Application/
│       ├── Takwene.TrackManagement.Infrastructure/
│       └── Takwene.TrackManagement.Api/
├── frontend/
├── compose.yaml
├── .env.example
├── DECISIONS.md
└── README.md
```

The backend separates responsibilities into four projects:

- **Domain:** Entities, enumerations, and core domain models.
- **Application:** DTOs, interfaces, business logic, and services.
- **Infrastructure:** EF Core database context, repositories, migrations, and seeding.
- **API:** Controllers, authentication, dependency injection, and HTTP configuration.

## Prerequisites

Install the following tools before running the project:

- .NET SDK 10
- Node.js 24 and npm
- Docker and Docker Compose
- Git

The Angular CLI can be executed through `npx` without a global installation.

For database migrations, install the EF Core CLI if it is not already available:

```bash
dotnet tool install --global dotnet-ef --version 10.0.0
```

## 1. Clone the Repository

```bash
git clone https://github.com/mostafamoharram95-oss/takwene-track-management.git

cd takwene-track-management
```

## 2. Start PostgreSQL

Copy the example environment file:

```bash
cp .env.example .env
```

Open `.env` and configure the PostgreSQL credentials.

Start the database:

```bash
docker compose up -d
```

The Docker Compose configuration exposes PostgreSQL on port `5433` on the local machine.

The development database is named `track_management`.

**Security:** The local `.env` file contains development credentials and must not be committed to Git. Do not use the example credentials for production deployments.

## 3. Configure the Backend Database Connection

Navigate to the backend directory:

```bash
cd backend
```

Restore the .NET dependencies:

```bash
dotnet restore
```

Initialize ASP.NET Core User Secrets if they have not already been configured:

```bash
dotnet user-secrets init \
  --project src/Takwene.TrackManagement.Api
```

Configure the database connection string:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Host=localhost;Port=5433;Database=track_management;Username=track_admin;Password=YOUR_PASSWORD" \
  --project src/Takwene.TrackManagement.Api
```

Replace `YOUR_PASSWORD` with the PostgreSQL password configured in `.env`. If you change the PostgreSQL username or database name in the Docker configuration, update the connection string accordingly.

The API reads its database connection from the `ConnectionStrings:DefaultConnection` configuration key.

## 4. Apply EF Core Migrations

From the `backend` directory, execute:

```bash
dotnet ef database update \
  --project src/Takwene.TrackManagement.Infrastructure \
  --startup-project src/Takwene.TrackManagement.Api \
  --context ApplicationDbContext
```

This applies the database migrations and runs the configured EF Core seeding logic.

### Seed Data

A fresh database is populated with:

| Entity | Initial records |
|---|---:|
| Artists | 3 |
| Tracks | 8 |
| DSPs | 3 |

The initial artists are Layla Mansour, Omar Haddad, and Nour Elsayed.

The DSPs are Spotify, Apple Music, and YouTube Music.

The eight tracks cover different genres and lifecycle statuses: `Draft`, `Submitted`, and `Distributed`.

Distribution records can be created through the distribution API endpoint. A newly seeded track without distribution records will display an empty distribution list.

## 5. Run the Backend

From `backend`:

```bash
ASPNETCORE_ENVIRONMENT=Development dotnet run \
  --project src/Takwene.TrackManagement.Api \
  --no-launch-profile -- --urls http://localhost:5080
```

The API is available at:

```text
http://localhost:5080
```

Verify the Track List endpoint:

```bash
curl http://localhost:5080/api/tracks
```

## 6. Run the Angular Frontend

Open a second terminal at the repository root:

```bash
cd frontend

npm install

npx ng serve --port 4200
```

Open:

http://localhost:4200

The frontend communicates with the API at `http://localhost:5080`.

The backend's development CORS policy permits requests from `http://localhost:4200`.

### Frontend Views

**Track List**

- Displays track title, artist name, genre, and status.
- Allows filtering tracks by status.
- Provides navigation to individual track details.

**Track Detail**

- Displays track title, artist, ISRC, genre, release date, and status.
- Displays associated DSP distribution records and their statuses.
- Handles tracks without distribution records.
- Provides navigation back to the Track List.

No frontend login screen is required for these read-only views.

## 7. API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/artists` | Create an artist |
| GET | `/api/artists` | List artists |
| POST | `/api/tracks` | Create a track |
| GET | `/api/tracks` | List and filter tracks |
| GET | `/api/tracks/{id}` | Get a track with DSP distribution details |
| POST | `/api/tracks/{id}/distribute` | Submit a track to selected DSPs |
| PATCH | `/api/tracks/{id}/status` | Update a track's lifecycle status |

### Track Filtering

The Track List endpoint supports optional query parameters:

- `artistId`
- `genre`
- `status`

Example:

```bash
curl "http://localhost:5080/api/tracks?genre=Pop&status=Draft"
```

### Track Status Values

Track lifecycle statuses are:

| Value | Status |
|---|---|
| 0 | Draft |
| 1 | Submitted |
| 2 | Distributed |

The API accepts numeric enum values where applicable. List and detail responses display readable status names.

DSP distribution statuses are `Pending`, `Live`, and `Rejected`.

## 8. JWT Authentication

The endpoint below requires a valid JWT bearer token:

```text
POST /api/tracks/{id}/distribute
```

The project uses ASP.NET Core's development JWT tooling to demonstrate endpoint protection without implementing a complete account-management or login system.

### Generate a Development Token

From the `backend` directory:

```bash
JWT=$(dotnet user-jwts create \
  --project src/Takwene.TrackManagement.Api \
  --audience http://localhost:5080 \
  --valid-for 1h \
  --output token)
```

Restart the backend after generating the first development JWT so its configuration is loaded.

### Test Without a Token

```bash
curl -i -X POST http://localhost:5080/api/tracks/1/distribute \
  -H "Content-Type: application/json" \
  -d '{"dspIds":[1]}'
```

Expected response:

```text
401 Unauthorized
```

### Test With a Valid Token

In the same terminal where the `JWT` variable was created:

```bash
curl -i -X POST http://localhost:5080/api/tracks/1/distribute \
  -H "Authorization: Bearer $JWT" \
  -H "Content-Type: application/json" \
  -d '{"dspIds":[1]}'
```

For a freshly seeded database, the expected response is:

```text
204 No Content
```

This creates a pending distribution record for the selected DSP. Repeating the same request will return a conflict because the track/DSP combination already exists.

The authentication behavior was tested with no token, an invalid token, and a valid token.

After testing, clear the shell variable:

```bash
unset JWT
```

**Note:** Development JWTs are for local testing. Production authentication would require a trusted identity provider and appropriately configured token validation.

## 9. Validation and Error Handling

The backend validates requests and uses meaningful HTTP response codes, including:

| Status | Meaning |
|---|---|
| 400 Bad Request | Invalid request or input |
| 401 Unauthorized | Missing or invalid JWT |
| 404 Not Found | Requested track does not exist |
| 409 Conflict | Duplicate ISRC or conflicting distribution |
| 201 Created | Resource created successfully |
| 204 No Content | Update or distribution submission completed |

Database constraints enforce unique ISRC values and prevent duplicate track/DSP associations.

Invalid artist references, invalid track statuses, and duplicate ISRC submissions have been tested.

## 10. Build Verification

Build the backend from the repository root:

```bash
dotnet build backend/src/Takwene.TrackManagement.Api/Takwene.TrackManagement.Api.csproj
```

Build the Angular frontend:

```bash
cd frontend

npm install

npx ng build
```

Both projects were successfully built during development.

The API was also tested through HTTP requests and direct PostgreSQL queries to verify persistence and status updates.

## 11. AI Usage and Engineering Decisions

AI was used for architectural suggestions, example implementations, troubleshooting, and code review.

All AI contributions, security considerations, and a specific AI mistake that was identified and corrected are documented in:

[DECISIONS.md](DECISIONS.md)

The implementation was reviewed and tested during development, including correcting an initial ISRC validation limitation and handling PostgreSQL unique-constraint errors appropriately.

## 12. Scope

This project implements the functionality requested for the technical assignment.

It does not include user registration, a full authentication UI, production identity management, or additional administrative frontend screens. These are outside the requested scope.