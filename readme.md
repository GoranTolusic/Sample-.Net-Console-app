#PULLANJE MS SQL SERVERA
docker pull --platform linux/arm64/v8 mcr.microsoft.com/azure-sql-edge

#RUNANJE CONTAINERA (Primjer)
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/azure-sql-edge

#INSTALACIJA ef alata
dotnet tool install --global dotnet-ef

#DODAVANJE ef alata u putanju (Primjer za Mac)
cat << \EOF >> ~/.zprofile
# Add .NET Core SDK tools
export PATH="$PATH:/Users/gorantolusic/.dotnet/tools"
EOF

#učitavanje promjene u trenutnoj sesiji (Primjer za Mac)
zsh -l

#Ovo će kreirati novu migraciju s nazivom InitialCreate koja će sadržavati SQL skripte za stvaranje tablica u bazi podataka.
dotnet ef migrations add InitialCreate

#Ovo će primijeniti migracije i kreirati tablice u vašoj bazi podataka prema modelima koje ste definirali.
dotnet ef database update


dotnet restore is for installing packages (similar like npm install, it will install packages into nuget_packages directory)
dotnet build is to build files
dotnet run is build + run