#!/usr/bin/env bash

# 默认参数值
rid='none'
exe=
arch=
project=
release='Debug'
cmd='dotnet publish'
args=''

# project isLib
Build() {
	local ridList=(win-x64 linux linux-x64 linux-musl-x64 linux-arm linux-arm64 linux-musl-arm64 osx-arm64)
	local finalCmd=''
	local finalArgs=''
	
	if [[ $1 =~ csproj$ ]]; then
		finalCmd=$cmd" $1"
	else
		finalCmd=$cmd
	fi
	
	if [[ $2 && $release == 'Release' ]]; then
		for r in ${ridList[*]}; do
			if [[ $r == 'linux' ]]; then
				finalArgs=$args" -p:RID=$r -p:PublishReadyToRun=false"
			else
				finalArgs=$args" -p:RID=$r"
			fi

			echo "$release Build $1"
			echo "RID: $r"
			$finalCmd$finalArgs
		done
	else
		finalArgs=$args" -p:RID=$rid"

		echo "$release Build $1"
		echo "RID: $rid"
		$finalCmd$finalArgs
	fi
}

# 解析命令行选项
while getopts ":r:e:a:c:z:" opt; do
	case $opt in
		r) rid=$OPTARG ;;
		e) exe=$OPTARG ;;
		a) arch=$OPTARG ;;
		c) project=$OPTARG ;;
		z) release='Release' ;;
		\?)
			echo "无效的选项: -$OPTARG" >&2
			exit 1 ;;
	esac
done

if [[ $arch ]]; then
	args=$args" -p:Arch=$arch"
fi

if [[ $exe ]]; then
	args=$args" -p:ExeFile=$exe"
fi

args=$args" -c $release"

if [[ $project ]]; then
	Build $project 1
else
	Build $(pwd) 1
fi