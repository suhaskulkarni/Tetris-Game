*** Build Instructions***

1) Download the project zip from the attachment and extract it(7zip can be used to extract).
 	For Example: Download it to the folder C:\Users\Username\Documents\TetrisGame and extract it here.
 
2) If you do not have the latest .Net framework, download and install the latest - For Example - .NET Framework 4.5.

3) Open the command prompt and change into the installation directory of the .NET Framework.
	For example: cd \Windows\Microsoft.NET\Framework\v4*

4) Use MSBuild.exe to compile your solution.
	For example: msbuild "C:\Users\Username\Documents\TetrisGame\TetrisGame.sln" /t:Rebuild /p:Configuration=Release /p:Platform="Any CPU"

5) Navigate into the "C:\Users\Username\Documents\TetrisGame\TetrisGame\bin\Release" folder. You can find the "TetrisGame.exe". Run it!

Thank you!