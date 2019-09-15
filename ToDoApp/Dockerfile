# FROM microsoft/dotnet:sdk-2.2 as build

# ARG BUILDCONFIG=RELEASE
# ARG VERSION=1.0.0

# COPY ToDoApp.csproj /build/

# RUN dotnet restore ./build/ToDoApp.csproj

# COPY . ./build/
# WORKDIR /build/
# RUN dotnet publish ./ToDoApp.csproj -c ${BUILDCONFIG} -o out /p:Version=${VERSION}

# FROM microsft/dotnet:2.2-aspnetcore-runtime
# WORKDIR /app

# COPY --from=build /build/out .

# ENTRYPOINT ["dotnet", "ToDoApp.dll"]

# FROM microsoft/aspnetcore-build:2.2 AS build-env
# WORKDIR /app
# COPY ToDoApp.csproj ./
# COPY . ./
# RUN dotnet publish -c Release -o out
# FROM microsoft/aspcore:2.2
# WORKDIR /app
# COPY --from=build-env /app/out .
# EXPOSE 80/tcp
# ENTRYPOINT [ "dotnet", "ToDoApp.dll" ]

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY . .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ToDoApp.dll