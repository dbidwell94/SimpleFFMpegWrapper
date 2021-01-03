[ -d "./Tests/Resources" ] && cp -r ./Tests/Resources/ bin/Debug/netcoreapp3.1/ && echo "Resource folder copied"
dotnet watch test -v m