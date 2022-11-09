rmdir /s /q build

dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:PublishReadyToRun=true /p:PublishTrimmed=false -o build/win