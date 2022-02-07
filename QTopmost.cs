// QTopmost v1.0 (c) 2022 Sensei (aka 'Q')
// Make console window topmost on/off.
//
// Usage:
// QTopmost [-h|--help|/?] [-o|-off|--off]
//
// Examples:
// QTopmost
// QTopmost --off
//
// Compilation:
// %SYSTEMROOT%\Microsoft.NET\Framework\v3.5\csc QTopmost.cs

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public static class QTopmost {
   public static class Kernel32 {
      [DllImport("kernel32.dll")]
      public static extern IntPtr GetConsoleWindow();
   }

   public static class User32 {
      [DllImport("user32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool SetWindowPos( IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags );

      public static IntPtr HWND_TOP       = new IntPtr(0);
      public static IntPtr HWND_TOPMOST   = new IntPtr(-1);
      public static IntPtr HWND_NOTOPMOST = new IntPtr(-2);
      public static IntPtr HWND_BOTTOM    = new IntPtr(-3);
      public const UInt32 SWP_NOSIZE = 0x0001;
      public const UInt32 SWP_NOMOVE = 0x0002;
      public const UInt32 SWP_SHOWWINDOW = 0x0040;
   }

   private static void Topmost( IntPtr hWnd, bool enabled ) {
      IntPtr hWndInsertAfter = enabled ? User32.HWND_TOPMOST : User32.HWND_NOTOPMOST;
      bool state = User32.SetWindowPos( hWnd, hWndInsertAfter, 0, 0, 0, 0, User32.SWP_NOMOVE | User32.SWP_NOSIZE | User32.SWP_SHOWWINDOW );
      if( state ) {
         if( enabled ) {
            Console.WriteLine( "Window is topmost!" );
         } else {
            Console.WriteLine( "Window is not topmost!" );
         }
      } else {
         Console.Error.WriteLine( "Error! hWnd {0:X}", hWnd );
      }
   }

   private static IntPtr GetActiveWindow() {
      IntPtr hWnd = Kernel32.GetConsoleWindow();
      //IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle; // Doesn't seem to be working. It returns null.
      return( hWnd );
   }

   private static void Help() {
      Console.WriteLine( "QTopmost v1.0 (c) 2022 Sensei (aka 'Q')" );
      Console.WriteLine( "Make console window topmost on/off." );
      Console.WriteLine();
      Console.WriteLine( "Usage:" );
      Console.WriteLine( "QTopmost [-h|--help|/?] [-o|-off|--off]" );
      Console.WriteLine( "Examples:" );
      Console.WriteLine( "QTopmost" );
      Console.WriteLine( "QTopmost --off" );
   }

   public static void Main( string [] args ) {
      IntPtr hWnd = GetActiveWindow();
      if( args.Length == 1 ) {
         string arg = args[0];
         if( arg.Equals( "-o" ) || arg.Equals( "-off" ) || arg.Equals( "--off" ) ) {
            Topmost( hWnd, false );
         } else if( arg.Equals( "-h" ) || arg.Equals( "--help" ) || arg.Equals( "/?" ) ) {
            Help();
         } else {
            Console.Error.WriteLine( "Unknown argument {0}", arg );
            System.Environment.Exit( 20 );
         }
      } else if( args.Length == 0 ) {
         Topmost( hWnd, true );
      } else {
         Help();
      }
   }
}
