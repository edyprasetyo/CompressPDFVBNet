Imports System.Diagnostics
Imports System.IO

Module Module1

    Dim currentDirectory As String = Directory.GetCurrentDirectory()
    Dim gsProgramPath As String = Path.Combine(currentDirectory, "GhostScript\gswin64c.exe")

    Sub Main()
        Dim inputFiles As String() = Directory.GetFiles(currentDirectory + "/input", "*.pdf", SearchOption.AllDirectories)
        For Each inputFile As String In inputFiles
            Dim outputFile As String = Path.Combine(currentDirectory, "output", Path.GetFileNameWithoutExtension(inputFile) + "_compressed" + Path.GetExtension(inputFile))
            Directory.CreateDirectory(Path.GetDirectoryName(outputFile))

            CompressPdf(inputFile, outputFile)

            Console.WriteLine((New FileInfo(inputFile).Length / 1024).ToString + " KB")
            Console.WriteLine((New FileInfo(outputFile).Length / 1024).ToString + " KB")
            Console.WriteLine()
        Next
        Console.ReadLine()

    End Sub


    Sub CompressPdf(inputPath As String, outputPath As String)
        Dim gsCommand As String() = {
        gsProgramPath,
        "-sDEVICE=pdfwrite",
        "-dPDFSETTINGS=/ebook",
        "-dCompatibilityLevel=1.4",
        "-dNOPAUSE",
        "-dQUIET",
        "-dBATCH",
        "-dDetectDuplicateImages=true",
        "-dCompressFonts=true",
        "-dDownsampleColorImages=true",
        "-dColorImageResolution=120",
        "-dMonoImageResolution=140",
        "-sOutputFile={Path.GetFullPath(outputPath)}",
        Path.GetFullPath(inputPath)
    }
        Process.Start(New ProcessStartInfo With {
        .FileName = gsCommand(0),
        .Arguments = String.Join(" ", gsCommand, 1, gsCommand.Length - 1),
        .CreateNoWindow = True,
        .UseShellExecute = False,
        .RedirectStandardOutput = True,
        .RedirectStandardError = True
    }).WaitForExit()
    End Sub

End Module
