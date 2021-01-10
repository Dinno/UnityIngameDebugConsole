using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Container for a simple debug entry
namespace IngameDebugConsole
{
	public class DebugLogEntry : System.IEquatable<DebugLogEntry>
	{
		private const int HASH_NOT_CALCULATED = -623218;

		public string logString;
		public string stackTrace;

		private string completeLog;

		// Sprite to show with this entry
		public Sprite logTypeSpriteRepresentation;

		// Collapsed count
		public int count;

		private byte[] hashValue;

		private static MD5 Md5 = MD5.Create();

        public void Initialize( string logString, string stackTrace )
		{
			this.logString = logString;
			this.stackTrace = stackTrace;

			completeLog = null;
			count = 1;
			hashValue = null;
		}

		// Check if two entries have the same origin
		public bool Equals( DebugLogEntry other )
		{
			return GetMD5Hash().SequenceEqual(other.GetMD5Hash());
		}

		// Checks if logString or stackTrace contains the search term
		public bool MatchesSearchTerm( string searchTerm )
		{
			return ( logString != null && logString.IndexOf( searchTerm, System.StringComparison.OrdinalIgnoreCase ) >= 0 ) ||
				( stackTrace != null && stackTrace.IndexOf( searchTerm, System.StringComparison.OrdinalIgnoreCase ) >= 0 );
		}

		// Return a string containing complete information about this debug entry
		public override string ToString()
		{
			if( completeLog == null )
				completeLog = string.Concat( logString, "\n", stackTrace );

			return completeLog;
		}

		public override int GetHashCode()
		{
			return BitConverter.ToInt32(GetMD5Hash(), 0);
		}

		public byte[] GetMD5Hash()
		{
			if (hashValue == null)
			{
				hashValue = Md5.ComputeHash(Encoding.ASCII.GetBytes(logString + stackTrace));
			}

			return hashValue;
		}
	}

	public struct QueuedDebugLogEntry
	{
		public readonly string logString;
		public readonly string stackTrace;
		public readonly LogType logType;

		public QueuedDebugLogEntry( string logString, string stackTrace, LogType logType )
		{
			this.logString = logString;
			this.stackTrace = stackTrace;
			this.logType = logType;
		}

		// Checks if logString or stackTrace contains the search term
		public bool MatchesSearchTerm( string searchTerm )
		{
			return ( logString != null && logString.IndexOf( searchTerm, System.StringComparison.OrdinalIgnoreCase ) >= 0 ) ||
				( stackTrace != null && stackTrace.IndexOf( searchTerm, System.StringComparison.OrdinalIgnoreCase ) >= 0 );
		}
	}
}