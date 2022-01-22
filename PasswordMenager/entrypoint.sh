#!/bin/bash

echo "dziala"

wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

apt-get update; \
  apt-get install -y apt-transport-https && \
  apt-get update && \
  apt-get install -y dotnet-sdk-6.0

until dotnet ef database update; do
>&2 echo "SQL Server is starting up"
sleep 1
done

dotnet PasswordMenager.dll