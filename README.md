# DocumentsService

This project is an ASP.NET Core service application that provides an API for storing and retrieving documents in various formats including like JSON, XML or MessagePack. 

## Implementation Details

### API Endpoints
- Implemented using a controller-based API.
- Input validation is performed using data annotations.
  
- **POST /documents**: Creates a new document.
- **PUT /documents/{id}**: Updates an existing document.
- **GET /documents/{id}**: Retrieves a document.

### Output Formats

- **JSON, XML, MessagePack**: The service supports multiple formats.
  - Custom output formatters can be created and added to the controllers in `Program.cs`.
  - Example: `CustomXmlOutputFormatter`, `MessagePackOutputFormatter`, and `ProtobufOutputFormatter`.

### Storage Solutions

- **Flexible Storage Backend**: Different storage backends can be easily added.
  - Implemented using the `IDocumentsRepository` interface.
  - Two implementations provided:
    - **HDD Repository**: Stores documents as JSON files.
    - **InMemoryDb Repository**: Stores documents in an in-memory database.
  - To use the HDD repository:
    ```csharp
    builder.Services.AddScoped<IDocumentsRepository, HddDocumentsRepository>();
    ```
  - To use the InMemoryDb repository:
    ```csharp
    builder.Services.AddDbContext<DocumentsDbContext>(options =>
    {
        options.UseInMemoryDatabase("MyTestDb");
    });
    builder.Services.AddScoped<IDocumentsRepository, DbDocumentsRepository>();
    ```

- **Memory Cache**: Both storage implementations use `MemoryCache` to improve read performance.

### Testing

- **Unit Tests**: The application includes a project for unit tests.
  - Tests cover all endpoints and functionalities.
  - Mocking is used to simulate different storage backends.
