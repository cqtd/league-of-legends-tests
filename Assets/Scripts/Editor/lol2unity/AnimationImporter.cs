using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace Lol2Unity
{
	public class AnimationImporter : Importer
	{
		public int numBones;
		public int numFrames;
		public float frameDelay;
		public List<AnimationBone> bones;

		private Dictionary<uint, char[]> boneHashes;

		private const float DIV = 65535.0f;
		private const float DIV3 = 32768.0f;
		
		private const int DIV2 = 16384;

		private const string MAGIC_1 = "r3d2anmd";
		private const string MAGIC_2 = "r3d2canm";
		
		public AnimationImporter(Dictionary<uint, char[]> boneHashMap)
		{
			this.boneHashes = boneHashMap;
		}

		private quaternion UncompressQuaternion(ushort flag, ushort sx, ushort sy, ushort sz)
		{
			float fx = Mathf.Sqrt(2.0f) * ((int)sx - DIV2) / DIV3;
			float fy = Mathf.Sqrt(2.0f) * ((int)sx - DIV2) / DIV3;
			float fz = Mathf.Sqrt(2.0f) * ((int)sx - DIV2) / DIV3;
			
			float fw = Mathf.Sqrt(1.0f - fx * fx - fy * fy - fz * fz);

			quaternion uncompressed = new quaternion();
			switch (flag)
			{
				case 0:
					uncompressed.value = new float4(fw, fx, fy, fz);
					break;
				case 1:
					uncompressed.value = new float4(fx, fw, fy, fz);
					break;
				case 2:
					uncompressed.value = new float4(-fx, -fy, -fw, -fz);
					break;
				case 3:
					uncompressed.value = new float4(fx, fy, fz, fw);
					break;
			}

			return uncompressed;
		}

		private float3 UncompressVector(float3 min, float3 max, ushort sx, ushort sy, ushort sz)
		{
			float3 uv = max - min;

			uv.x *= (sx / DIV);
			uv.y *= (sy / DIV);
			uv.z *= (sz / DIV);

			uv += min;

			return uv;
		}

		private float UncompressTime(ushort ct, float animationLength)
		{
			float ut = ct / DIV;
			ut *= animationLength;

			return ut;
		}
		
		public bool ReadFile(string path)
		{
			BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));

			string magicNumber = new string(reader.ReadChars(8));
			
			if (string.CompareOrdinal(magicNumber, MAGIC_1) != 0
			    && string.CompareOrdinal(magicNumber, MAGIC_2) != 0)
			{
				throw new NotCompatibleMagicNumberException();
			}

			int version = reader.ReadInt32();
			if (version > 5)
			{
				throw new NotSupoortedVersionException();
			}

			if (version == 1)
			{
				int fileSize = reader.ReadInt32() + 12;
				Seek(8);

				numBones = reader.ReadInt32();

				int numEntries = reader.ReadInt32();
				Seek(4);

				float animationLength = reader.ReadSingle();
				float framesPerSecond = reader.ReadSingle();

				numFrames = (int) (animationLength * framesPerSecond);
				frameDelay = 1.0f / framesPerSecond;
				
				Seek(24);

				float3 minTranslation = new float3(
					reader.ReadSingle(),
					reader.ReadSingle(),
					reader.ReadSingle()
				);
				
				float3 maxTranslation = new float3(
					reader.ReadSingle(),
					reader.ReadSingle(),
					reader.ReadSingle()
				);
				
				float3 minScale = new float3(
					reader.ReadSingle(),
					reader.ReadSingle(),
					reader.ReadSingle()
				);
				
				float3 maxScale = new float3(
					reader.ReadSingle(),
					reader.ReadSingle(),
					reader.ReadSingle()
				);

				int entriesOffset = reader.ReadInt32() + 12;
				int indicesOffset = reader.ReadInt32() + 12;
				int hashesOffset = reader.ReadInt32() + 12;

				List<uint> hashEntries = new List<uint>();
				
				Seek(hashesOffset, SeekOrigin.Begin);

				for (int i = 0; i < numBones; i++)
				{
					uint hashEntry = reader.ReadUInt32();
					hashEntries.Add(hashEntry);
				}
				
				Seek(entriesOffset, SeekOrigin.Begin);

				const ushort quaternionType = 0;
				const ushort translationType = 64;
				const ushort scaleType = 128;
				
				
			}
			
			
			reader.Close();
			
			void Seek(int offset, SeekOrigin origin = SeekOrigin.Current)
			{
				reader.BaseStream.Seek(offset, origin);
			}
			
			return true;
		}
		
		public class AnimationImportException : Exception
		{
			
		}
		
		public class NotCompatibleMagicNumberException : AnimationImportException
		{
			
		}
		
		public class NotSupoortedVersionException : AnimationImportException
		{
			
		}
	}
}