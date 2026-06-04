FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build-env
WORKDIR /src

COPY ["Appointment.web/Appointment.web.csproj", "Appointment.web/"]
COPY ["Appointment.Infrastructure/Appointment.Infrastructure.csproj", "Appointment.Infrastructure/"]
COPY ["Appointment.Domain/Appointment.Domain.csproj", "Appointment.Domain/"]
COPY ["Appointment.Application/Appointment.Application.csproj", "Appointment.Application/"]

RUN dotnet restore "Appointment.web/Appointment.web.csproj" --disable-parallel

COPY . .


WORKDIR "/src/Appointment.web"
RUN dotnet publish -c Release -o out /p:BuildInParallel=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble
WORKDIR /app

COPY --from=build-env /src/Appointment.web/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Appointment.web.dll"]