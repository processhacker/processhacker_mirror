// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbeck�" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2284 $</version>
// </file>

#pragma warning disable 1591

using System;
using System.Runtime.InteropServices;

namespace Debugger.Wrappers
{
	public delegate void UnmanagedStringGetter(uint pStringLenght, out uint stringLenght, System.IntPtr pString);
	
	public static class Util
	{
		public static string GetString(UnmanagedStringGetter getter)
		{
			return GetString(getter, 64, true);
		}
		
		public static string GetString(UnmanagedStringGetter getter, uint defaultLenght, bool trim)
		{
			string managedString;
			IntPtr unmanagedString;
			uint exactLenght;
			
			// First attempt
			unmanagedString = Marshal.AllocHGlobal((int)defaultLenght * 2 + 2); // + 2 for terminating zero
			getter(defaultLenght, out exactLenght, defaultLenght > 0 ? unmanagedString : IntPtr.Zero);
			
			if(exactLenght > defaultLenght) {
				// Second attempt
				Marshal.FreeHGlobal(unmanagedString);
				unmanagedString = Marshal.AllocHGlobal((int)exactLenght * 2 + 2); // + 2 for terminating zero
				getter(exactLenght, out exactLenght, unmanagedString);
			}
			
			// Return managed string and free unmanaged memory
			managedString = Marshal.PtrToStringUni(unmanagedString, (int)exactLenght);
			//Console.WriteLine("Marshaled string from COM: \"" + managedString + "\" lenght=" + managedString.Length + " arrayLenght=" + exactLenght);
			// The API might or might not include terminating null at the end
			if (trim) {
				managedString = managedString.TrimEnd('\0');
			}
			Marshal.FreeHGlobal(unmanagedString);
			return managedString;
		}
	}
}

#pragma warning restore 1591
