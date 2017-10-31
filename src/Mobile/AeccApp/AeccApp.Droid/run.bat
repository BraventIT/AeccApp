SonarQube.Scanner.MSBuild.exe begin /k:"Aecc.Droid" /n:"Aecc Android" /v:"1.0"
MSBuild.exe /t:Rebuild
SonarQube.Scanner.MSBuild.exe end