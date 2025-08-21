# Script to generate OpenAPI JSON
Write-Host "Starting application to generate OpenAPI JSON..."

# Start the application in background
$job = Start-Job -ScriptBlock {
    Set-Location "Calculadora"
    dotnet run --urls="http://localhost:5000"
}

# Wait for application to start
Start-Sleep -Seconds 5

try {
    # Download the OpenAPI JSON
    $response = Invoke-RestMethod -Uri "http://localhost:5000/swagger/v1/swagger.json"
    
    # Save to file
    $response | ConvertTo-Json -Depth 10 | Out-File -FilePath "openapi.json" -Encoding UTF8
    
    Write-Host "OpenAPI JSON saved to openapi.json"
    Write-Host "Content:"
    Get-Content "openapi.json"
}
catch {
    Write-Host "Error accessing OpenAPI endpoint: $_"
}
finally {
    # Stop the application
    Stop-Job -Job $job
    Remove-Job -Job $job
}