# 🚀 Cassini MCP Server Project Plan

## 📖 Project Overview

Development of a Model Context Protocol (MCP) server that provides access to Cassini mission data stored in a SQLite database (`Data/master_plan.db`). The server will be built as a .NET 9 Web API and will expose mission planning data through MCP-compliant endpoints.

## 📊 Database Schema

The SQLite database contains mission planning data with the following columns (from `master_plan.csv`):
- `start_time_utc`: Start time in UTC format (YYYY-DDDTHH:MM:SS)
- `duration`: Duration in format DDDTHH:MM:SS
- `date`: Date in format DD-MMM-YY
- `team`: Team/instrument identifier (CAPS, CDA, MAG, ISS, MIMI, etc.)
- `spass_type`: SPASS classification type (Non-SPASS, Prime, SPASS Rider)
- `target`: Observation target (Saturn, rings, moons, etc.)
- `request_name`: Request type identifier (SURVEY, DIFSATSRC, TEMPSIT, etc.)
- `library_definition`: Library definition/category
- `title`: Observation title
- `description`: Detailed description

**Total Records**: ~61,874 observation entries

## 🏗️ Technology Stack

- **Framework**: .NET 9 Web API
- **Database**: SQLite3 (existing `Data/master_plan.db`)
- **ORM**: Entity Framework Core 9.x
- **Protocol**: Model Context Protocol (MCP)
- **Language**: C# 13

## 🔄 Development Iterations

### Iteration 1: Database Analysis & EF Core Setup
**Goal**: Establish database connection and generate EF Core models

**Tasks**:
1. Verify `Data/master_plan.db` exists and analyze schema structure
2. Install EF Core SQLite packages (`Microsoft.EntityFrameworkCore.Sqlite`)
3. Create `DbContext` configuration with SQLite connection string
4. Scaffold EF Core models from existing database into `/EF` directory
5. Create initial repository pattern for data access
6. Write basic query tests to verify database connectivity

**Deliverables**:
- `CassiniDbContext` class with SQLite configuration
- Generated EF models in `/EF/MasterPlanEntry.cs`
- Repository pattern implementation in `/Services/MasterPlanRepository.cs`
- Connection string configuration in `appsettings.json`
- Verification tests confirming database access

**Git Commits**:
- `feat: add EF Core SQLite packages`
- `feat: create DbContext for master_plan database`
- `feat: scaffold EF models from database`
- `feat: implement repository pattern for data access`
- `test: verify database connectivity`

---

### Iteration 2: MCP Protocol Implementation
**Goal**: Implement MCP server infrastructure and protocol handling

**Tasks**:
1. Research MCP protocol specification (JSON-RPC 2.0 based)
2. Create MCP request/response DTOs (`/DTOs/Mcp/`)
3. Implement MCP message handling middleware
4. Create MCP tools registration and discovery
5. Implement MCP protocol validation and error handling
6. Add logging for MCP message lifecycle

**Deliverables**:
- MCP protocol documentation in `/docs/mcp-protocol.md`
- MCP DTOs: `McpRequest`, `McpResponse`, `McpError`, `McpTool`
- `McpMiddleware` for request/response handling
- `McpToolRegistry` for tool registration
- Error handling with MCP-compliant error codes
- Logging configuration for debugging

**Git Commits**:
- `docs: add MCP protocol specification`
- `feat: create MCP request/response DTOs`
- `feat: implement MCP middleware`
- `feat: add MCP tool registry`
- `feat: implement MCP error handling`

---

### Iteration 3: Core Data Access MCP Tools
**Goal**: Create MCP tools for querying mission data

**Tasks**:
1. Implement `query_observations_by_timerange` tool
2. Implement `query_observations_by_team` tool
3. Implement `query_observations_by_target` tool
4. Implement `get_observation_details` tool
5. Add pagination support for large result sets
6. Create tool documentation with examples

**Deliverables**:
- 4 functional MCP tools in `/Tools/` directory
- Query filtering capabilities with parameters
- Pagination implementation (page size, offset)
- Tool schemas with parameter validation
- API documentation in `/docs/api/tools.md`
- Example requests/responses

**Git Commits**:
- `feat: add query_observations_by_timerange tool`
- `feat: add query_observations_by_team tool`
- `feat: add query_observations_by_target tool`
- `feat: add get_observation_details tool`
- `feat: implement pagination for tools`
- `docs: add tool documentation with examples`

---

### Iteration 4: Advanced Features & Optimization
**Goal**: Add advanced querying capabilities and performance optimizations

**Tasks**:
1. Implement multi-parameter complex filtering tool
2. Add full-text search across title and description fields
3. Create aggregation tools (count by team, target, date range)
4. Optimize database queries with proper indexing
5. Implement response caching for frequently accessed data
6. Add query performance monitoring and logging

