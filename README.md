# CPK Extractor

CPK Extractor is a simple wrapper program around CriFsV2Lib for simple extracting of individual files and directories.

## Usage

```bash
CpkExtract.exe <file-path> [output-directory]
```

### Parameters

- `<file-path>`: The path to the directory or file to search for .cpk files.
- `[output-directory]`: Optional. The directory where extracted files will be stored. Defaults to the same directory as the .cpk file.

## Usage

To extract all .cpk files in a directory to adjacent directories as output:

```bash
CpkExtract.exe "C:\path\to\directory"
```

To extract a specific .cpk file to a custom output directory:

```bash
CpkExtract.exe "C:\path\to\file.cpk" "C:\path\to\output\directory"
```

## Error Handling

If there are any errors during extraction, they will be logged to the console with the file name and the reason for the failure.

## Development

### Building
- Install .NET 8 SDK
- Build with `dotnet build -c Release`

## License

This project is licensed under the GPL-3.0 license - see the [LICENSE](LICENSE.txt) for details.

## Credits
|Author|Project|
|:---------:|:---------:|
|[Sewer56](https://github.com/Sewer56)|[CriFsV2Lib](https://github.com/Sewer56/CriFsV2Lib)|