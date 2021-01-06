using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Lol2Unity
{
	public class ConverterTest
	{
		[MenuItem("Tool/LOL 2 Unity/Test")]
		private static void Test()
		{
			const string folder = "C:\\Github\\League of Legends\\Root\\assets\\characters\\garen\\skins\\base";
            
			const string skn = folder + "\\garen.skn";
			const string skl = folder + "\\garen.skl";
            
			const string animationFolderPath = folder + "\\animations";
			
			DirectoryInfo animationFolder = new DirectoryInfo(animationFolderPath);
			var animations = animationFolder.GetFiles("*.anm");

			new Converter()
				
				.SetSkin(skn)
				.SetSkeleton(skl)
				.SetOutputFolder(folder)
				.SetAnimations(animations.Select(e => e.FullName))
				.Work();
			
			// Skin
			// material : 1
			// index : 19008
			// vertices : 3932
		}
	}

	public static class MathHelper
	{
		public static Matrix4x4 GjInverse(this Matrix4x4 matrix)
		{
			int i, j, k;
			
			Matrix4x4 s = Matrix4x4.zero;
			Matrix4x4 t = matrix;

			// Forward elimination

			for (i = 0; i < 3 ; i++)
			{
				int pivot = i;

				float pivotsize = t[i,i];

				if (pivotsize < 0)
					pivotsize = -pivotsize;

				for (j = i + 1; j < 4; j++)
				{
					float tmp = t[j,i];

					if (tmp < 0)
						tmp = -tmp;

					if (tmp > pivotsize)
					{
						pivot = j;
						pivotsize = tmp;
					}
				}

				if (pivotsize == 0)
				{
					return Matrix4x4.zero;
				}

				if (pivot != i)
				{
					for (j = 0; j < 4; j++)
					{
						float tmp;

						tmp = t[i,j];
						t[i,j] = t[pivot,j];
						t[pivot,j] = tmp;

						tmp = s[i,j];
						s[i,j] = s[pivot,j];
						s[pivot,j] = tmp;
					}
				}

				for (j = i + 1; j < 4; j++)
				{
					float f = t[j,i] / t[i,i];

					for (k = 0; k < 4; k++)
					{
						t[j,k] -= f * t[i,k];
						s[j,k] -= f * s[i,k];
					}
				}
			}

			// Backward substitution

			for (i = 3; i >= 0; --i)
			{
				float f;

				if ((f = t[i,i]) == 0)
				{
					return Matrix4x4.zero;
				}

				for (j = 0; j < 4; j++)
				{
					t[i,j] /= f;
					s[i,j] /= f;
				}

				for (j = 0; j < i; j++)
				{
					f = t[j,i];

					for (k = 0; k < 4; k++)
					{
						t[j,k] -= f * t[i,k];
						s[j,k] -= f * s[i,k];
					}
				}
			}

			return s;
		}

		public static Matrix4x4 ToMatrix4X4(this quaternion q)
		{
			float4 v = q.value;

			return new Matrix4x4(new float4(1 - 2 * (v.y * v.y + v.z * v.z),
					2 * (v.x * v.y + v.z * v.w),
					2 * (v.z * v.x - v.y * v.w),
					0),
				new float4(2 * (v.x * v.y - v.z * v.w),
					1 - 2 * (v.z * v.z + v.x * v.x),
					2 * (v.y * v.z + v.x * v.w),
					0),
				new float4(2 * (v.z * v.x + v.y * v.w),
					2 * (v.y * v.z - v.x * v.w),
					1 - 2 * (v.y * v.y + v.x * v.x),
					0),
				new float4(0,
					0,
					0,
					1)
			);
		}
	}
}