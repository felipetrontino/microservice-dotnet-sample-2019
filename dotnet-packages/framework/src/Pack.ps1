$Version = "1.0.0"

dotnet pack Framework.csproj /p:Configuration=Release /p:Version=$Version

Remove-Item  ..\..\..\nupkg\$Version -Recurse -Force -ErrorAction Ignore

../../../tools/nuget/nuget.exe add bin\Release\Framework.$Version.nupkg -source ..\..\..\nuget-packages
