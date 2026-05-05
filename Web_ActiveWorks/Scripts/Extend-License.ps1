param(
    [Parameter(Mandatory = $true)]
    [string] $LicenseKey,

    [int] $Months = 1,

    [int] $Days = 0,

    [string] $StorePath = ""
)

$ErrorActionPreference = "Stop"

if ($Months -lt 0) {
    throw "Months cannot be negative."
}

if ($Days -lt 0) {
    throw "Days cannot be negative."
}

if ($Months -eq 0 -and $Days -eq 0) {
    throw "Specify at least one month or one day to extend."
}

if ([string]::IsNullOrWhiteSpace($StorePath)) {
    $StorePath = Join-Path $PSScriptRoot "..\Data\licenses.json"
}

$StorePath = [System.IO.Path]::GetFullPath($StorePath)

if (-not (Test-Path -LiteralPath $StorePath)) {
    throw "License store was not found: $StorePath"
}

$document = Get-Content -LiteralPath $StorePath -Raw | ConvertFrom-Json
$license = @($document.licenses | Where-Object { $_.licenseKey -eq $LicenseKey }) | Select-Object -First 1

if ($null -eq $license) {
    throw "License was not found: $LicenseKey"
}

$now = [DateTimeOffset]::UtcNow
$currentPaidUntil = [DateTimeOffset]::MinValue

if (-not [string]::IsNullOrWhiteSpace([string]$license.paidUntilUtc)) {
    $currentPaidUntil = [DateTimeOffset]::Parse([string]$license.paidUntilUtc)
}

$baseDate = $currentPaidUntil
if ($baseDate -lt $now) {
    $baseDate = $now
}

$newPaidUntil = $baseDate.AddMonths($Months).AddDays($Days)
$license.paidUntilUtc = $newPaidUntil.ToString("O")
$license.status = "active"

$document | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $StorePath -Encoding utf8

Write-Host "License extended"
Write-Host "Store: $StorePath"
Write-Host "LicenseKey: $LicenseKey"
Write-Host "OldPaidUntilUtc: $($currentPaidUntil.ToString("O"))"
Write-Host "NewPaidUntilUtc: $($newPaidUntil.ToString("O"))"
