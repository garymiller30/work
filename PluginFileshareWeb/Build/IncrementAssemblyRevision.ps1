param(
    [Parameter(Mandatory = $true)]
    [string]$AssemblyInfoPath
)

$encoding = New-Object System.Text.UTF8Encoding($true)
$content = [System.IO.File]::ReadAllText($AssemblyInfoPath, $encoding)
$versionPattern = '\[assembly:\s*AssemblyVersion\("(?<major>\d+)\.(?<minor>\d+)\.(?<build>\d+)\.(?<revision>\d+)"\)\]'
$match = [regex]::Match($content, $versionPattern)

if (-not $match.Success) {
    throw "AssemblyVersion was not found in $AssemblyInfoPath"
}

$major = $match.Groups['major'].Value
$minor = $match.Groups['minor'].Value
$build = $match.Groups['build'].Value
$revision = [int]$match.Groups['revision'].Value + 1
$nextVersion = "$major.$minor.$build.$revision"

$content = [regex]::Replace(
    $content,
    '\[assembly:\s*AssemblyVersion\("\d+\.\d+\.\d+\.\d+"\)\]',
    "[assembly: AssemblyVersion(`"$nextVersion`")]",
    1)

$content = [regex]::Replace(
    $content,
    '\[assembly:\s*AssemblyFileVersion\("\d+\.\d+\.\d+\.\d+"\)\]',
    "[assembly: AssemblyFileVersion(`"$nextVersion`")]",
    1)

[System.IO.File]::WriteAllText($AssemblyInfoPath, $content, $encoding)
Write-Host "PluginFileshareWeb version bumped to $nextVersion"
