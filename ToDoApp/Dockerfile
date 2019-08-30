FROM microsoft/dotnet:sdk-2.2 as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY ToDoApp.csproj /build/

RUN dotnet restore ./build/ToDoApp.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./ToDoApp.csproj -c ${BUILDCONFIG} -o out /p:Version=${VERSION}

FROM microsft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "ToDoApp.dll"]