**Deliverables**:
- `query_observations_advanced` tool with multi-filters
- `search_observations_fulltext` tool
- Aggregation tools: `get_statistics`, `get_team_summary`
- Database indexes on commonly queried columns
- Response caching layer with configurable TTL
- Performance metrics logging

**Git Commits**:
- `feat: add advanced multi-parameter query tool`
- `feat: implement full-text search`
- `feat: add aggregation and statistics tools`
- `perf: add database indexes`
- `perf: implement response caching`
- `feat: add query performance monitoring`

---

### Iteration 5: Testing, Documentation & Deployment Prep
**Goal**: Finalize testing, documentation, and prepare for production deployment

**Tasks**:
1. Create unit tests for all MCP tools (>80% coverage)
2. Implement integration tests with test database
3. Complete `README.md` with setup and usage instructions
4. Document all MCP tools with parameter specs and examples
5. Create Docker configuration for containerization
6. Add health check endpoint for monitoring
7. Final code review and cleanup

**Deliverables**:
- Comprehensive test suite in `/tests/` directory
  - Unit tests for all tools
  - Integration tests for database operations
  - MCP protocol validation tests
- Complete documentation:
  - `README.md` with setup instructions
  - `/docs/api/` with all tool specifications
  - `/docs/deployment.md` with deployment guide
- `Dockerfile` and `docker-compose.yml`
- Health check endpoint at `/health`
- Production-ready configuration
- Code coverage report

**Git Commits**:
- `test: add unit tests for all MCP tools`
- `test: add integration tests`
- `docs: complete README with setup instructions`
- `docs: document all MCP tools`
- `feat: add Docker configuration`
- `feat: add health check endpoint`
- `chore: final cleanup and review`

---

## 📁 Project Structure

```
cassini/
├── EF/                          # Generated EF Core models
│   └── MasterPlanEntry.cs       # Database entity
├── Data/
│   ├── master_plan.db           # SQLite database
│   ├── master_plan.csv          # Source data
│   └── csv_to_sqlite.sh         # Import script
├── Controllers/                 # API controllers
│   └── McpController.cs         # MCP endpoint
├── Services/                    # Business logic layer
│   ├── MasterPlanRepository.cs  # Data access
│   └── CassiniDbContext.cs      # EF DbContext
├── Tools/                       # MCP tool implementations
│   ├── QueryByTimeRangeTool.cs
│   ├── QueryByTeamTool.cs
│   ├── QueryByTargetTool.cs
│   └── GetObservationDetailsTool.cs
├── Middleware/                  # MCP protocol handling
│   └── McpMiddleware.cs
├── DTOs/                        # Data transfer objects
│   ├── Mcp/                     # MCP protocol DTOs
│   └── Observations/            # Domain DTOs
├── docs/
│   ├── logs/                    # Iteration logs
│   ├── api/                     # API documentation
│   ├── mcp-protocol.md          # MCP spec
│   └── deployment.md            # Deployment guide
├── tests/
│   ├── Unit/                    # Unit tests
│   └── Integration/             # Integration tests
├── Dockerfile
├── docker-compose.yml
├── appsettings.json
├── Program.cs
└── PROJECT.md                   # This file
```

## 🔐 Development Guidelines

### Version Control
- **Single Change = Single Commit**: Each meaningful change is one commit
- **Iteration = Single Push**: Each iteration pushed to GitHub `dev` branch
- **Descriptive Messages**: Use conventional commit format (feat, fix, docs, test, etc.)

### Code Standards
- **Comments**: Add XML documentation to all public classes and methods
- **No Assumptions**: Use only facts from database schema
- **Direct Communication**: No embellishment in documentation
- **Emoji in Docs**: Use descriptive emoji in markdown files

### Model Usage
- **DO NOT** use models in `/Models` directory (reference only)
- **ALWAYS** generate EF models in `/EF` directory
- **Reference** `/docs/master_plan_schema.md` for schema information

## 📈 Success Criteria

- ✅ Successful connection to SQLite database with EF Core
- ✅ MCP protocol compliance (JSON-RPC 2.0)
- ✅ All core query tools functional and tested
- ✅ Comprehensive test coverage (>80%)
- ✅ Complete documentation for setup and usage
- ✅ Production-ready deployment configuration
- ✅ Performance optimizations with caching and indexing
- ✅ Docker containerization for easy deployment

## 🎯 Definition of Done (per Iteration)

An iteration is considered complete when:
1. All tasks are implemented and committed
2. Code is reviewed and cleaned up
3. Documentation is updated
4. Changes are pushed to `dev` branch
5. Iteration log is created in `docs/logs/2025-10-21-[iteration].md`
6. User explicitly confirms iteration closure

## 📝 Next Steps

To begin **Iteration 1**, run:
```powershell
cd c:\Repos\mcp\cassini
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Then verify the database exists:
```powershell
ls Data/master_plan.db
```
