param([string]$a, [string]$e, [string]$r, [string]$c)

if ($a -and $e -and $r) {
	echo 'Release Build'
	dotnet publish $c -c Release -p:Arch=$a -p:RID=$r -p:ExeFile=$e
} else {
	echo 'Debug Build'
	dotnet publish -c Debug
}