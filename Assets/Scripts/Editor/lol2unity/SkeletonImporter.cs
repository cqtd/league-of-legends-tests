using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace Lol2Unity
{
	public class SkeletonImporter : Importer
	{
		public List<SkinBone> bones = default;
		public List<int> boneIndices = default;

		public int numBones;
		public int numBoneIndices;
		
		public Dictionary<uint, char[]> boneHashes = default;

		private int fileVersion;
		private readonly short skinVersion;

		private const int FILE_VERSION_1 = 1953262451;
		private const int FILE_VERSION_2 = 587026371;

		public SkeletonImporter(short fileVersion)
		{
			this.skinVersion = fileVersion;
		}
		
		public bool ReadFile(string path)
		{
			BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));

			Seek(4);
			fileVersion = reader.ReadInt32();

			if (fileVersion != FILE_VERSION_1 && fileVersion != FILE_VERSION_2)
			{
				throw new NotSupportedVersionException();
			}
			
			boneHashes = new Dictionary<uint, char[]>();

			if (fileVersion == FILE_VERSION_1)
			{
				Logger.Log("FILE VERSION 1");

				Seek(8);

				numBones = reader.ReadInt32();
				bones = new List<SkinBone>(numBones);


				for (int i = 0; i < numBones; i++)
				{
					SkinBone bone = new SkinBone();

					bone.name = reader.ReadChars(SkinBone.NAME_SIZE);
					bone.hash = StringToHash(bone.name);

					boneHashes[bone.hash] = bone.name;

					bone.parent = reader.ReadInt32();
					float scale = reader.ReadSingle() * 10f;
					bone.scale = new float3
					{
						x = scale,
						y = scale,
						z = scale,
					};

					for (int j = 0; j < 3; j++)
					{
						for (int k = 0; k < 3; k++)
						{
							bone.globalMatrix[j, 3] = reader.ReadSingle();
						}
					}

					for (int j = 0; j < 3; j++)
					{
						bone.globalMatrix[3, j] = 0f;
					}

					bone.globalMatrix[3, 3] = 1f;

					bones.Add(bone);
				}

				foreach (SkinBone bone in bones)
				{
					if (bone.parent != -1)
					{
						Matrix4x4 inverseMatrix = bones[bone.parent].globalMatrix.GjInverse();
						bone.localMatrix = inverseMatrix * bone.globalMatrix;
					}
					else
					{
						bone.localMatrix = bone.globalMatrix;
					}
				}

				if (skinVersion == 0 || skinVersion == 1)
				{
					numBoneIndices = numBones;
					boneIndices = new List<int>(numBoneIndices);

					for (int i = 0; i < numBoneIndices; i++)
					{
						boneIndices.Add(i);
					}
				}
				else if (skinVersion == 2)
				{
					numBoneIndices = reader.ReadInt32();
					boneIndices = new List<int>(numBoneIndices);

					for (int i = 0; i < numBoneIndices; i++)
					{
						int index = reader.ReadInt32();
						boneIndices.Add(index);
					}
				}
			}
			else if (fileVersion == FILE_VERSION_2)
			{
				Logger.Log("FILE VERSION 2");
				
				Seek(6);

				numBones = (short) reader.ReadInt16();
				bones = new List<SkinBone>(numBones);

				numBoneIndices = reader.ReadInt32();
				boneIndices = new List<int>(numBoneIndices);

				short dataOffset = reader.ReadInt16();
				Seek(2);

				Seek(4);
				int boneIndicesOffset = reader.ReadInt32();
				
				Seek(8);
				int boneNamesOffset = reader.ReadInt32();

				Seek(dataOffset, SeekOrigin.Begin);

				for (int i = 0; i < numBones; i++)
				{
					Seek(2);

					short boneId = reader.ReadInt16();

					if (boneId != i)
					{
						throw new UnidentifiedBoneException();
					}
					
					SkinBone bone = new SkinBone();

					bone.parent = (short) reader.ReadInt16();
					
					
					bone.name = new char[SkinBone.NAME_SIZE];
					
					Seek(2);

					bone.hash = reader.ReadUInt32();
					boneHashes[bone.hash] = bone.name;
					
					// Logger.Log($"[{i}] :: {bone.hash} :: {bone.parent}");

					Seek(4);

					float tx = reader.ReadSingle();
					float ty = reader.ReadSingle();
					float tz = reader.ReadSingle();

					bone.scale = new float3()
					{
						x = reader.ReadSingle(),
						y = reader.ReadSingle(),
						z = reader.ReadSingle(),
					};

					quaternion q = new quaternion
					{
						value = new float4()
						{
							x = reader.ReadSingle(),
							y = reader.ReadSingle(),
							z = reader.ReadSingle(),
							w = reader.ReadSingle(),
						}
					};

					bone.localMatrix = q.ToMatrix4X4();
					bone.localMatrix = bone.localMatrix.GjInverse();

					bone.localMatrix[0, 3] = tx;
					bone.localMatrix[1, 3] = ty;
					bone.localMatrix[2, 3] = tz;

					bone.localMatrix[3, 0] = 0f;
					bone.localMatrix[3, 1] = 0f;
					bone.localMatrix[3, 2] = 0f;
					bone.localMatrix[3, 3] = 1f;
					
					bones.Add(bone);

					Seek(44);
				}

				foreach (SkinBone bone in bones)
				{
					if (bone.parent != -1)
					{
						bone.globalMatrix = bones[bone.parent].globalMatrix * bone.localMatrix;
					}
					else
					{
						bone.globalMatrix = bone.localMatrix;
					}
				}
				
				Seek(boneIndicesOffset, SeekOrigin.Begin);

				for (int i = 0; i < numBoneIndices; i++)
				{
					short index = reader.ReadInt16();
					boneIndices.Add(index);
				}
				
				Seek(boneNamesOffset, SeekOrigin.Begin);

				// bone name logic failed
				for (int i = 0; i < numBones; i++)
				{
					long filePos;
					char current;
					
					do
					{
						filePos = reader.BaseStream.Position;
						current = reader.ReadChar();
					} while (filePos % 4 != 0);

					int currentPos = 0;

					while (current != '\0')
					{
						bones[i].name[currentPos] = current;
						currentPos++;

						if (currentPos >= 31)
						{
							break;
						}

						current = reader.ReadChar();
					}

					bones[i].name[currentPos] = '\0';
				}
			}
			
			reader.Close();

			Logger.Log($"bones : {bones.Count}");
			Logger.Log($"bone Indices : {boneIndices.Count}");
							
			foreach (char[] boneName in boneHashes.Values)
			{
				string name = new string(boneName);
					
				if (!string.IsNullOrEmpty(name))
				{
					Logger.Log($"bone : {name}");
				}
			}

			void Seek(int offset, SeekOrigin origin = SeekOrigin.Current)
			{
				reader.BaseStream.Seek(offset, origin);
			}
			
			return true;
		}

		public class SkeletonImporterException : Exception { }

		public class NotSupportedVersionException : SkeletonImporterException { }
		
		public class UnidentifiedBoneException : SkeletonImporterException { }
	}
}