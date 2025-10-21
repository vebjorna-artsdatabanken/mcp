# Test database connectivity through MCP endpoint
Start-Sleep -Seconds 3

$body = @'
{
  "jsonrpc": "2.0",
  "method": "tools/call",
  "params": {
    "name": "query_observations_by_team",
    "arguments": {
      "team": "ISS",
      "limit": 2
    }
  },
  "id": 1
}
'@

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5054/mcp" -Method POST -Body $body -ContentType "application/json"
    Write-Host "✅ Database connection successful!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Response:" -ForegroundColor Cyan
    $response.Content | ConvertFrom-Json | ConvertTo-Json -Depth 10
} catch {
    Write-Host "❌ Database connection failed!" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
