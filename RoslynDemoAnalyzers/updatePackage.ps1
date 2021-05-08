rm .\RoslynDemoAnalyzers.Package\bin\Debug\*.nupkg
[xml]$project = Get-Content .\RoslynDemoAnalyzers.Package\RoslynDemoAnalyzers.Package.csproj

$version = $project.Project.PropertyGroup[1].PackageVersion
$parts = $version.Split(".")
$minorVersion = [int]$parts[2] + 1
$newVersion = "$($parts[0]).$($parts[1]).$minorVersion"
write-host "incrementing project to $newVersion"
$project.Project.PropertyGroup[1].PackageVersion = "$newVersion.0"

$project.save(".\RoslynDemoAnalyzers.Package\RoslynDemoAnalyzers.Package.csproj")

dotnet build .\RoslynDemoAnalyzers.Package\RoslynDemoAnalyzers.Package.csproj
nuget.exe add .\RoslynDemoAnalyzers.Package\bin\Debug\RoslynDemoAnalyzers.$newVersion.nupkg -source C:\local-nuget-feed