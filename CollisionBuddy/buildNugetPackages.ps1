rm *.nupkg
nuget pack .\CollisionBuddy.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget push *.nupkg