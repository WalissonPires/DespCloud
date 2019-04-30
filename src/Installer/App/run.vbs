Dim WShell
Set WShell = CreateObject("WScript.Shell")

originalDir = WShell.CurrentDirectory
strApp = originalDir & "\bin\webapi\webapi.dll"
arrPath = Split(strApp, "\")
For i = 0 to Ubound(arrPath) - 1
    strAppPath = strAppPath & arrPath(i) & "\"
Next 
WShell.CurrentDirectory = strAppPath
WShell.Run "dotnet.exe " & strApp, 2



strApp = originalDir & "\bin\website\website.dll"
arrPath = Split(strApp, "\")
strAppPath = ""
For i = 0 to Ubound(arrPath) - 1
    strAppPath = strAppPath & arrPath(i) & "\"
Next 
WShell.CurrentDirectory = strAppPath
WShell.Run "dotnet.exe " & strApp, 2



laucherBrowser = true
if (WScript.Arguments.Count = 1) then
	if (WScript.Arguments(0) = "/s") then
		laucherBrowser = false
	end if
end if

if laucherBrowser then
	WScript.Sleep 5000
	WShell.Run "http://localhost:5005", 9
end if

Set WShell = Nothing
