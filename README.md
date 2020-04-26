aka BitmapCSV - Tool for reading bitmap and converting it into a school project compatible csv (.xyz) format

![BiCeSeVe cover picture](doc/cover.jpg)
[dennybusyet](https://www.123rf.com/profile_dennybusyet) © 123RF.com

## Wow! How do I execute it?

### Requirements
- dotnet core 3.1 
    - (tested with)
### Execute commands
- dotnet build
- dotnet run

### Find the executable in the bin folder

.\Biceseve.ConsoleApp.exe [argument] [path to either .bmp or .xyz]

#### Example:
.\Biceseve.ConsoleApp.exe -wzc C:\file.bmp

## Awesome, what are the arguments?

Four different functions:

1. **-w** or **--write**
    - convert from bmp to xyz
2. **-r** or **--read**
    - convert from xyz to bmp
3. **-wzc** or **--writeZeroCentered**
    - convert from zero-centered bmp to xyz
4. **-rzc** or **--readZeroCentered**
    - convert from zero-centered xyz to bmp
 
