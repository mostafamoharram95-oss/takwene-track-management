# DECISIONS.md

## 1. Which parts were AI-generated, and which were personally implemented?

I used ChatGPT as a development assistant throughout this assignment for architectural guidance, code examples, troubleshooting, and code review.

**AI-assisted work included:**
- Suggestions for organizing the .NET solution using Domain, Application, Infrastructure, and API projects.
- Example code for EF Core configuration, repository methods, DTOs, controller actions, JWT authentication, and Angular components.
- Guidance on validation, HTTP response codes, and testing the API.
- Reviewing implementations and suggesting corrections when issues were identified.

**My own work included:**
- Setting up the development environment, PostgreSQL Docker container, .NET solution, and Angular application.
- Implementing and integrating repository, service, and controller logic, using and adapting AI examples where appropriate.
- Implementing track filtering, distribution handling, status updates, and frontend interactions.
- Running API requests, inspecting PostgreSQL records, building both applications, and debugging failures.
- Reviewing AI suggestions rather than accepting them without testing.

Some implementation code was adapted directly from AI-provided examples. I do not claim that every line was written without assistance. My focus was on understanding, integrating, testing, and correcting the resulting application.

## 2. What security issues were identified in AI-assisted code, and how were they addressed?

**Database credentials and secrets**

Database credentials and connection details should not be committed to source control. Local environment files were excluded from Git, example configuration was provided separately, and ASP.NET Core User Secrets were used for sensitive development configuration.

**Unprotected administrative operation**

The track distribution endpoint initially had no authentication requirement. JWT bearer authentication was added, and `POST /api/tracks/{id}/distribute` was protected with `[Authorize]`.

Testing confirmed:
- No token: `401 Unauthorized`
- Invalid token: `401 Unauthorized`
- Valid development JWT: `204 No Content`

The application uses .NET development JWT tooling for demonstration. It does not implement a production identity provider or user login system.

**Database error exposure**

Submitting an existing ISRC initially caused an unhandled PostgreSQL unique-constraint exception and an HTTP `500` response containing a development stack trace.

The repository was updated to recognize the specific ISRC unique-constraint violation and return a meaningful application error. The controller translates that error to `409 Conflict`. This behavior was verified through an API test.

**Input validation**

Validation was added for invalid artist references, track statuses, and ISRC formats. Invalid requests now receive meaningful client-error responses rather than proceeding unchecked to persistence.

## 3. Describe an actual AI error and how you corrected it.

An AI suggestion initially treated EF Core's `HasMaxLength(12)` configuration as sufficient ISRC validation.

After reviewing the implementation, I identified that `HasMaxLength(12)` only limits the maximum length of a database field. It does not validate the complete ISRC structure.

I added a regular-expression validation rule to the track creation request:

```csharp
[RegularExpression(
    @"^[A-Z]{2}[A-Z0-9]{3}\d{7}$",
    ErrorMessage = "Invalid ISRC format.")]
```

This validates the expected 12-character structure rather than relying only on the database column length.

This experience reinforced the importance of checking AI-generated suggestions against actual framework behavior and verifying the implementation through tests.

## Additional implementation decisions

- **Clean Architecture:** Separate projects were used to keep domain entities, application logic, persistence, and API concerns organized.
- **PostgreSQL and EF Core:** PostgreSQL runs in Docker, while EF Core handles database access and migrations.
- **Angular frontend:** The frontend focuses on the two required views: Track List with status filtering and Track Detail with DSP distribution statuses.
- **JWT scope:** Only the distribution operation is protected because the assignment requires JWT authentication on at least one endpoint, not a complete account-management system.
- **Database constraints:** A unique ISRC index and a unique track/DSP combination protect data integrity independently of application validation.