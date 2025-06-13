$projects = Get-ChildItem -Recurse -Filter *.csproj
$target = "$PSScriptRoot\nupkgs"

if (!(Test-Path $target)) {
    New-Item $target -ItemType Directory | Out-Null
}

Get-ChildItem $target -Filter *.nupkg -Recurse -ErrorAction SilentlyContinue | Remove-Item -Force
Get-ChildItem $target -Filter *.snupkg -Recurse -ErrorAction SilentlyContinue | Remove-Item -Force

foreach ($proj in $projects) {
    $release = Join-Path $proj.Directory.FullName "bin\Release"
    if (Test-Path $release) {
        Get-ChildItem "$release\*.nupkg" | Copy-Item -Destination $target -Force
    }
}