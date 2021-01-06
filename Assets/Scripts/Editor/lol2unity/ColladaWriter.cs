using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Lol2Unity
{
	public class ColladaWriter
	{
		public enum Mode
		{
			Mesh = 1,
			Skeleton,
			Animation,
		}

		private List<short> m_indices;
		private List<SkinVertex> m_vertices;
		private List<SkinBone> m_bones;
		private List<int> m_boneIndices;
		private AnimationImporter m_animImporter;

		public ColladaWriter SetIndecies(List<short> indices)
		{
			this.m_indices = indices;
			return this;
		}
		
		public ColladaWriter SetVertices(List<SkinVertex> vertices)
		{
			this.m_vertices = vertices;
			return this;
		}
		
		public ColladaWriter SetBones(List<SkinBone> bones)
		{
			this.m_bones = bones;
			return this;
		}
		
		public ColladaWriter SetBoneIndices(List<int> boneIndices)
		{
			this.m_boneIndices = boneIndices;
			return this;
		}
		
		public ColladaWriter SetAnimationImporter(AnimationImporter animImporter)
		{
			this.m_animImporter = animImporter;
			return this;
		}

		public bool WriteFile(string path)
		{
			return true;
		}
	}

	public struct SkinVertex
	{
		public float3 position;
		public char[] boneIndices;
		public float[] boneWeights;
		public float3 normal;
		public float u;
		public float v;
	}

	public class SkinBone
	{
		public char[] name;
		public uint hash;
		public int parent;
		public float3 scale;
		public Matrix4x4 localMatrix;
		public Matrix4x4 globalMatrix;

		public const int NAME_SIZE = 32;
	}

	public class AnimationBone
	{
		public char[] name;
		public List<(float, float3)> translations;
		public List<(float, quaternion)> quaternions;
		public List<(float, float3)> scales;
	}
}