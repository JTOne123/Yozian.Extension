dotnet publish src/Yozian.Extension/Yozian.Extension.csproj \
    --force \
    -c Release \
    -o "nuget/lib/netstandard2.0"
    
rm nuget/lib/netstandard2.0/Yozian.Extension.deps.json