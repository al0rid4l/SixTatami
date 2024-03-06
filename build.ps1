param([string]$c = $PWD, [string]$a, [string]$e, [string]$r = 'none', [switch]$z = $false)

$cmd = 'dotnet publish'
$argss = ''
$releaseMode = 'Debug'

# if ($c -match "csproj$") {
# 	$argss += " $c"
# }

if ($a) {
	$argss += " -p:Arch=$a"
}

if ($e) {
	$argss += " -p:ExeFile=$e"
}

if ($r -notlike 'none') {
	$argss += " -p:RID=$r"
}

if ($z) {
	$releaseMode = 'Release'
}

$argss += " -c $releaseMode"

echo "$releaseMode Build $c"
echo "RID: $r"
if ($c -match "csproj$") {
	Invoke-Expression "$cmd $c $argss"
} else {
	Invoke-Expression "$cmd $argss"
}