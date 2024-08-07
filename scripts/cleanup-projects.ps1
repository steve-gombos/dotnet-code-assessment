$location = Get-location

Write-Host "Checking for projects in $location"

$projects = Get-ChildItem -Path "." -Filter "*.csproj" -Recurse -Name

Write-Host "Found $($projects.length) projects to cleanup"

dotnet jb cleanupcode $projects
