param(
    [Parameter(Mandatory = $true)]
    [string] $CustomerId,

    [string] $LicenseId = "",

    [string] $LicenseKey = "",

    [int] $Months = 1,

    [int] $MaxDevices = 1,

    [int] $MaxProjects = 999,

    [bool] $Updates = $true,

    [bool] $ExportPdf = $true,

    [bool] $AdvancedReports = $true,

    [bool] $Sync = $true,

    [bool] $ThreeDPreview = $true,

    [string] $StorePath = ""
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($StorePath)) {
    $StorePath = Join-Path $PSScriptRoot "..\Data\licenses.json"
}

$StorePath = [System.IO.Path]::GetFullPath($StorePath)

if ([string]::IsNullOrWhiteSpace($LicenseId)) {
    $LicenseId = "lic_" + [Guid]::NewGuid().ToString("N")
}

if ([string]::IsNullOrWhiteSpace($LicenseKey)) {
    $randomBytes = New-Object byte[] 24
    $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    try {
        $rng.GetBytes($randomBytes)
    } finally {
        $rng.Dispose()
    }
    $raw = [Convert]::ToBase64String($randomBytes).TrimEnd("=").Replace("+", "").Replace("/", "")
    $LicenseKey = ($raw.Substring(0, 4), $raw.Substring(4, 4), $raw.Substring(8, 4), $raw.Substring(12, 4), $raw.Substring(16, 4)) -join "-"
}

$paidUntilUtc = [DateTimeOffset]::UtcNow.AddMonths($Months)

if (Test-Path -LiteralPath $StorePath) {
    $document = Get-Content -LiteralPath $StorePath -Raw | ConvertFrom-Json
} else {
    $directory = Split-Path -Path $StorePath -Parent
    New-Item -ItemType Directory -Path $directory -Force | Out-Null
    $document = [pscustomobject]@{ licenses = @() }
}

$existing = @($document.licenses | Where-Object { $_.licenseKey -eq $LicenseKey -or $_.licenseId -eq $LicenseId })
if ($existing.Count -gt 0) {
    throw "License with the same key or id already exists."
}

$license = [pscustomobject]@{
    licenseKey = $LicenseKey
    licenseId = $LicenseId
    customerId = $CustomerId
    status = "active"
    paidUntilUtc = $paidUntilUtc.ToString("O")
    maxDevices = $MaxDevices
    activatedMachineIds = @()
    features = [pscustomobject]@{
        updates = $Updates
        exportPdf = $ExportPdf
        advancedReports = $AdvancedReports
        sync = $Sync
        threeDPreview = $ThreeDPreview
        maxProjects = $MaxProjects
    }
}

$licenses = @($document.licenses)
$document.licenses = @($licenses + $license)
$document | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $StorePath -Encoding utf8

Write-Host "License created"
Write-Host "Store: $StorePath"
Write-Host "LicenseKey: $LicenseKey"
Write-Host "LicenseId: $LicenseId"
Write-Host "CustomerId: $CustomerId"
Write-Host "PaidUntilUtc: $($paidUntilUtc.ToString("O"))"
