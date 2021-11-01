/********************************************************************
    created:	2018/8/27 17:23:21
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
	public static class Md5HashEx
	{
		public static MD5 Md5 = MD5.Create();

		public static string GetMd5Hash(byte[] input)
		{
			byte[] data = Md5.ComputeHash(input);

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for(int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
			
		}
		public static string GetMd5Hash(string input)
		{
			return GetMd5Hash(Encoding.UTF8.GetBytes(input));
		}

		public static string GetMd5HashFromFile(string file)
		{
			if (!File.Exists(file))
				return null;

		    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
		    {
		        using (BinaryReader sr = new BinaryReader(fs, System.Text.Encoding.Default))
		        {
		            return GetMd5Hash(sr.ReadBytes((int)fs.Length));
		        }
		    }
		}

		
		public static bool VerifyMd5Hash(byte[] input, string hash)			// Verify a hash against a string.
		{
			// Hash the input.
			string hashOfInput = GetMd5Hash(input);

			// Create a StringComparer an compare the hashes.
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			if(0 == comparer.Compare(hashOfInput, hash))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool	VerifyMd5Hash(string input, string hash)			// Verify a hash against a string.
		{
			return VerifyMd5Hash(Encoding.UTF8.GetBytes(input), hash);

		}
		public static bool	VerifyFileMd5Hash(string path, string hash)			// Verify a hash against a string.
		{
			if (!File.Exists(path))
				return false;

			return VerifyMd5Hash(File.ReadAllBytes(path), hash);
		}

		public static bool Equal(string left, string right)
		{
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;
			if(0 == comparer.Compare(left, right))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
