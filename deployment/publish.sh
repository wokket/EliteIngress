#!/bin/bash

# get directory hostng the scipt
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Build release package of the EnlEliteBot

cd $DIR/../EliteIngressWeb
dotnet publish -c Release

echo
echo 'Adding installation script'
cp $DIR/install.sh bin/Release/netcoreapp2.0/publish/

echo
echo 'Creating tarball...'
tar -zcvf eliteIngress.tar.gz bin/Release/netcoreapp2.0/publish/*

echo
echo 'Uploading published files to temp directory'
#copy the new files up to a tmp directory, -C = use compression, -p = preserve modified timestamps etc, -r = recursive
scp -C -p -r eliteIngress.tar.gz tim@163.172.163.77:/tmp/


echo
echo 'Connecting to server to restart process...'
# connect to the server, -t = feed terminal input back for passwords etc
# make our target if it doesn't exist, extract the tar (skipping 4 blank directories), and run the installer'
ssh -t tim@163.172.163.77 'mkdir -p /tmp/eliteIngress_publish && tar -zxvf /tmp/eliteIngress.tar.gz -C /tmp/eliteIngress_publish --strip-components=4 && /tmp/eliteIngress_publish/install.sh'


