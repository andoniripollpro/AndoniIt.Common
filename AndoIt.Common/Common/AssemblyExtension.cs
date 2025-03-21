﻿using System;
using System.IO;
using System.Reflection;

namespace AndoIt.Common
{
	public static class AssemblyExtension
	{
		public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
		{
			var filePath = assembly.Location;
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;

			var buffer = new byte[2048];

			using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				stream.Read(buffer, 0, 2048);

			var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
			var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

			var tz = target ?? TimeZoneInfo.Local;
			var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

			return localTime;
		}

		public static DateTime GetAssemblyCompilationTime(this Assembly assembly, TimeZoneInfo target = null)
		{
			string codeBase = assembly.GetName().CodeBase;
			string fullPath = codeBase.Replace("file:///", string.Empty);
			DateTime result = File.GetLastWriteTime(fullPath);
			return result;
		}

		public static string AssemblyDirectory(this Assembly assembly)
		{
			UriBuilder uri = new UriBuilder(assembly.CodeBase);
			string path = Uri.UnescapeDataString(uri.Path);
			return Path.GetDirectoryName(path);
		}

	}
}
