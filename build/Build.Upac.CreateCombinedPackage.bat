"..\src\lib\NAnt\bin\NAnt" -buildfile:"..\src\Upac.Foundation\Upac.Foundation.build" CreateBasePackage
pause
"..\src\lib\NAnt\bin\NAnt" -buildfile:"..\src\Upac.Foundation\Upac.Foundation.build" CreateNewsModulePackage
pause
"..\src\lib\NAnt\bin\NAnt" -buildfile:"..\src\Upac.Foundation\Upac.Foundation.build" CreatePublicationModulePackage
pause
"..\src\lib\NAnt\bin\NAnt" -buildfile:"..\src\Upac.Foundation\Upac.Foundation.build" CreateEventModulePackage
pause
"..\src\lib\NAnt\bin\NAnt" -buildfile:"..\src\Upac.Foundation\Upac.Combined.build" CreateCombinedPackage
pause