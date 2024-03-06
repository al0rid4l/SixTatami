#!/usr/bin/env bash

# 默认参数值
rid='none'
exe=
arch=
project=$(pwd)
release='Debug'
cmd='dotnet publish '
args=''

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
	args=$args"-p:Arch=$arch "
fi

if [[ $exe ]]; then
	args=$args"-p:ExeFile=$exe "
fi

if [[ $rid ]] && [[ $rid != 'none' ]]; then
	args=$args"-p:RID=$rid "
fi

args=$args" -c $release "

echo "$release Build $project"
echo "RID: $rid"

if [[ $project =~ csproj$ ]]; then
	$cmd$project$args
else
	$cmd$args
fi