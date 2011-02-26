'------------------------------------------------------------------------------------------------
'
' Usage: post-build <--target|-t buildpath>
'               <--mode|-m buildmode>
'               <--project|-p projectname>
'               <--lang|-l languages>
'               [--help|-?]
'------------------------------------------------------------------------------------------------

' Force explicit declaration of all variables.
Option Explicit

'On Error Resume Next

Dim oArgs, ArgNum
Dim buildPath, projectName, langArray, debugMode
Dim verbose

verbose = false
debugMode = false

Set oArgs = WScript.Arguments
ArgNum = 0
While ArgNum < oArgs.Count

	Select Case LCase(oArgs(ArgNum))
		Case "--target","-t":
			ArgNum = ArgNum + 1
			buildPath = oArgs(ArgNum)
		Case "--mode","-m":
			ArgNum = ArgNum + 1
			debugMode = (LCase(oArgs(ArgNum)) = "debug")
		Case "--project","-p":
			ArgNum = ArgNum + 1
			projectName = oArgs(ArgNum)
		Case "--lang","-l":
			ArgNum = ArgNum + 1
			langArray = Split(oArgs(ArgNum), ",", -1)
		Case "--help","-?":
			Call DisplayUsage
		Case Else:
			Call DisplayUsage
	End Select	

	ArgNum = ArgNum + 1
Wend

If (buildPath = "") Then
	Call DisplayUsage
End If

Call PrepareLangFolder
'Call PrepareConfig
Call CleanUp

Sub PrepareLangFolder
	Dim fso, langPath, i
	Set fso = CreateObject("Scripting.FileSystemObject")
    langPath = fso.BuildPath(buildPath, "Languages")
    If fso.FolderExists(langPath) Then
        Call fso.DeleteFolder(langPath, true)
    End If
	'If Not debugMode Then
        If IsArray(langArray) Then
            fso.CreateFolder(langPath)
	        for i = 0 to UBound(langArray)
	            Call fso.MoveFolder(fso.BuildPath(buildPath, langArray(i)), fso.BuildPath(langPath, langArray(i)))
	        next
        End If
	'End If
    Set fso = Nothing
End Sub

Sub PrepareConfig
	Dim fso, xmlDoc, xnDicDir, xnRecentFiles, configPath
	If Not debugMode Then
	    Set fso = CreateObject("Scripting.FileSystemObject")
        configPath = fso.BuildPath(buildPath, projectName & ".exe.config")
        Set xmlDoc = CreateObject("Microsoft.XMLDOM")
        xmlDoc.async = False
        xmlDoc.Load configPath
        Set xnDicDir = xmlDoc.selectSingleNode("//setting[@name='DicDir']/value")
        xnDicDir.Text = "[[DicDir]]"
        Set xnRecentFiles = xmlDoc.selectSingleNode("//setting[@name='RecentFiles']/value")
        xnRecentFiles.Text = "[[RecentFiles]]"
        xmlDoc.Save configPath
        Set fso = Nothing
	End If
End Sub

Sub CleanUp
	Dim fso
	If Not debugMode Then
	    Set fso = CreateObject("Scripting.FileSystemObject")
	    fso.DeleteFile(fso.BuildPath(buildPath, projectName & "*.pdb"))
        Set fso = Nothing
	End If
End Sub

Sub DisplayUsage
	WScript.Echo "Usage: post-build <--target|-t buildpath>"
	WScript.Echo "               <--mode|-m buildmode>"
	WScript.Echo "               <--project|-p projectname>"
	WScript.Echo "               <--lang|-l languages>"
 	WScript.Echo "               [--help|-?]"
	WScript.Quit (1)
End Sub

Sub Display(Msg)
	WScript.Echo Now & ". Error Code: " & Hex(Err) & " - " & Msg
End Sub

Sub Trace(Msg)
	if verbose = true then
		WScript.Echo Now & " : " & Msg	
	end if
End Sub
