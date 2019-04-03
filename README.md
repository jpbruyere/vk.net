# vk.net

This repo is composed of three projects in early development state.

## vk.net
Low level Vulkan c# binding on the model of OpenTK Wrapper with a rewrited DLL.
The original bindings has been made by mellinoe@gmail.com (https://github.com/mellinoe) with a generator. I've copy the generated c# bindings and modified them by hand removing lot's of uneeded overrides, trying to limit the use of unsafe pointers to replace them by IntPtr where possible or ref/out decorations. Some structures have also been modified this way, adding also some #ctors.
Lot's of vkFunctions are missing for now, I add them when required while developing some samples.
I plan to rework the rewriter, maybe remove it to let default marshaler do it's job (depending on future perf study), and maybe try to make a new generator limiting it's overrides count.

## VKE : VulKan Engine (good name still to be found for it)
higher c# classes to simplify vulkan apps programmation with IDispose model, My plan is to have good classes for a model/view 3d editor, but the base classes should still target speed.
Multiplatform window handling is done through GLFW and gltfNet/KTX/stbi libs stands for models loadings.
Consider this as R&D.
## Samples
Lot's of the work is done with Sachawillems examples in c++ nearby.


### Current development environment

=== MonoDevelop ===

Version 7.7 (build 1869)
Installation UUID: 3b8b3878-f97c-4a95-b5ff-26a145ec2172
	GTK+ 2.24.31 (Xamarin-Dark theme)

=== Mono Framework MDK ===

Runtime:
	Mono 5.16.0.220 (tarball Mon Nov 26 17:15:30 UTC 2018) (64-bit)

=== NuGet ===

Version : 4.7.0.5148

=== .NET Core ===

Runtime : /usr/share/dotnet/dotnet
Versions du runtime :
	2.2.0
	2.0.9
	2.0.7
SDK : /usr/share/dotnet/sdk/2.2.100/Sdks
Versions du SDK :
	2.2.100
	2.1.202
	2.1.105
SDK MSBuild : /usr/lib/mono/msbuild/15.0/bin/Sdks

=== Build Information ===

Release ID: 707001869
Git revision: f0f3d1d931d44682f076dde486ecec835f705b8d
Build date: 2018-11-29 14:54:14-05

=== Operating System ===

Linux
Linux 4.19.0-1-amd64 #1 SMP Debian 4.19.12-1 (2018-12-22) x86_64

=== Enabled user installed extensions ===

AddinMaker 1.4.3

