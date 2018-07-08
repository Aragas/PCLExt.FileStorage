if($isWindows)
{
	dotnet tool install -g coveralls.net
	choco install codecov

	(New-Object System.Net.WebClient).DownloadFile("https://ci.appveyor.com/api/buildjobs/2kynh1hlndbq1ona/artifacts/main%2Fbin%2Fpackages%2Fchocolatey%2Fopencover.portable%2Fopencover.portable.4.6.832.nupkg", "$(Get-Location)\opencover.portable.4.6.832.nupkg")
	choco upgrade opencover.portable -s $(Get-Location)
	
	choco install nunit-console-runner
	OpenCover.Console -filter:"+[PCLExt.*]*" -register:user -target:"nunit3-console.exe" -targetargs:"/domain:single test/PCLExt.FileStorage.NetFX.Test/bin/$env:CONFIGURATION/PCLExt.FileStorage.NetFX.Test.dll" -output:coverage_netfx.xml
	csmacnz.coveralls --opencover -i coverage_netfx.xml --repoToken $env:COVERALLS_REPO_TOKEN
	codecov -f coverage_netfx.xml
	
	dotnet add test/PCLExt.FileStorage.Core.Test/PCLExt.FileStorage.Core.Test.csproj package coverlet.msbuild
	dotnet test test/PCLExt.FileStorage.Core.Test/PCLExt.FileStorage.Core.Test.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Exclude=[NUnit*]*
	csmacnz.coveralls --opencover -i test/PCLExt.FileStorage.Core.Test/coverage.opencover.xml --repoToken $env:COVERALLS_REPO_TOKEN
	codecov -f test/PCLExt.FileStorage.Core.Test/coverage.opencover.xml
}
if($isLinux)
{
	wget "https://github.com/nunit/nunit-console/releases/download/3.8/NUnit.Console-3.8.0.zip"
	unzip "NUnit.Console-3.8.0.zip" -d "nunit"
	
	mono nunit/nunit3-console.exe ./test/PCLExt.FileStorage.NetFX.Test/bin/$env:CONFIGURATION/PCLExt.FileStorage.NetFX.Test.dll --result="fx-result.xml"

	dotnet vstest ./test/PCLExt.FileStorage.Core.Test/bin/$env:CONFIGURATION/netcoreapp2.0/PCLExt.FileStorage.Core.Test.dll --logger:"trx;LogFileName=../core-result.trx"	
	
	# manually upload test results
	(New-Object 'System.Net.WebClient').UploadFile("https://ci.appveyor.com/api/testresults/nunit/$($env:APPVEYOR_JOB_ID)", (Resolve-Path 'fx-result.xml'))
	(New-Object 'System.Net.WebClient').UploadFile("https://ci.appveyor.com/api/testresults/mstest/$($env:APPVEYOR_JOB_ID)", (Resolve-Path 'core-result.trx'))
}
