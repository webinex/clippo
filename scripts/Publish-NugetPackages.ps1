param (
  [Parameter(Mandatory = $true)] [string] $ApiKey
)

Import-Module -Name $PSScriptRoot\NugetPackages\Package.psm1 -Force

$ErrorActionPreference = "Stop"

function Invoke-PackAndPublishNugetPackages() {
  Publish-LibraryNugetPackage -ApiKey $ApiKey -NotPublishedOnly $true -Project "Webinex.Clippo.All" -Name "Webinex.Clippo"
}

Invoke-PackAndPublishNugetPackages;
exit 0
