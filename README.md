# Centauri OS
This repository contains most of the code for Centauri OS.
Centauri can run on two different platform types, dedicated and shared.
The dedicated platform means it is the only code running on a machine, so the full OS is installed.
The shared platform means there is another OS running and Centauri is just loaded as a single application.

## Directory Structure
| Path                                                                                                      | Description                                                                                  |
| --------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- |
| [`/Core/                   `](https://github.com/CentauriOS/centauri/tree/master/Core)                    | The code that is essential for Cenauri OS to run                                             |
| [`/Core/Fs/                `](https://github.com/CentauriOS/centauri/tree/master/Core/Fs)                 | The code that manages interactions with the file system                                      |
| [`/Core/Fs/Api/            `](https://github.com/CentauriOS/centauri/tree/master/Core/Fs/Api)             | The interfaces for the file system layer                                                     |
| [`/Core/Fs/Dedicated/      `](https://github.com/CentauriOS/centauri/tree/master/Core/Fs/Dedicated)       | The file system layer implementation for when Centauri is running on a dedicated platform    |
| [`/Core/Fs/Mock/           `](https://github.com/CentauriOS/centauri/tree/master/Core/Fs/Mock)            | The mock implementation of the file system layer                                             |
| [`/Core/Fs/Shared/         `](https://github.com/CentauriOS/centauri/tree/master/Core/Fs/Shared)          | The file system layer implementation for when Centauri is running on top of another platform |
| [`/Core/IoC/               `](https://github.com/CentauriOS/centauri/tree/master/Core/IoC)                | The inversion of control code                                                                |
| [`/Core/IoC/Api/           `](https://github.com/CentauriOS/centauri/tree/master/Core/IoC/Api)            | The interfaces and attribuets for the inversion of control code                              |
| [`/Core/IoC/Framework/     `](https://github.com/CentauriOS/centauri/tree/master/Core/IoC/Framework)      | The framework that makes inversion of control possible                                       |
| [`/Core/Platform/          `](https://github.com/CentauriOS/centauri/tree/master/Core/Platform)           | The code that is specific to a single platform                                               |
| [`/Core/Platform/Dedicated/`](https://github.com/CentauriOS/centauri/tree/master/Core/Platform/Dedicated) | The platform code for when Centauri is running on a dedicated platform                       |
| [`/Core/Platform/Shared/   `](https://github.com/CentauriOS/centauri/tree/master/Core/Platform/Shared)    | The platform code for when Centauri is running on top of another platform                    |
| [`/Core/Plugins/           `](https://github.com/CentauriOS/centauri/tree/master/Core/Plugins)            | The plugins that are required for Centauri to run                                            |
| [`/Core/Plugins/Api/       `](https://github.com/CentauriOS/centauri/tree/master/Core/Plugins/Api)        | The plugin interfaces                                                                        |
| [`/Core/Plugins/Login/     `](https://github.com/CentauriOS/centauri/tree/master/Core/Plugins/Login)      | The plugin that manages the login screen and user authentication                             |
| [`/Core/Plugins/Splash/    `](https://github.com/CentauriOS/centauri/tree/master/Core/Plugins/Splash)     | The plugin that starts while the system is booting to show the splash screen                 |
| [`/Core/Plugins/Welcome/   `](https://github.com/CentauriOS/centauri/tree/master/Core/Plugins/Welcome)    | The welcome page that is the default tab once the user has logged in                         |
| [`/Core/Rendering/         `](https://github.com/CentauriOS/centauri/tree/master/Core/Rendering)          | The code that manages rendering onto the screen                                              |
| [`/Core/Rendering/Api/     `](https://github.com/CentauriOS/centauri/tree/master/Core/Rendering/Api)      | The interfaces for the rendering layer                                                       |
| [`/Core/Rendering/DirectFB/`](https://github.com/CentauriOS/centauri/tree/master/Core/Rendering/DirectFB) | The rendering layer implementation that writes directly to the Linux framebuffer             |
| [`/Core/Rendering/Mock/    `](https://github.com/CentauriOS/centauri/tree/master/Core/Rendering/Mock)     | The mock implementation of the rendering layer                                               |
| [`/Core/Rendering/OpenGL/  `](https://github.com/CentauriOS/centauri/tree/master/Core/Rendering/OpenGL)   | The OpenGL implementation of the rendering layer                                             |
| [`/Core/Rendering/WinForms/`](https://github.com/CentauriOS/centauri/tree/master/Core/Rendering/WinForms) | The implementation of the rendering layer that runs on top of WinForms                       |
| [`/Core/Ui/                `](https://github.com/CentauriOS/centauri/tree/master/Core/Ui)                 | The core UI code for Centauri                                                                |

## File System Structure

### Dedicated Platform
| Path                                   | Description             |
| -------------------------------------- | ----------------------- |
| `/bin/Centauri.IoC.Api.dll           ` | Centauri DLL            |
| `/bin/Centauri.IoC.Framework.dll     ` | Centauri DLL            |
| `/bin/Centauri.Platform.Dedicated.dll` | Centauri executable     |
| `/boot/initrd.img                    ` | Boot filesystem         |
| `/boot/vmlinuz                       ` | Linux kernel            |
| `/etc/fstab                          ` | File system information |
| `/home/username/                     ` | User data               |
| `/home/username/.local/centauri/     ` | User plugin directory   |
| `/sbin/dotnet                        ` | CLR                     |
| `/sbin/init                          ` | Init program            |
| `/var/centauri/                      ` | Plugin directory        |

### Shared Platform

#### Windows
| Path                                                   | Description           |
| ------------------------------------------------------ | --------------------- |
| `%APPDATA%\Centauri\                                 ` | User plugin directory |
| `%ProgramData%\Centauri\                             ` | Plugin directory      |
| `%ProgramFiles%\Centauri\Centauri.IoC.Api.dll        ` | Centauri DLL          |
| `%ProgramFiles%\Centauri\Centauri.IoC.Framework.dll  ` | Centauri DLL          |
| `%ProgramFiles%\Centauri\Centauri.Platform.Shared.dll` | Centauri executable   |
| `%USERPROFILE%\                                      ` | User data             |

#### Mac
| Path                                                                     | Description           |
| ------------------------------------------------------------------------ | --------------------- |
| `~                                                                     ` | User data             |
| `~/Library/Application Support/Centauri/                               ` | User plugin directory |
| `/Applications/Centauri.app/Contents/MacOS/Centauri.IoC.Api.dll        ` | Centauri DLL          |
| `/Applications/Centauri.app/Contents/MacOS/Centauri.IoC.Framework.dll  ` | Centauri DLL          |
| `/Applications/Centauri.app/Contents/MacOS/Centauri.Platform.Shared.dll` | Centauri executable   |
| `/Applications/Centauri.app/Contents/MacOS/wrapper                     ` | Launcher executable   |
| `/Library/Application Support/Centauri/                                ` | Plugin directory      |

#### Other
| Path                                                   | Description           |
| ------------------------------------------------------ | --------------------- |
| `~                                                   ` | User data             |
| `~/.local/centauri/                                  ` | User plugin directory |
| `/usr/share/centauri/bin/Centauri.IoC.Api.dll        ` | Centauri DLL          |
| `/usr/share/centauri/bin/Centauri.IoC.Framework.dll  ` | Centauri DLL          |
| `/usr/share/centauri/bin/Centauri.Platform.Shared.dll` | Centauri executable   |
| `/usr/share/centauri/plugins/                        ` | Plugin directory      |
