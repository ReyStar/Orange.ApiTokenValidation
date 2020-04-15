FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env 
WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR "/src/Orange.ApiTokenValidation.Shell"
RUN dotnet build "Orange.ApiTokenValidation.Shell.csproj" --runtime linux-x64 -c Debug --no-restore 

#FROM build AS publish
RUN dotnet publish -c Debug --self-contained -r linux-x64 "Orange.ApiTokenValidation.Shell.csproj" -o /Public --no-restore

WORKDIR /Public
#VOLUME  /Public

ENV ASPNETCORE_ENVIRONMENT Development
EXPOSE 60000 60001

CMD /Public/Orange.ApiTokenValidation.Shell