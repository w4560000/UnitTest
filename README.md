# UnitTest

MSTest、NUnit、xUnit 三大測試框架測試範例



## 開發時用到的指令

dotnet test ./NUnitTests/NUnitTests.csproj -l "console;verbosity=normal"

vstest.console.exe ./NUnitTests/bin/Debug/netcoreapp3.1/NUnitTests.dll /Tests:GetDatetime_測試Fakes_檢查回傳結果
dotnet test ./NUnitTests/NUnitTests.csproj --filter "GetDatetime_測試Fakes_檢查回傳結果" 


VS 擴充功能
NUnit 3 Test Adapter
NUnit Test Generator VS2022

https://marketplace.visualstudio.com/items?itemName=NUnitDevelopers.NUnit3TestAdapter


/opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h -1 -U sa -P Aa123456 -Q "SET NOCOUNT ON SELECT N'MS SQL is available 🔥' , @@servername"


## 參考文件
[MSDN - .NET Core 和 .NET Standard 的單元測試最佳做法](https://learn.microsoft.com/zh-tw/dotnet/core/testing/unit-testing-best-practices)  
[Hao Blog - C#單元測試教學](https://asbolus.medium.com/c-%E5%96%AE%E5%85%83%E6%B8%AC%E8%A9%A6%E6%95%99%E5%AD%B8-4dc7bb3370d2)
