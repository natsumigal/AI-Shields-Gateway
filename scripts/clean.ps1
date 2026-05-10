$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$targets = @(
    "bin",
    "obj",
    "TestResults",
    "coverage"
)

Get-ChildItem -Path $root -Directory -Recurse -Force |
    Where-Object { $targets -contains $_.Name } |
    ForEach-Object {
        Write-Host "Removing $($_.FullName)"
        Remove-Item -LiteralPath $_.FullName -Recurse -Force
    }

Get-ChildItem -Path $root -File -Force |
    Where-Object { $_.Name -in @("api-server-current.log", "api-server-current.err.log") } |
    ForEach-Object {
        Write-Host "Removing $($_.FullName)"
        Remove-Item -LiteralPath $_.FullName -Force
    }
