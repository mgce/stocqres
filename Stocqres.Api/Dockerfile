FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 44328

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Stocqres.Api/Stocqres.Api.csproj", "Stocqres.Api/"]
COPY ["../Users/MateuszGajda/source/repos/Stocqres.Transactions/Stocqres.Transactions.csproj", "../Users/MateuszGajda/source/repos/Stocqres.Transactions/"]
COPY ["Stocqres.Core/Stocqres.Core.csproj", "Stocqres.Core/"]
COPY ["Stocqres.Infrastructure/Stocqres.Infrastructure.csproj", "Stocqres.Infrastructure/"]
COPY ["Stocqres.Domain/Stocqres.Domain.csproj", "Stocqres.Domain/"]
COPY ["Stocqres.Customers/Stocqres.Customers.csproj", "Stocqres.Customers/"]
COPY ["../Users/MateuszGajda/source/repos/Stocqres.SharedKernel/Stocqres.SharedKernel.csproj", "../Users/MateuszGajda/source/repos/Stocqres.SharedKernel/"]
COPY ["Stocqres.Identity/Stocqres.Identity.csproj", "Stocqres.Identity/"]
RUN dotnet restore "Stocqres.Api/Stocqres.Api.csproj"
COPY . .
WORKDIR "/src/Stocqres.Api"
RUN dotnet build "Stocqres.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Stocqres.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Stocqres.Api.dll"]