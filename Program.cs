using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MakeLower;

static class Program
{
	static void Main(string[] args)
	{
		string workingDir = Environment.CurrentDirectory;

		if (!Directory.Exists(workingDir))
		{
			throw new ArgumentException("Working directory does not exist!");
		}

		bool shouldRecurse = args.Any(o => o.Equals("-recurse"));

		uint fileCount = 0;
		uint operationCount = 0;
		foreach (var filePath in Directory.EnumerateFiles(workingDir, "*", shouldRecurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
		{
			if (args.Length > 0 && (args[0] == "ext" || args[0] == "extension"))
			{
				File.Move(filePath, Path.ChangeExtension(filePath, Path.GetExtension(filePath).ToLowerInvariant()));
				operationCount++;
			}
			else if (args.Length > 0 && args[0] == "name")
			{
				File.Move(filePath, Path.Combine(Path.GetDirectoryName(filePath)!, Path.GetFileNameWithoutExtension(filePath).ToLowerInvariant()) + Path.GetExtension(filePath));
				operationCount++;
			}
			else if (args.Length == 0 || args[0] == "full")
			{
				File.Move(filePath, Path.Combine(Path.GetDirectoryName(filePath)!, Path.GetFileName(filePath).ToLowerInvariant()));
				operationCount++;
			}

			fileCount++;
		}

		if (operationCount == fileCount)
		{
			Console.WriteLine($"Renamed {operationCount} files");
		}
		// Must've been given invalid arguments (or/including -h), print usage
		else
		{
			string processName = Path.GetFileNameWithoutExtension(Environment.ProcessPath)!;
			Console.WriteLine($"Usage: {processName} <operation> [options]");
			Console.WriteLine("\nOperations:");
			Console.WriteLine("  ext|extension\t\tMake only extensions lowercase.");
			Console.WriteLine("  name\t\t\tMake only names lowercase, excluding extensions.");
			Console.WriteLine("  full\t\t\tMake both names and extensions lowercase.");

			Console.WriteLine("\nOptions:");
			Console.WriteLine("  -recurse\t\tRename files recursively.");
		}
	}
}