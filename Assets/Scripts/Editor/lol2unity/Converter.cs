using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lol2Unity
{
    public class Converter
    {
        private string m_skinPath = default;
        private string m_skeletonPath = default;
        private string m_outputFolder = default;
        private List<string> m_animations = default;

        private bool m_includeSkeleton = default;
        
        public Converter()
        {
            
        }

        public Converter SetSkin(string path)
        {
            this.m_skinPath = path;
            return this;
        }

        public Converter SetSkeleton(string path)
        {
            this.m_includeSkeleton = true;
            this.m_skeletonPath = path;
            return this;
        }

        public Converter SetOutputFolder(string folderPath)
        {
            this.m_outputFolder = folderPath;
            return this;
        }

        public Converter SetAnimations(IEnumerable<string> paths)
        {
            this.m_animations = paths?.ToList();
            
            return this;
        }

        public void Work()
        {
            ColladaWriter.Mode mode = m_includeSkeleton ? ColladaWriter.Mode.Skeleton : ColladaWriter.Mode.Mesh;
            
            SkinImporter skinImporter = new SkinImporter();
            skinImporter.ReadFile(m_skinPath);
            
            SkeletonImporter skelImporter = new SkeletonImporter(skinImporter.fileVersion);
            AnimationImporter animImporter = new AnimationImporter(skelImporter.boneHashes);

            if (mode >= ColladaWriter.Mode.Skeleton)
            {
                skelImporter.ReadFile(m_skeletonPath);

                if (m_animations != null)
                {
                    mode = ColladaWriter.Mode.Animation;
                    ColladaWriter writer = new ColladaWriter()
                        .SetIndecies(skinImporter.indicies)
                        .SetVertices(skinImporter.vertices)
                        .SetBones(skelImporter.bones)
                        .SetBoneIndices(skelImporter.boneIndices);

                    foreach (string animation in m_animations)
                    {
                        animImporter.ReadFile(animation);
                        writer.SetAnimationImporter(animImporter);
                        
                        string outputPath = m_outputFolder + "\\" + new FileInfo(animation).Name;
                        writer.WriteFile(outputPath.Replace(".anm", ".dae"));
                    }
                }
            }
        }
    }
}