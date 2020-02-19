dotnet test /p:CollectCoverage=true `
/p:CoverletOutputFormat=opencover `
/p:CoverletOutput=.\results\coverage\ `
/p:Include="[Bookstore*]*"

& "../../../tools/report-generator/ReportGenerator.exe" `
-reports:"./results/coverage/coverage.opencover.xml" `
-targetdir:"./results/coverage/Reports" `
-reportTypes:htmlInline

Start-Process "./results/coverage/reports/index.htm"