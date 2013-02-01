// include Fake lib
#r @"Tools/FAKE/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile
open System

// build properties
let buildTarget = getBuildParamOrDefault "buildTarget" "Publish"
let buildConfiguration = getBuildParamOrDefault "buildConfiguration" "Debug"

// common path properties
let sourceDir = currentDirectory @@ "src"
let publishDir = currentDirectory @@ "Publish"
let solutionFile = sourceDir @@ "License.Manager.sln"

// tools path properties
let toolsDir = currentDirectory @@ "Tools"

// common assembly info properties
let assemblyVersion = getBuildParamOrDefault "assemblyVersion" "0.0.0.0"
let assemblyFileVersion = getBuildParamOrDefault "assemblyFileVersion" "0.0.0.0"
let assemblyInformationalVersion = getBuildParamOrDefault "assemblyInformationalVersion" "0.0.0-devel"

// Targets
Target "All" DoNothing

Target "Clean" (fun _ ->
    let directoriesToClean =
        !+ publishDir |> Scan

    CleanDirs directoriesToClean
)

Target "CreateAssemblyInfo" (fun _ ->
    CreateCSharpAssemblyInfo (sourceDir @@ "CommonAssemblyInfo.cs")
        [
        Attribute.Company "Nauck IT KG"
        Attribute.Product "License Manager Light"
        Attribute.Copyright (sprintf "Copyright © 2012 - %A Nauck IT KG" DateTime.Now.Year)
        Attribute.Version assemblyVersion
        Attribute.FileVersion assemblyFileVersion
        Attribute.InformationalVersion assemblyInformationalVersion
        ]
)

Target "Build" (fun _ ->
    build (fun msbuild ->
        {msbuild with
            Targets = [buildTarget]
            Properties = 
                [
                "Configuration", buildConfiguration
                "AssemblyVersion", assemblyFileVersion
                ]
        }
    ) solutionFile
)

// Dependencies
"Clean" 
    ==> "CreateAssemblyInfo"
    ==> "Build"
    ==> "All"
 
// start build
Run <| getBuildParamOrDefault "target" "All"