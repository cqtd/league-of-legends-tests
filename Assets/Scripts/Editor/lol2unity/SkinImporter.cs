using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;

namespace Lol2Unity
{
	public class SkinImporter : Importer
	{
		public short fileVersion = default;

		public List<short> indicies = default;
		public List<SkinVertex> vertices = default;

		public int numIndices;
		public int numVertices;

		private int magicNumber;
		private short matHeader;
		private int numMaterials;

		private const int MAGIC = 1122867;
		
		public SkinImporter()
		{
			
		}

		~SkinImporter()
		{
			indicies = null;
			vertices = null;
		}

		public bool ReadFile(string path)
		{
			BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));
			
			magicNumber = reader.ReadInt32();
			if (magicNumber != MAGIC)
			{
				throw new NotCompatibleMagicNumberException();
			}
			
			fileVersion = reader.ReadInt16();
			if (fileVersion > 4)
			{
				throw new NotSupportedVersionException();
			}
			
			matHeader = reader.ReadInt16();
			if (matHeader != 1)
			{
				throw new NotParsableFileException();
			}

			if (fileVersion > 0)
			{
				numMaterials = reader.ReadInt32();

				for (int i = 0; i < numMaterials; i++)
				{
					reader.BaseStream.Seek(80, SeekOrigin.Current);
				}
			}

			if (fileVersion == 4)
			{
				reader.BaseStream.Seek(4, SeekOrigin.Current);
			}

			numIndices = reader.ReadInt32();
			numVertices = reader.ReadInt32();
			
			if (fileVersion == 4)
			{
				reader.BaseStream.Seek(48, SeekOrigin.Current);
			}

			indicies = new List<short>(numIndices);
			for (int i = 0; i < numIndices; i++)
			{
				short index = reader.ReadInt16();
				indicies.Add(index);
			}

			vertices = new List<SkinVertex>(numVertices);
			for (int i = 0; i < numVertices; i++)
			{
				SkinVertex vertex = new SkinVertex
				{
					position = new float3()
					{
						x = (float) reader.ReadSingle(),
						y = (float) reader.ReadSingle(),
						z = (float) reader.ReadSingle(),
					},
					boneIndices = reader.ReadChars(4),
					boneWeights = new[]
					{
						reader.ReadSingle(),
						reader.ReadSingle(),
						reader.ReadSingle(),
						reader.ReadSingle(),
					},
					normal = new float3()
					{
						x = reader.ReadSingle(),
						y = reader.ReadSingle(),
						z = reader.ReadSingle(),
					},
					u = reader.ReadSingle(),
					v = reader.ReadSingle(),
				};

				vertex.v = 1 - vertex.v;
				vertices.Add(vertex);
			}

			foreach (SkinVertex vertex in vertices)
			{
				float totalWeight =
					vertex.boneWeights[0]
					+ vertex.boneWeights[1]
					+ vertex.boneWeights[2]
					+ vertex.boneWeights[3];

				float weightError = 1.0f - totalWeight;
				if (weightError != 0.0f)
				{
					for (int i = 0; i < 4; i++)
					{
						if (vertex.boneWeights[i] > 0.0f)
						{
							vertex.boneWeights[i] += (vertex.boneWeights[i] / totalWeight) * weightError;
						}
					}
				}
			}
			
			reader.Close();
			return true;
		}

		public class SkinImporterException : Exception
		{
			
		}

		public class NotCompatibleMagicNumberException : SkinImporterException
		{
			
		}
		
		public class NotSupportedVersionException : SkinImporterException
		{
			
		}

		public class NotParsableFileException : SkinImporterException
		{
			
		}
	}
}