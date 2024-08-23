# PULLANJE MS SQL SERVERA (macOs)
docker pull --platform linux/arm64/v8 mcr.microsoft.com/azure-sql-edge

# PULLANJE MS SQL SERVERA (LINUX)
docker pull --platform linux/amd64/v3 mcr.microsoft.com/azure-sql-edge

# RUNANJE CONTAINERA (Primjer)
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/azure-sql-edge

# INSTALACIJA ef alata
dotnet tool install --global dotnet-ef

# DODAVANJE ef alata u putanju (Primjer za Mac)
cat << \EOF >> ~/.zprofile
# Add .NET Core SDK tools
export PATH="$PATH:/Users/gorantolusic/.dotnet/tools"
EOF

# učitavanje promjene u trenutnoj sesiji (Primjer za Mac)
zsh -l

# Ovo će kreirati novu migraciju s nazivom InitialCreate koja će sadržavati SQL skripte za stvaranje tablica u bazi podataka.
dotnet ef migrations add InitialCreate

# Ovo će primijeniti migracije i kreirati tablice u vašoj bazi podataka prema modelima koje ste definirali.
dotnet ef database update


dotnet restore is for installing packages (similar like npm install, it will install packages into nuget_packages directory)
dotnet build is to build files
dotnet run is build + run


# Pullanje elastic searcha
docker pull elasticsearch:8.10.2


# Runanje Elastic containera (Primjer)

# sa https
docker run -d --name elasticsearch-container -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" elasticsearch:8.10.2

# sa http
docker run -d --name elasticsearch-container -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e "xpack.security.enabled=false" elasticsearch:8.10.2

# Kibana tool za elastic 
docker run -d --name kibana-container --link elasticsearch-container:elasticsearch -p 5601:5601 kibana:8.10.2



