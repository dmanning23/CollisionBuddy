rm *.nupkg
nuget pack .\CollisionBuddy.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget pack .\CollisionBuddy.Bridge.nuspec -IncludeReferencedProjects -Prop Configuration=Release
cp *.nupkg C:\Projects\Nugets\
nuget push *.nupkg -Source https://www.nuget.org/api/v2/package