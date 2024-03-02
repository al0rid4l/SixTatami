param([string]$a, [string]$e, [string]$r, [string]$c, [switch]$z = $false)

function Build([switch]$isLib = $false, [switch]$isRelease = $false, [string]$project = $PWD, [string]$rid = 'none', [string]$arch, [string]$exe) {
	$ridList = 'win-x64', 'linux', 'linux-x64', 'linux-musl-x64', 'linux-arm', 'linux-arm64', 'linux-musl-arm64', 'osx-arm64'
	$cmd = 'dotnet publish'

	if ($project -match "csproj$") {
		$cmd += " $project"
	}
	
	if ($arch) {
		$cmd += " -p:Arch=$arch"
	}

	if ($exe) {
		$cmd += " -p:ExeFile=$exe"
	}
	
	$cmd += $isRelease ? ' -c Release' : ' -c Debug'
	
	if ($isLib && $isRelease) {
		foreach ($r in $ridList) {
			echo "Release Build $project"
			echo "RID: $r"
			if ($r -eq 'linux') {
				Invoke-Expression "$cmd -p:RID=$r -p:PublishReadyToRun=false"
			} else {
				Invoke-Expression "$cmd -p:RID=$r"
			}
		}
	} elseif ($isRelease) {
		echo "Release Build $project"
		echo "RID: $rid"
		Invoke-Expression $rid == 'none' ? $cmd : "$cmd -p:RID=$rid"
	} else {
		echo "Debug Build $project"
		echo "RID: $rid"
		Invoke-Expression $cmd
	}
}


Build -project $c -isLib -isRelease:$z -rid $r -arch $a -exe $e