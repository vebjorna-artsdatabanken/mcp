# Project Info
I'm creating an MCP Server for the Cassini mission data. The server will provide endpoints to access the mission data stored in a SQLite database located in the cassini/Data/master_plan.db.

## Considerations

I want to move very slowly, with one change being a single git commit, one iteration being a single push to GitHub dev branch.

## Guidelines

DO NOT use the models in the `/Models` directory, except for reference. They are not EF models. The EF models will be generated in the `/EF` directory when the time comes.

ALWAYS use descriptive emoji with markdown docs.

Don't embellish or fill in gaps, ALWAYS use facts. Don't act effusive or overly positive. Just be direct.

Always add comments and documentation to generated code. Classes and methods ONLY.

## Closing iterations

Don't close an iteration until I tell you to.

When closing, summarize the chat session with your instructions my prompts and the result. Create a new in `docs/logs` with the name `[year]-[month]-[day]-[iteration].md` with the summary.