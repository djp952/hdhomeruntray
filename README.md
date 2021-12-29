# HDHomeRun Status Monitor  

![logo](assets/appmarkdown.png)   
A Windows system tray application that monitors the status of all local HDHomeRun devices in the system.   
   
## [__USER DOCUMENTATION AND DOWNLOADS__](https://github.com/djp952/hdhomeruntray/wiki)   
   
Copyright (C)2021 Michael G. Brehm    
[MIT LICENSE](https://opensource.org/licenses/MIT)   
   
[__LIBHDHOMERUN__](https://github.com/Silicondust/libhdhomerun) - Copyright (C)2005-2018 Silicondust USA Inc   
[__JSON.NET__](https://www.newtonsoft.com/json) - Copyright (c) 2007 James Newton-King   
   
## BUILD ENVIRONMENT
**REQUIRED COMPONENTS**   
* Visual Studio 2022 (__.NET Framework 4.6.2 Targeting Pack__, __Windows 10 SDK (any version)__, __C++/CLI tools__, and __C++ 2022 Redistributable MSMs__ required)   
* Wix Toolset Build Tools v3.11.2 (https://wixtoolset.org/releases/)   
* Wix Toolset Visual Studio 2022 Extension (https://wixtoolset.org/releases/)   
   
## BUILD
**INITIALIZE SOURCE TREE AND DEPENDENCIES**
* Open "Developer Command Prompt for VS2022"   
```
git clone https://github.com/djp952/hdhomeruntray
cd hdhomeruntray
git submodule update --init
```
   
**BUILD TARGET PACKAGE(S)**   
* Open "Developer Command Prompt for VS2022"   
```
cd hdhomeruntray
msbuild msbuild.proj
```
   
Output .MSI installer package(s) will be generated in the __out__ folder.   
   
## ADDITIONAL LICENSE INFORMATION
   
**LIBHDHOMERUN**   
[https://www.gnu.org/licenses/gpl-faq.html#LGPLStaticVsDynamic](https://www.gnu.org/licenses/gpl-faq.html#LGPLStaticVsDynamic)   
This library statically links with code licensed under the GNU Lesser Public License, v2.1 [(LGPL 2.1)](https://www.gnu.org/licenses/old-licenses/lgpl-2.1.en.html).  As per the terms of that license, the maintainer (djp952) must provide the library in an object (or source) format and allow the user to modify and relink against a different version(s) of the LGPL 2.1 libraries.  To use a different or custom version of libhdhomerun the user may alter the contents of the depends/libhdhomerun source directory prior to building this library.   
